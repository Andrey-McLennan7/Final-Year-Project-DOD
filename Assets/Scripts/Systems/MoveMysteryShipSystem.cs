using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(SpawnMysteryShipSystem))]
partial struct MoveMysteryShipSystem : ISystem
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

        // Get the refernces of the necessary components of the Entity
        RefRO<MysteryShipSpawner> mysteryShipSpawner = SystemAPI.GetComponentRO<MysteryShipSpawner>(mysteryShipSpawnerEntity);

        // Skip the code the reference to the Entity is Null or the Entity no longer exists
        if (mysteryShipSpawner.ValueRO.mysteryShipEntity == Entity.Null ||
            !state.EntityManager.Exists(mysteryShipSpawner.ValueRO.mysteryShipEntity))
        {
            return;
        }

        // Get reference to the Entity
        Entity mysteryShipEntity = mysteryShipSpawner.ValueRO.mysteryShipEntity;

        // Get the references of the necessary components of the Entity
        RefRO<Movement> mysteryShipMover = SystemAPI.GetComponentRO<Movement>(mysteryShipEntity);
        RefRW<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(mysteryShipEntity);

        // Change the position of the mystery ship
        mysteryShipLocalTransform.ValueRW.Position
            += mysteryShipMover.ValueRO.movementDirection * mysteryShipMover.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
    }
}