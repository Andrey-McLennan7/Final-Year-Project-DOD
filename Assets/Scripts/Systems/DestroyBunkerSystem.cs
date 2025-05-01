using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateAfter(typeof(BunkerProjectileCollisionResponseSystem))]
partial struct DestroyBunkerSystem : ISystem
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
        // Get reference to the Entity Command Buffer
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<BunkerHealth> bunkerHealth, Entity bunkerEntity) in SystemAPI.Query<RefRO<BunkerHealth>>().WithEntityAccess())
        {
            // Skip the code as long as the bunker health is
            // above zero
            if (bunkerHealth.ValueRO.health > 0)
            {
                continue;
            }

            // Queue the Entity to be destroyed
            entityCommandBuffer.DestroyEntity(bunkerEntity);
        }
    }
}