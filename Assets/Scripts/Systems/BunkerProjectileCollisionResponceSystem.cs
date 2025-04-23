using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]
partial struct BunkerProjectileCollisionResponceSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRO<LocalToWorld> localToWorld, RefRW<BunkerHealth> bunkerHealth) in SystemAPI.Query<RefRO<LocalToWorld>, RefRW<BunkerHealth>>())
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                --bunkerHealth.ValueRW.health;

                float normalisedHealth = (float)bunkerHealth.ValueRO.health / (float)bunkerHealth.ValueRO.maxHealth;

                RefRW<PostTransformMatrix> bunkerHealthBarTransformMatrix = SystemAPI.GetComponentRW<PostTransformMatrix>(bunkerHealth.ValueRW.healthBar);

                bunkerHealthBarTransformMatrix.ValueRW.Value =
                    float4x4.Scale(normalisedHealth * localToWorld.ValueRO.Value.c0.x, localToWorld.ValueRO.Value.c1.y, 1.0f);
            }
        }
    }
}