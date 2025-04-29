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
                if (!BoxCollisionResponseSystem.OnCollisionResponce(bunkerLocalTransform, bunkerBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                --bunkerHealth.ValueRW.health;

                if (!SystemAPI.HasComponent<PostTransformMatrix>(bunkerHealth.ValueRW.healthBar))
                {
                    continue;
                }

                float normalisedHealth = (float)bunkerHealth.ValueRO.health / (float)bunkerHealth.ValueRO.maxHealth;

                RefRW<PostTransformMatrix> bunkerHealthBarTransformMatrix = SystemAPI.GetComponentRW<PostTransformMatrix>(bunkerHealth.ValueRW.healthBar);

                bunkerHealthBarTransformMatrix.ValueRW.Value =
                    float4x4.Scale(normalisedHealth * bunkerLocalToWorld.ValueRO.Value.c0.x, bunkerLocalToWorld.ValueRO.Value.c1.y, 1.0f);
            }
        }
    }
}