using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
[UpdateBefore(typeof(DestroyOffSreenMysteryShipSystem))]
partial struct MysteryShipCollisionResponseSystem : ISystem
{
    // Reference point to the mystery ship spawner
    Entity mysteryShipSpawnerEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get existing entity command buffer
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        if (mysteryShipSpawnerEntity == Entity.Null || !state.EntityManager.Exists(mysteryShipSpawnerEntity))
        {
            if (!SystemAPI.HasSingleton<MysteryShipSpawner>())
            {
                return;
            }

            // Get reference to the mystery ship spawner
            mysteryShipSpawnerEntity = SystemAPI.GetSingletonEntity<MysteryShipSpawner>();
        }

        // Get necessary mystery ship spawner components
        RefRW<MysteryShipSpawner> mysteryShipSpawner = SystemAPI.GetComponentRW<MysteryShipSpawner>(mysteryShipSpawnerEntity);

        // Skip code if no instance of a mystery ship entity is found
        if (mysteryShipSpawner.ValueRO.mysteryShipEntity == Entity.Null ||
            !state.EntityManager.Exists(mysteryShipSpawner.ValueRO.mysteryShipEntity))
        {
            return;
        }

        Entity mysteryShipEntity = mysteryShipSpawner.ValueRO.mysteryShipEntity;

        if (!SystemAPI.HasComponent<BoxCollider>(mysteryShipEntity))
        {
            return;
        }

        RefRO<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(mysteryShipEntity);
        RefRO<BoxCollider> mysteryShipBoxCollider = SystemAPI.GetComponentRO<BoxCollider>(mysteryShipEntity);

        foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider, RefRO<Laser> laser) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>, RefRO<Laser>>())
        {
            if (!BoxCollisionResponseSystem.OnCollisionResponse(mysteryShipLocalTransform, mysteryShipBoxCollider,
                projectileLocalTransform, projectileBoxCollider))
            {
                continue;
            }

            mysteryShipSpawner.ValueRW.activeMysteryShip = false;

            entityCommandBuffer.DestroyEntity(mysteryShipEntity);

            break;
        }
    }
}