using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;
using Unity.Mathematics;

partial struct BunkerProjectileCollisionResponceSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (RefRW<Bunker> bunker in SystemAPI.Query<RefRW<Bunker>>())
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                --bunker.ValueRW.health;
            }

            RefRW<PostTransformMatrix> bunkerHealthBarTransformformMatrix = SystemAPI.GetComponentRW<PostTransformMatrix>(bunker.ValueRW.healthBar);

            float normalisedHealth = (float)bunker.ValueRO.health / (float)bunker.ValueRO.maxHealth;

            bunkerHealthBarTransformformMatrix.ValueRW.Value = float4x4.Scale(normalisedHealth, 1.0f, 1.0f);
        }
    }
}