using UnityEngine;
using Unity.Entities;
using Unity.Burst;

partial struct DestroyBunkerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<Bunker> bunker, Entity bunkerEntity) in SystemAPI.Query<RefRO<Bunker>>().WithEntityAccess())
        {
            if (bunker.ValueRO.health > 0)
            {
                return;
            }

            entityCommandBuffer.DestroyEntity(bunkerEntity);
        }
    }
}