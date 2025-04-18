using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

partial struct PlayerShootSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();

        RefRW<PlayerShoot> playerShoot = SystemAPI.GetComponentRW<PlayerShoot>(playerEntity);
        RefRO<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!playerShoot.ValueRO.activeLaser)
            {
                Entity laserEntity = state.EntityManager.Instantiate(playerShoot.ValueRO.laserPrefab);

                RefRW<Projectile> laserProjectile = SystemAPI.GetComponentRW<Projectile>(laserEntity);
                RefRW<LocalTransform> laserTransform = SystemAPI.GetComponentRW<LocalTransform>(laserEntity);

                laserProjectile.ValueRW.playerEntity = playerEntity;

                laserTransform.ValueRW.Position = playerLocalTransform.ValueRO.Position;
                laserTransform.ValueRW.Rotation = Quaternion.identity;

                playerShoot.ValueRW.activeLaser = true;
            }
        }
    }
}