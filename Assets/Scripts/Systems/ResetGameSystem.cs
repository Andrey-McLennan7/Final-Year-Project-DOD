using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateAfter(typeof(CreateInvaderGridSystem))]
partial struct ResetGameSystem : ISystem
{
    // Reference Entities once
    Entity resetGameEntity;
    Entity invaderGridEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is Null or the Entity no longer 
        if (resetGameEntity == Entity.Null || !state.EntityManager.Exists(resetGameEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<ResetGame>())
            {
                return;
            }

            // Get reference to the singleton entity
            resetGameEntity = SystemAPI.GetSingletonEntity<ResetGame>();
        }

        // Check if the entity reference is Null or the Entity no longer 
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

        // Get the refernces of the necessary components of the Entity
        RefRO<InvaderGridState> invaderGridState = SystemAPI.GetComponentRO<InvaderGridState>(invaderGridEntity);

        // Skip the code if the amount killed does not match the total amount
        if (invaderGridState.ValueRO.amountKilled != invaderGridState.ValueRO.totalAmount)
        {
            return;
        }

        // Reset the game
        SystemAPI.SetComponent(resetGameEntity, new ResetGame
        {
            reset = true,
        });
    }
}