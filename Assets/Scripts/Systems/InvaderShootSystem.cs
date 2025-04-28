using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(PlayerShootSystem))]
[UpdateBefore(typeof(ProjectileMovementSystem))]
partial struct InvaderShootSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalTransform> localTransform, RefRW<InvaderShoot> invaderShoot, Entity invaderEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<InvaderShoot>>().WithEntityAccess())
        {
            if (invaderShoot.ValueRO.missilePrefab == Entity.Null ||
                !state.EntityManager.Exists(invaderShoot.ValueRO.missilePrefab))
            {
                continue;
            }

            if (invaderShoot.ValueRO.invaderGridEntity == Entity.Null ||
                !state.EntityManager.Exists(invaderShoot.ValueRO.invaderGridEntity))
            {
                continue;
            }

            if (invaderShoot.ValueRO.activeMissile)
            {
                continue;
            }

            RefRO<InvaderGridState> invaderGridState = SystemAPI.GetComponentRO<InvaderGridState>(invaderShoot.ValueRO.invaderGridEntity);

            if (UnityEngine.Random.Range(0, 501) == 500)
            {
                if (UnityEngine.Random.value < (1.0f / invaderGridState.ValueRO.amountAlive))
                {
                    Entity missileEntity = state.EntityManager.Instantiate(invaderShoot.ValueRO.missilePrefab);

                    SystemAPI.SetComponent(missileEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));

                    RefRW<Projectile> missileProjectile = SystemAPI.GetComponentRW<Projectile>(missileEntity);

                    missileProjectile.ValueRW.entityThatShot = invaderEntity;

                    invaderShoot.ValueRW.activeMissile = true;
                }
            }
        }
    }
}