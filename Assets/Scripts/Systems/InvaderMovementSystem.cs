using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Jobs;

[BurstCompile]
[UpdateAfter(typeof(CreateInvaderGridSystem))]
partial struct InvaderMovementSystem : ISystem
{
    // Keep track on when to advance the grid's y position
    private bool advanceRow;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        advanceRow = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Update the movement of every invader Entity in the scene
        InvaderMovementJob movementJob = new InvaderMovementJob()
        {
            deltaTime = SystemAPI.Time.DeltaTime,
        };

        // Schedule the multi-core process of the invader movement
        JobHandle movementHandle = movementJob.ScheduleParallel(state.Dependency);

        // Make sure that the task is complete before moving on to
        // the next set of tasks/multi-processing
        state.Dependency = movementHandle;
        movementHandle.Complete();

        foreach ((RefRO<LocalTransform> localTransform, RefRO<Movement> movement) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Movement>>().WithPresent<Invader>())
        {
            // Advance grid to the next x and y position when one of them collides
            // with the edge of the border/screen
            if (movement.ValueRO.movementDirection.x > 0.0f && localTransform.ValueRO.Position.x >=  (14.0f) ||
                movement.ValueRO.movementDirection.x < 0.0f && localTransform.ValueRO.Position.x <= (-14.0f)  )
            {
                advanceRow = true;
                break;
            }
        }

        // Skip the code if non of the invader Entities have reached
        // the edge of the border/screen
        if (!advanceRow)
        {
            return;
        }

        // Update the y position of every invader Entity in the scene
        AdvanceRowJob advanceRowJob = new AdvanceRowJob();

        // Schedule the multi-core process of the invader advancement
        JobHandle advanceRowHandle = advanceRowJob.ScheduleParallel(state.Dependency);

        // Make sure that the task is complete
        state.Dependency = advanceRowHandle;
        advanceRowHandle.Complete();

        // Reset advancement
        advanceRow = false;
    }
}

/// <summary>
/// 
/// The Unity Jobs System is a DOTS package that allows developers to make use of the multiple cores
/// in modern CPUs for multi-core processing and improved performance, especially in data intensive
/// operations
/// 
/// </summary>

[BurstCompile]
public partial struct InvaderMovementJob : IJobEntity
{
    public float deltaTime;

    public void Execute(ref LocalTransform localTransform, in Movement movement, in Invader invader)
    {
        // Update the position based on the direction of movement
        localTransform.Position += movement.movementDirection * movement.movementSpeed * deltaTime;
    }
}

[BurstCompile]
public partial struct AdvanceRowJob : IJobEntity
{
    public void Execute(ref LocalTransform localTransform, ref Movement movement, in Invader invader)
    {
        // Next direction of movement
        movement.movementDirection.x *= -1.0f;

        // Update the y position of each invader in the grid
        float3 position = localTransform.Position;
        position.y -= 1.0f;
        localTransform.Position = position;
    }
}