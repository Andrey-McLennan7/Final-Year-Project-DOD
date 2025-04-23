using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(PlayerShootSystem))]
partial struct ProjectileMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRW<LocalTransform> localTransform, RefRO<Projectile> projectile, RefRO<Movement> movement, Entity projectileEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Projectile>, RefRO<Movement>>().WithEntityAccess())
        {
            localTransform.ValueRW.Position += movement.ValueRO.movementDirection * movement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;

            // Temporary start
            if (localTransform.ValueRO.Position.y < 16.0f &&
                localTransform.ValueRO.Position.y > -16.0f )
            {
                continue;
            }

            if (projectile.ValueRO.playerEntity != Entity.Null)
            {
                RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(projectile.ValueRO.playerEntity);

                playerShoot.ValueRW.activeLaser = false;
            }

            entityCommandBuffer.DestroyEntity(projectileEntity);
            // Temporary end
        }
    }
}