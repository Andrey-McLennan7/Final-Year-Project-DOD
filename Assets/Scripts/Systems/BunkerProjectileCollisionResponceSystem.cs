using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
partial struct BunkerProjectileCollisionResponceSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> bunkerLocalTransform, RefRO <LocalToWorld> bunkerLocalToWorld, RefRW<BunkerHealth> bunkerHealth, RefRO<BoxCollider> bunkerBoxCollider, Entity bunkerEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<LocalToWorld>, RefRW<BunkerHealth>, RefRO<BoxCollider>>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<Projectile> projectile, RefRO<Laser> laser, RefRO<BoxCollider> projectileBoxCollider, Entity projectileEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Projectile>, RefRO<Laser>, RefRO<BoxCollider>>().WithEntityAccess())
            {
                if (!BoxCollisionResponceSystem.OnCollisionResponce(bunkerLocalTransform, bunkerBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                --bunkerHealth.ValueRW.health;

                RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(projectile.ValueRO.entityThatShot);

                playerShoot.ValueRW.activeLaser = false;

                entityCommandBuffer.DestroyEntity(projectileEntity);

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