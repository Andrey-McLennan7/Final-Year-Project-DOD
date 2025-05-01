using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

[BurstCompile]
[UpdateBefore(typeof(ProjectileMovementSystem))]
partial struct PlayerShootSystem : ISystem
{
    // Reference Entity once
    Entity playerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is Null or the Entity no longer 
        if (playerEntity == Entity.Null || !state.EntityManager.Exists(playerEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<Player>())
            {
                return;
            }

            // Get reference to the singleton entity
            playerEntity = SystemAPI.GetSingletonEntity<Player>();
        }

        // Get the refernces of the necessary components of the Entity
        RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(playerEntity);
        RefRO<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        // Shoot
        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space) || UnityEngine.Input.GetMouseButtonDown(0))
        {
            // Only shoot one laser at a time
            if (playerShoot.ValueRO.activeLaser)
            {
                return;
            }

            Entity laserEntity = state.EntityManager.Instantiate(playerShoot.ValueRO.laserPrefab);

            state.EntityManager.SetComponentData(laserEntity, new LocalTransform
            {
                Position = playerLocalTransform.ValueRO.Position,
                Rotation = quaternion.identity,
                Scale = 1.0f,
            });

            RefRW<Projectile> laserProjectile = SystemAPI.GetComponentRW<Projectile>(laserEntity);

            laserProjectile.ValueRW.entityThatShot = playerEntity;

            playerShoot.ValueRW.activeLaser = true;
        }
    }
}