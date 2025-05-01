using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(MoveMysteryShipSystem))]
partial struct DestroyOffSreenMysteryShipSystem : ISystem
{
    // Reference Entity once
    Entity mysteryShipSpawnerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is null or no longer exists
        if (mysteryShipSpawnerEntity == Entity.Null || !state.EntityManager.Exists(mysteryShipSpawnerEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<MysteryShipSpawner>())
            {
                return;
            }

            // Get reference to the singleton entity
            mysteryShipSpawnerEntity = SystemAPI.GetSingletonEntity<MysteryShipSpawner>();
        }

        // Get the references to the necessary components of the Entity
        RefRW<MysteryShipSpawner> mysteryShipSpawner = SystemAPI.GetComponentRW<MysteryShipSpawner>(mysteryShipSpawnerEntity);

        // Skip the code if no instance of a mystery ship entity is found
        if (mysteryShipSpawner.ValueRO.mysteryShipEntity == Entity.Null ||
            !state.EntityManager.Exists(mysteryShipSpawner.ValueRO.mysteryShipEntity))
        {
            return;
        }

        // Get the reference to the mystery ship Entity
        Entity mysteryShipEntity = mysteryShipSpawner.ValueRO.mysteryShipEntity;

        // Get the references to the necessary components of the Entity
        RefRO<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(mysteryShipEntity);

        // Skip the code as long as the Mystery Ship Entity is within the boundaries of the scene/border
        if (mysteryShipLocalTransform.ValueRO.Position.x > -17.0f && mysteryShipLocalTransform.ValueRO.Position.x < 17.0f)
        {
            return;
        }

        mysteryShipSpawner.ValueRW.activeMysteryShip = false;

        // Destroy mystery ship
        state.EntityManager.DestroyEntity(mysteryShipEntity);
    }
}