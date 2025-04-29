using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct InvaderProjectileResponseSystem : ISystem
{
    Entity invaderGridStateEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<LocalTransform>();
        state.RequireForUpdate<BoxCollider>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (invaderGridStateEntity == Entity.Null || !state.EntityManager.Exists(invaderGridStateEntity))
        {
            if (!SystemAPI.HasSingleton<InvaderGridState>())
            {
                return;
            }

            invaderGridStateEntity = SystemAPI.GetSingletonEntity<InvaderGridState>();
        }

        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> invaderLocalTransform, RefRO<BoxCollider> invaderBoxCollider, Entity invaderEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Invader>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Laser>())
            {
                if (!BoxCollisionResponseSystem.OnCollisionResponce(invaderLocalTransform, invaderBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                RefRW<InvaderGridState> invaderGridState = SystemAPI.GetComponentRW<InvaderGridState>(invaderGridStateEntity);

                ++invaderGridState.ValueRW.amountKilled;

                invaderGridState.ValueRW.amountAlive =
                    invaderGridState.ValueRO.totalAmount - invaderGridState.ValueRO.amountKilled;

                invaderGridState.ValueRW.percentKilled =
                    (float)invaderGridState.ValueRO.amountKilled / (float)invaderGridState.ValueRO.totalAmount;

                entityCommandBuffer.DestroyEntity(invaderEntity);

                IncreaseInvaderMovementSpeedJob speedJob = new IncreaseInvaderMovementSpeedJob()
                {
                    percentKilled = invaderGridState.ValueRO.percentKilled,
                    maxSpeed = invaderGridState.ValueRO.maxSpeed,
                };

                JobHandle speedHandle = speedJob.ScheduleParallel(state.Dependency);
                state.Dependency = speedHandle;
                speedHandle.Complete();
            }
        }
    }
}

[BurstCompile]
public partial struct IncreaseInvaderMovementSpeedJob : IJobEntity
{
    public float percentKilled;
    public float maxSpeed;

    public void Execute(ref Movement movement, in Invader invader)
    {
        movement.movementSpeed = percentKilled * maxSpeed;
    }
}