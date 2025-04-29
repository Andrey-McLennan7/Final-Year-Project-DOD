using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
partial struct ProjectileMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform, RefRO<Movement> movement) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Movement>>().WithPresent<Projectile>())
        {
            localTransform.ValueRW.Position +=
                movement.ValueRO.movementDirection *
                movement.ValueRO.movementSpeed *
                SystemAPI.Time.DeltaTime;
        }
    }
}