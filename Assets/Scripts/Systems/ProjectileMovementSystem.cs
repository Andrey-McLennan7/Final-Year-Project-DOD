using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
partial struct ProjectileMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRW<LocalTransform> localTransform, RefRO<Projectile> projectile, Entity projectileEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Projectile>>().WithEntityAccess())
        {
            localTransform.ValueRW.Position += projectile.ValueRO.movementDirection * projectile.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;

            // Temporary start
            if (localTransform.ValueRO.Position.y < 16.0f &&
                localTransform.ValueRO.Position.y > -16.0f  )
            {
                continue;
            }

            if (state.EntityManager.Exists(projectile.ValueRO.playerEntity))
            {
                RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(projectile.ValueRO.playerEntity);

                playerShoot.ValueRW.activeLaser = false;
            }

            entityCommandBuffer.DestroyEntity(projectileEntity);
            // Temporary end
        }
    }
}