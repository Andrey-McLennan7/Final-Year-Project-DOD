using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct BunkerProjectileCollisionResponseSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalTransform> bunkerLocalTransform, RefRO <LocalToWorld> bunkerLocalToWorld, RefRW<BunkerHealth> bunkerHealth, RefRO<BoxCollider> bunkerBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<LocalToWorld>, RefRW<BunkerHealth>, RefRO<BoxCollider>>())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Projectile>())
            {
                // Skip the code if no collision is detected
                if (!BoxCollisionResponseSystem.OnCollisionResponse(bunkerLocalTransform, bunkerBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                // Reduce the health
                --bunkerHealth.ValueRW.health;

                // Calculate the percentage of health
                float normalisedHealth = (float)bunkerHealth.ValueRO.health / (float)bunkerHealth.ValueRO.maxHealth;

                /// <summary>
                /// 
                /// Unity DOTS does not provide access to changing a single value of
                /// of the scale componens, as it is determined by a single float value
                /// and not a 3D vector
                /// 
                /// This is why you need the Post Transform Matrix because it renders the
                /// geometry with nonuniform scale
                /// 
                /// </summary>

                // Get reference to the Post Transform Matrix component of the Entity
                RefRW<PostTransformMatrix> bunkerHealthBarTransformMatrix =
                    SystemAPI.GetComponentRW<PostTransformMatrix>(bunkerHealth.ValueRW.healthBar);

                // Update the x component of the scale
                bunkerHealthBarTransformMatrix.ValueRW.Value =
                    float4x4.Scale(normalisedHealth, 1.0f, 1.0f);
            }
        }
    }
}