using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

[BurstCompile]
[UpdateAfter(typeof(CreateInvaderGridSystem))]
partial struct InvaderMovementSystem : ISystem
{
    private bool advanceRow;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        advanceRow = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        InvaderMovementJob movementJob = new InvaderMovementJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };

        JobHandle movementHandle = movementJob.ScheduleParallel(state.Dependency);

        state.Dependency = movementHandle;
        movementHandle.Complete();

        foreach ((RefRO<LocalTransform> localTransform, RefRO<Movement> movement) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Movement>>().WithPresent<Invader>())
        {
            if (movement.ValueRO.movementDirection.x > 0.0f && localTransform.ValueRO.Position.x >=  (14.0f) ||
                movement.ValueRO.movementDirection.x < 0.0f && localTransform.ValueRO.Position.x <= (-14.0f)  )
            {
                advanceRow = true;
                break;
            }
        }

        if (advanceRow)
        {
            AdvanceRowJob advanceRowJob = new AdvanceRowJob();
            JobHandle advanceRowHandle = advanceRowJob.ScheduleParallel(state.Dependency);

            state.Dependency = advanceRowHandle;

            advanceRow = false;
        }
    }
}

[BurstCompile]
public partial struct InvaderMovementJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, in Movement movement, in Invader invader)
    {
        localTransform.Position += movement.movementDirection * movement.movementSpeed * deltaTime;
    }
}

[BurstCompile]
public partial struct AdvanceRowJob : IJobEntity
{
    public void Execute(ref LocalTransform localTransform, ref Movement movement, in Invader invader)
    {
        movement.movementDirection.x *= -1.0f;

        float3 position = localTransform.Position;
        position.y -= 1.0f;
        localTransform.Position = position;
    }
}