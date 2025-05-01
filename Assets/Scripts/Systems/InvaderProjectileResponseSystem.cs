using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct InvaderProjectileResponseSystem : ISystem
{
    // Reference Entity once
    Entity invaderGridEntity;

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
        // Check if the entity reference is null or no longer exists
        if (invaderGridEntity == Entity.Null || !state.EntityManager.Exists(invaderGridEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<InvaderGrid>())
            {
                return;
            }

            // Get reference to the singleton entity
            invaderGridEntity = SystemAPI.GetSingletonEntity<InvaderGrid>();
        }

        // Get reference to the Entity Command Buffer
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> invaderLocalTransform, RefRO<BoxCollider> invaderBoxCollider, Entity invaderEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Invader>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Laser>())
            {
                // Skip the code if no collision is detected
                if (!BoxCollisionResponseSystem.OnCollisionResponse(invaderLocalTransform, invaderBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                // Get the references to the necessary components of the Entity
                RefRW<InvaderGridState> invaderGridState = SystemAPI.GetComponentRW<InvaderGridState>(invaderGridEntity);

                // Update the amount killed
                ++invaderGridState.ValueRW.amountKilled;

                // Update the amount alive
                invaderGridState.ValueRW.amountAlive =
                    invaderGridState.ValueRO.totalAmount - invaderGridState.ValueRO.amountKilled;

                // Update the percentage killed
                invaderGridState.ValueRW.percentKilled =
                    (float)invaderGridState.ValueRO.amountKilled / (float)invaderGridState.ValueRO.totalAmount;

                // Queue the Entity to be destroyed
                entityCommandBuffer.DestroyEntity(invaderEntity);

                // Update the speed of every invader Entity in the scene
                IncreaseInvaderMovementSpeedJob speedJob = new IncreaseInvaderMovementSpeedJob()
                {
                    percentKilled = invaderGridState.ValueRO.percentKilled,
                    maxSpeed = invaderGridState.ValueRO.maxSpeed,
                };

                // Schedule the multi-core process of the invader movement
                JobHandle speedHandle = speedJob.ScheduleParallel(state.Dependency);

                // Make sure that the task is complete
                state.Dependency = speedHandle;
                speedHandle.Complete();
            }
        }
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
public partial struct IncreaseInvaderMovementSpeedJob : IJobEntity
{
    public float percentKilled;
    public float maxSpeed;

    public void Execute(ref Movement movement, in Invader invader)
    {
        // Update the movement speed of the invader Entity
        movement.movementSpeed = percentKilled * maxSpeed;
    }
}