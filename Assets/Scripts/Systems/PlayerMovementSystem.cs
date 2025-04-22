using UnityEngine;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get reference to the player
        Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();

        // Get necessary player components
        RefRW<PlayerMovement> playerMovement = SystemAPI.GetComponentRW<PlayerMovement>(playerEntity);
        RefRW<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(playerEntity);

        // Move right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            playerMovement.ValueRW.movementDirection = Vector3.right;

            if (playerLocalTransform.ValueRO.Position.x < 14.0f)
            {
                playerLocalTransform.ValueRW.Position += playerMovement.ValueRO.movementDirection * playerMovement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
            }
        }

        // Move left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            playerMovement.ValueRW.movementDirection = Vector3.left;

            if (playerLocalTransform.ValueRO.Position.x > -14.0f)
            {
                playerLocalTransform.ValueRW.Position += playerMovement.ValueRO.movementDirection * playerMovement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
            }
        }

        // Reset direction of movement
        playerMovement.ValueRW.movementDirection = float3.zero;
    }
}