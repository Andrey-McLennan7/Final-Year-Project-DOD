using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(SpawnMysteryShipSystem))]
partial struct MoveMysteryShipSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get reference to the mystery ship spawner
        Entity mysteryShipSpawnerEntity = SystemAPI.GetSingletonEntity<MysteryShipSpawner>();

        // Get necessary mystery ship spawner components
        RefRO<MysteryShipSpawner> mysteryShipSpawner = SystemAPI.GetComponentRO<MysteryShipSpawner>(mysteryShipSpawnerEntity);

        // Skip code if no instance of a mystery ship entity is found
        if (!state.EntityManager.Exists(mysteryShipSpawner.ValueRO.mysteryShipEntity))
        {
            return;
        }

        // Get reference to the mystery ship
        Entity mysteryShipEntity = mysteryShipSpawner.ValueRO.mysteryShipEntity;

        // Get necessary mystery ship components
        RefRO<Movement> mysteryShipMover = SystemAPI.GetComponentRO<Movement>(mysteryShipEntity);
        RefRW<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(mysteryShipEntity);

        // Change position of the mystery ship
        mysteryShipLocalTransform.ValueRW.Position
            += mysteryShipMover.ValueRO.movementDirection * mysteryShipMover.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
    }
}