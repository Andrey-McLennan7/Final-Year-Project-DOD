using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateBefore(typeof(ProjectileMovementSystem))]
partial struct InvaderShootSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalTransform> invaderLocalTransform, RefRW<InvaderShoot> invaderShoot, Entity invaderEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRW<InvaderShoot>>().WithEntityAccess())
        {
            // Check if the Entity reference is Null or no longer exists
            if (invaderShoot.ValueRO.missilePrefab == Entity.Null ||
                !state.EntityManager.Exists(invaderShoot.ValueRO.missilePrefab))
            {
                continue;
            }

            // Check if the Entity reference is Null or no longer exists
            if (invaderShoot.ValueRO.invaderGridEntity == Entity.Null ||
                !state.EntityManager.Exists(invaderShoot.ValueRO.invaderGridEntity))
            {
                continue;
            }

            // Only shoot one missile at a time
            if (invaderShoot.ValueRO.activeMissile)
            {
                continue;
            }

            // Get the references of the necessary components of the Entity
            RefRO<InvaderGridState> invaderGridState = SystemAPI.GetComponentRO<InvaderGridState>(invaderShoot.ValueRO.invaderGridEntity);

            // Shoot missile randomly if the random
            // value is the max value
            if (UnityEngine.Random.Range(0, invaderShoot.ValueRO.shootInfrequency + 1) == invaderShoot.ValueRO.shootInfrequency)
            {
                // Increase the chance of shooting based on the
                // amount of Invaders that are still alive
                if (UnityEngine.Random.value < (1.0f / invaderGridState.ValueRO.amountAlive))
                {
                    // Set the initial position of the projectile Entity
                    state.EntityManager.SetComponentData(invaderShoot.ValueRO.missilePrefab, new LocalTransform
                    {
                        Position = invaderLocalTransform.ValueRO.Position,
                        Rotation = quaternion.identity,
                        Scale = 1.0f,
                    });

                    // Instantiate the projectile Entity
                    Entity missileEntity = state.EntityManager.Instantiate(invaderShoot.ValueRO.missilePrefab);

                    // Get the references of the necessary components of the Entity
                    RefRW<Projectile> missileProjectile = SystemAPI.GetComponentRW<Projectile>(missileEntity);

                    missileProjectile.ValueRW.entityThatShot = invaderEntity;

                    invaderShoot.ValueRW.activeMissile = true;
                }
            }
        }
    }
}