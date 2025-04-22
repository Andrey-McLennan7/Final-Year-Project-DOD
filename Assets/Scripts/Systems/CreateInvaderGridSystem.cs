using Unity.Entities;
using Unity.Burst;

[BurstCompile]
partial struct CreateInvaderGridSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndInitializationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<ExecuteOnceTag>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        Entity invaderGridEntity = SystemAPI.GetSingletonEntity<InvaderGrid>();

        RefRO<InvaderGrid> invaderGrid = SystemAPI.GetComponentRO<InvaderGrid>(invaderGridEntity);

        for (int i = 0; i < invaderGrid.ValueRO.rows; ++i)
        {
            
        }
    }
}