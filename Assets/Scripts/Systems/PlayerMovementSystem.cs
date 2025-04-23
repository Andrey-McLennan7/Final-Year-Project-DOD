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
        RefRW<Movement> playerMovement = SystemAPI.GetComponentRW<Movement>(playerEntity);
        RefRW<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(playerEntity);

        // Move right
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.D))
        {
            playerMovement.ValueRW.movementDirection = UnityEngine.Vector3.right;

            if (playerLocalTransform.ValueRO.Position.x < 14.0f)
            {
                playerLocalTransform.ValueRW.Position += playerMovement.ValueRO.movementDirection * playerMovement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
            }
        }

        // Move left
        if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow) || UnityEngine.Input.GetKey(UnityEngine.KeyCode.A))
        {
            playerMovement.ValueRW.movementDirection = UnityEngine.Vector3.left;

            if (playerLocalTransform.ValueRO.Position.x > -14.0f)
            {
                playerLocalTransform.ValueRW.Position += playerMovement.ValueRO.movementDirection * playerMovement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
            }
        }

        // Reset direction of movement
        playerMovement.ValueRW.movementDirection = float3.zero;
    }
}