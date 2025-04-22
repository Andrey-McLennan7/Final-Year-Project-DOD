using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
partial struct SpawnMysteryShipSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get reference to the mystery ship spawner
        Entity mysteryShipSpawnerEntity = SystemAPI.GetSingletonEntity<MysteryShipSpawner>();

        // Get necessary mystery ship spawner components
        RefRW<MysteryShipSpawner> mysteryShipSpawner = SystemAPI.GetComponentRW<MysteryShipSpawner>(mysteryShipSpawnerEntity);

        // Skip code during active ship
        if (mysteryShipSpawner.ValueRO.activeMysteryShip)
        {
            return;
        }

        // Reduce timer
        mysteryShipSpawner.ValueRW.timer -= SystemAPI.Time.DeltaTime;

        // Skip code during timer
        if (mysteryShipSpawner.ValueRO.timer > 0.0f)
        {
            return;
        }

        // Reset timer
        mysteryShipSpawner.ValueRW.timer = (float)UnityEngine.Random.Range(10, 30);

        // Instantiate mystery ship
        Entity mysteryShipEntity = state.EntityManager.Instantiate(mysteryShipSpawner.ValueRO.mysteryShipPrefab);

        mysteryShipSpawner.ValueRW.mysteryShipEntity = mysteryShipEntity;

        // Get necessary mystery ship components
        RefRW<MysteryShipMover> mysteryShipMover = SystemAPI.GetComponentRW<MysteryShipMover>(mysteryShipEntity);
        RefRW<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(mysteryShipEntity);

        // Spawn the mystery ship at a random position
        if (UnityEngine.Random.Range(0, 11) % 2 == 0)
        {
            // Spawn right and move it left when the random value is even
            mysteryShipLocalTransform.ValueRW.Position = new float3(17.0f, 13.0f, 0.0f);
            mysteryShipLocalTransform.ValueRW.Rotation = Quaternion.identity;

            mysteryShipMover.ValueRW.movementDirection = Vector3.left;
        }
        else
        {
            // Spawn left and move it riht when the random value is odd
            mysteryShipLocalTransform.ValueRW.Position = new float3(-17.0f, 13.0f, 0.0f);
            mysteryShipLocalTransform.ValueRW.Rotation = Quaternion.identity;

            mysteryShipMover.ValueRW.movementDirection = Vector3.right;
        }

        mysteryShipSpawner.ValueRW.activeMysteryShip = true;
    }
}