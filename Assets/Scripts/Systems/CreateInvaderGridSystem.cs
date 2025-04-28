using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct CreateInvaderGridSystem : ISystem
{
    Entity invaderGridEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (invaderGridEntity == Entity.Null || !state.EntityManager.Exists(invaderGridEntity))
        {
            if (!SystemAPI.HasSingleton<InvaderGrid>())
            {
                return;
            }

            invaderGridEntity = SystemAPI.GetSingletonEntity<InvaderGrid>();
        }

        if (!SystemAPI.IsComponentEnabled<Initialization>(invaderGridEntity))
        {
            return;
        }

        RefRO<InvaderGrid> invaderGrid = SystemAPI.GetComponentRO<InvaderGrid>(invaderGridEntity);
        RefRW<InvaderGridState> invaderGridState = SystemAPI.GetComponentRW<InvaderGridState>(invaderGridEntity);
        DynamicBuffer<InvaderTypes> invaderTypes = SystemAPI.GetBuffer<InvaderTypes>(invaderGridEntity);

        for (int row = 0; row < invaderGrid.ValueRO.rows; ++row)
        {
            float width = 2.0f * (float)(invaderGrid.ValueRO.columns - 1);
            float height = 2.0f * (float)(invaderGrid.ValueRO.rows - 1);

            float2 centering = new float2(-width / 2.0f, -height / 2.0f);
            float3 rowPosition = new float3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < invaderGrid.ValueRO.columns; ++col)
            {
                Entity invaderEntity = state.EntityManager.Instantiate(invaderTypes[row].invaderTypePrefabs);

                SystemAPI.SetComponent(invaderEntity, LocalTransform.FromPosition(invaderGrid.ValueRO.position));

                RefRW<LocalTransform> invaderLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(invaderEntity);
                RefRW<InvaderShoot> invaderShoot = SystemAPI.GetComponentRW<InvaderShoot>(invaderEntity);

                float3 position = rowPosition;
                position.x += col * 2.0f;

                invaderLocalTransform.ValueRW.Position += position;

                invaderShoot.ValueRW.invaderGridEntity = invaderGridEntity;

                ++invaderGridState.ValueRW.amountAlive;
            }
        }

        invaderGridState.ValueRW.totalAmount = invaderGrid.ValueRO.rows * invaderGrid.ValueRO.columns;

        invaderGridState.ValueRW.percentKilled =
            (float)invaderGridState.ValueRO.amountKilled / (float)invaderGridState.ValueRO.totalAmount;

        SystemAPI.SetComponentEnabled<Initialization>(invaderGridEntity, false);
    }
}