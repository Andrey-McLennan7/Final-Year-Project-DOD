using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
partial struct DestroyProjectileSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        /// <summary>
        /// 
        /// The Entity Command Buffer is best suited for destroying looped entity,
        /// as it does not destroy them until the end of an interator is reached
        /// 
        /// </summary>

        // Make sure that the Entity Command Buffer exists before running this system
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();

        // Make sure that Entities with the following components exist before running this system
        state.RequireForUpdate<LocalTransform>();
        state.RequireForUpdate<BoxCollider>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get reference to the Entity Command Buffer
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider, RefRO<Projectile> projectile, RefRO<Movement> movement, Entity projectileEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>, RefRO<Projectile>, RefRO<Movement>>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> otherLocalTransform, RefRO<BoxCollider> otherBoxCollider, Entity otherEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithEntityAccess())
            {
                // Make sure that the projectile does not collide with itself
                if (projectileEntity == otherEntity)
                {
                    continue;
                }

                // Make sure that the laser projectile does not collide with the player
                // and also the missile projectile does not collide with the invader
                if (SystemAPI.HasComponent<Laser>(projectileEntity)   && SystemAPI.HasComponent<Player>(otherEntity) ||
                    SystemAPI.HasComponent<Missile>(projectileEntity) && SystemAPI.HasComponent<Invader>(otherEntity) )
                {
                    continue;
                }

                // Skip the code if no collision is detected
                if (!BoxCollisionResponseSystem.OnCollisionResponse(projectileLocalTransform, projectileBoxCollider,
                    otherLocalTransform, otherBoxCollider))
                {
                    continue;
                }

                // Check for which Entity shot the projectile
                if (projectile.ValueRO.entityThatShot != Entity.Null &&
                    state.EntityManager.Exists(projectile.ValueRO.entityThatShot))
                {
                    if (SystemAPI.HasComponent<Laser>(projectileEntity))
                    {
                        // Player
                        RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(projectile.ValueRO.entityThatShot);
                        playerShoot.ValueRW.activeLaser = false;
                    }
                    else
                    {
                        // Invader
                        RefRW<InvaderShoot> invaderShoot = SystemAPI.GetComponentRW<InvaderShoot>(projectile.ValueRO.entityThatShot);
                        invaderShoot.ValueRW.activeMissile = false;
                    }
                }

                // Destroy the Entity the projectile collided with if it is a missile
                if (SystemAPI.HasComponent<Missile>(otherEntity))
                {
                    entityCommandBuffer.DestroyEntity(otherEntity);
                }

                // Queue the Entity to be destroyed
                entityCommandBuffer.DestroyEntity(projectileEntity);
            }
        }
    }
}