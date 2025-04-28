using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(MoveMysteryShipSystem))]
partial struct DestroyOffSreenMysteryShipSystem : ISystem
{
    // Reference point to the mystery ship spawner
    Entity mysteryShipSpawnerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
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

        // Get reference to the mystery ship
        Entity mysteryShipEntity = mysteryShipSpawner.ValueRO.mysteryShipEntity;

        // Get necessary mystery ship components
        RefRO<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(mysteryShipEntity);

        if (mysteryShipLocalTransform.ValueRO.Position.x > -17.0f && mysteryShipLocalTransform.ValueRO.Position.x < 17.0f)
        {
            return;
        }

        mysteryShipSpawner.ValueRW.activeMysteryShip = false;

        // Destroy mystery ship
        state.EntityManager.DestroyEntity(mysteryShipEntity);
    }
}