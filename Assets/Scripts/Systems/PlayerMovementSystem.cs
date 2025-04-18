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
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach ((RefRW<LocalTransform> transform, RefRO<PlayerMovement> player) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<PlayerMovement>>())
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                if (transform.ValueRO.Position.x < (rightEdge.x - 1.0f))
                {
                    transform.ValueRW.Position += new float3(1.0f, 0.0f, 0.0f) * player.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (transform.ValueRO.Position.x > (leftEdge.x + 1.0f))
                {
                    transform.ValueRW.Position += new float3(-1.0f, 0.0f, 0.0f) * player.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;
                }
            }
        }
    }
}