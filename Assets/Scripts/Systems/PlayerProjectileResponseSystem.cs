using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct PlayerProjectileResponseSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        state.RequireForUpdate<LocalTransform>();
        state.RequireForUpdate<BoxCollider>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach ((RefRO<LocalTransform> playerLocalTransform, RefRO<BoxCollider> playerBoxCollider, RefRO<Player> player) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>, RefRO<Player>>())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<Missile> missile, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Missile>, RefRO<BoxCollider>>())
            {
                if (!BoxCollisionResponseSystem.OnCollisionResponce(playerLocalTransform, playerBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                UnityEngine.Debug.Log("YOU ARE DEAD NO BIG SUPRISE");
            }
        }
    }
}