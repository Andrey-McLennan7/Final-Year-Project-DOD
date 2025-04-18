using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();

        RefRW<PlayerMovement> playerMovement = SystemAPI.GetComponentRW<PlayerMovement>(playerEntity);
        RefRW<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(playerEntity);

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            playerMovement.ValueRW.movementDirection = Vector3.right;

            if (playerLocalTransform.ValueRO.Position.x < (rightEdge.x - 1.0f))
            {
                playerLocalTransform.ValueRW.Position += playerMovement.ValueRO.movementDirection * playerMovement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            playerMovement.ValueRW.movementDirection = Vector3.left;

            if (playerLocalTransform.ValueRO.Position.x > (leftEdge.x + 1.0f))
            {
                playerLocalTransform.ValueRW.Position += playerMovement.ValueRO.movementDirection * playerMovement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
            }
        }

        playerMovement.ValueRW.movementDirection = float3.zero;
    }
}