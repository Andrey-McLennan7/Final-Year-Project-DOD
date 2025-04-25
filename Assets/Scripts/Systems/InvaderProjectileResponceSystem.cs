using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
partial struct InvaderProjectileResponceSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<LocalTransform>();
        state.RequireForUpdate<BoxCollider>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> invaderLocalTransform, RefRO<BoxCollider> invaderBoxCollider, RefRO<Invader> invader, Entity invaderEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>, RefRO<Invader>>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<Laser> laser, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Laser>, RefRO<BoxCollider>>())
            {
                if (!BoxCollisionResponceSystem.OnCollisionResponce(invaderLocalTransform, invaderBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                entityCommandBuffer.DestroyEntity(invaderEntity);
            }
        }
    }
}