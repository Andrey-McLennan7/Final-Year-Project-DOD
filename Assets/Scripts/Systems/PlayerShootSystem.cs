using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(ProjectileMovementSystem))]
partial struct PlayerShootSystem : ISystem
{
    Entity playerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (playerEntity == Entity.Null || !state.EntityManager.Exists(playerEntity))
        {
            if (!SystemAPI.HasSingleton<Player>())
            {
                return;
            }

            playerEntity = SystemAPI.GetSingletonEntity<Player>();
        }

        RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(playerEntity);
        RefRO<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Space) || UnityEngine.Input.GetMouseButtonDown(0))
        {
            if (playerShoot.ValueRO.activeLaser)
            {
                return;
            }

            Entity laserEntity = state.EntityManager.Instantiate(playerShoot.ValueRO.laserPrefab);

            SystemAPI.SetComponent(laserEntity, LocalTransform.FromPosition(playerLocalTransform.ValueRO.Position));

            RefRW<Projectile> laserProjectile = SystemAPI.GetComponentRW<Projectile>(laserEntity);

            laserProjectile.ValueRW.entityThatShot = playerEntity;

            playerShoot.ValueRW.activeLaser = true;
        }
    }
}