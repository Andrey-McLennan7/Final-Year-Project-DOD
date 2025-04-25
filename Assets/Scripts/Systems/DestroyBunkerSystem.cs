using Unity.Entities;
using Unity.Burst;

[BurstCompile]
[UpdateAfter(typeof(BunkerProjectileCollisionResponseSystem))]
partial struct DestroyBunkerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<BunkerHealth> bunker, Entity bunkerEntity) in SystemAPI.Query<RefRO<BunkerHealth>>().WithEntityAccess())
        {
            if (bunker.ValueRO.health > 0)
            {
                continue;
            }

            entityCommandBuffer.DestroyEntity(bunkerEntity);
        }
    }
}