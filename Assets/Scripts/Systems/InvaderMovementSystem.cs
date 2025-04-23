using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
[UpdateAfter(typeof(CreateInvaderGridSystem))]
partial struct InvaderMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform, RefRO<Movement> movement, RefRO<Invader> invader) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Movement>, RefRO<Invader>>())
        {
            localTransform.ValueRW.Position +=
                movement.ValueRO.movementDirection * movement.ValueRO.movementSpeed * SystemAPI.Time.DeltaTime;

            if (movement.ValueRO.movementDirection.x > 0.0f && localTransform.ValueRO.Position.x >= ( 14.0f) ||
                movement.ValueRO.movementDirection.x < 0.0f && localTransform.ValueRO.Position.x <= (-14.0f)  )
            {
                AdvanceRow(ref state);
                break;
            }
        }
    }

    [BurstCompile]
    private void AdvanceRow(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform, RefRW<Movement> movement, RefRO<Invader> invader) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Movement>, RefRO<Invader>>())
        {
            movement.ValueRW.movementDirection.x *= -1.0f;

            float3 position = localTransform.ValueRW.Position;
            position.y -= 1.0f;
            localTransform.ValueRW.Position = position;
        }   
    }
}