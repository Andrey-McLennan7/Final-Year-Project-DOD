using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
partial struct SpawnMysteryShipSystem : ISystem
{
    // Reference Entity once
    Entity mysteryShipSpawnerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is Null or the Entity no longer 
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

        // Get the refernces of the necessary components of the Entity
        RefRW<Movement> mysteryShipMover = SystemAPI.GetComponentRW<Movement>(mysteryShipEntity);

        // Spawn the mystery ship at a random position
        if (UnityEngine.Random.Range(0, 11) % 2 == 0)
        {
            // Spawn right and move it left when the random value is even
            SystemAPI.SetComponent<LocalTransform>(mysteryShipEntity,
                LocalTransform.FromPositionRotation(new float3(17.0f, 13.0f, 0.0f), quaternion.identity));

            mysteryShipMover.ValueRW.movementDirection = Vector3.left;
        }
        else
        {
            // Spawn left and move it riht when the random value is odd
            SystemAPI.SetComponent<LocalTransform>(mysteryShipEntity,
                LocalTransform.FromPositionRotation(new float3(-17.0f, 13.0f, 0.0f), quaternion.identity));

            mysteryShipMover.ValueRW.movementDirection = Vector3.right;
        }

        mysteryShipSpawner.ValueRW.activeMysteryShip = true;
    }
}