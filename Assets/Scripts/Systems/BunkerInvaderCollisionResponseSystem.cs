using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
partial struct BunkerInvaderCollisionResponseSystem : ISystem
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
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get the reference to the Entity Command Buffer
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> bunkerLocalTransform, RefRO<BoxCollider> bunkerBoxCollider, Entity bunkerEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Bunker>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> invaderLocalTransform, RefRO<BoxCollider> invaderBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Invader>())
            {
                // Skip the code if no collision is detected
                if (!BoxCollisionResponseSystem.OnCollisionResponse(bunkerLocalTransform, bunkerBoxCollider,
                    invaderLocalTransform, invaderBoxCollider))
                {
                    continue;
                }

                // Queue the Entity to be destroyed
                entityCommandBuffer.DestroyEntity(bunkerEntity);

                break;
            }
        }
    }
}