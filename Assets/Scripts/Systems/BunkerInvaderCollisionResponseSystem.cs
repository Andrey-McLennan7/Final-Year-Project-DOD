using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
partial struct BunkerInvaderCollisionResponseSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> bunkerLocalTransform, RefRO<BoxCollider> bunkerBoxCollider, Entity bunkerEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Bunker>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> invaderLocalTransform, RefRO<BoxCollider> invaderBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Invader>())
            {
                if (!BoxCollisionResponseSystem.OnCollisionResponse(bunkerLocalTransform, bunkerBoxCollider,
                    invaderLocalTransform, invaderBoxCollider))
                {
                    continue;
                }

                entityCommandBuffer.DestroyEntity(bunkerEntity);

                break;
            }
        }
    }
}