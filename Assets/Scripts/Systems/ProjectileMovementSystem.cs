using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

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

            if (localTransform.ValueRW.Position.y >= 16.0f) // Temporary
            {
                RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(projectile.ValueRO.playerEntity);

                playerShoot.ValueRW.activeLaser = false;

                entityCommandBuffer.DestroyEntity(projectileEntity);
            }
        }
    }
}