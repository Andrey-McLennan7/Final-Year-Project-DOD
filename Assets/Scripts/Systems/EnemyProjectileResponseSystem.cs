using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct EnemyProjectileResponseSystem : ISystem
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

        foreach ((RefRO<LocalTransform> enemyLocalTransform, RefRO<BoxCollider> enemyBoxCollider, RefRO<Enemy> enemy, Entity enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>, RefRO<Enemy>>().WithEntityAccess())
        {
            foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<Laser> laser, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<Laser>, RefRO<BoxCollider>>())
            {
                if (!BoxCollisionResponseSystem.OnCollisionResponce(enemyLocalTransform, enemyBoxCollider,
                    projectileLocalTransform, projectileBoxCollider))
                {
                    continue;
                }

                entityCommandBuffer.DestroyEntity(enemyEntity);
            }
        }
    }
}