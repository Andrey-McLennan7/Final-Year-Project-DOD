using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct PlayerMovementSystem : ISystem
{
    // Reference Entity once
    Entity playerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is Null or the Entity no longer 
        if (playerEntity == Entity.Null || !state.EntityManager.Exists(playerEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<Player>())
            {
                return;
            }

            // Get reference to the singleton entity
            playerEntity = SystemAPI.GetSingletonEntity<Player>();
        }

        // Get the refernces of the necessary components of the Entity
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