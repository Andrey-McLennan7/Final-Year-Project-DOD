using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
partial struct DestroyProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<LocalTransform>();
        state.RequireForUpdate<BoxCollider>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider, RefRO<Projectile> projectile, RefRO<Movement> movement, Entity projectileEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>, RefRO<Projectile>, RefRO<Movement>>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> otherLocalTransform, RefRO<BoxCollider> otherBoxCollider, Entity otherEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithEntityAccess())
            {
                if (projectileEntity == otherEntity)
                {
                    continue;
                }

                if (!BoxCollisionResponceSystem.OnCollisionResponce(projectileLocalTransform, projectileBoxCollider,
                    otherLocalTransform, otherBoxCollider))
                {
                    continue;
                }

                if (projectile.ValueRO.entityThatShot != Entity.Null ||
                    !state.EntityManager.Exists(projectile.ValueRO.entityThatShot))
                {
                    if (SystemAPI.HasComponent<Laser>(projectileEntity))
                    {
                        RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(projectile.ValueRO.entityThatShot);

                        playerShoot.ValueRW.activeLaser = false;
                    }
                    else
                    {
                        RefRW<InvaderShoot> invaderShoot = SystemAPI.GetComponentRW<InvaderShoot>(projectile.ValueRO.entityThatShot);

                        invaderShoot.ValueRW.activeMissile = false;
                    }
                }

                entityCommandBuffer.DestroyEntity(projectileEntity);
            }
        }
    }
}