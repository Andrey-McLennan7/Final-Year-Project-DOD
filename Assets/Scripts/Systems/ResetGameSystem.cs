using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateAfter(typeof(CreateInvaderGridSystem))]
partial struct ResetGameSystem : ISystem
{
    Entity resetGameEntity;
    Entity invaderGridEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (resetGameEntity == Entity.Null || !state.EntityManager.Exists(resetGameEntity))
        {
            if (!SystemAPI.HasSingleton<ResetGame>())
            {
                return;
            }

            resetGameEntity = SystemAPI.GetSingletonEntity<ResetGame>();
        }

        if (invaderGridEntity == Entity.Null || !state.EntityManager.Exists(invaderGridEntity))
        {
            if (!SystemAPI.HasSingleton<InvaderGrid>())
            {
                return;
            }

            invaderGridEntity = SystemAPI.GetSingletonEntity<InvaderGrid>();
        }

        RefRO<InvaderGridState> invaderGridState = SystemAPI.GetComponentRO<InvaderGridState>(invaderGridEntity);

        if (invaderGridState.ValueRO.amountKilled != invaderGridState.ValueRO.totalAmount)
        {
            return;
        }

        SystemAPI.SetComponent(resetGameEntity, new ResetGame
        {
            reset = true,
        });
    }
}
