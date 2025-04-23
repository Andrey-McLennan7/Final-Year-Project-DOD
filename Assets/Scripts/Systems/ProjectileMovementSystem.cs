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

        foreach ((RefRW<LocalTransform> localTransform, RefRO<Projectile> projectile, RefRO<Movement> movement, Entity projectileEntity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Projectile>, RefRO<Movement>>().WithEntityAccess())
        {
            localTransform.ValueRW.Position += movement.ValueRO.movementDirection * movement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;

            // ----------------------- Temporary start -----------------------
            if (localTransform.ValueRO.Position.y < 16.0f &&
                localTransform.ValueRO.Position.y > -16.0f )
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
            // ----------------------- Temporary end -----------------------
        }
    }
}