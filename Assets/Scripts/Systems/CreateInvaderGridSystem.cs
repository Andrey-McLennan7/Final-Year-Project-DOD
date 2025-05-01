using Unity.Entities;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct CreateInvaderGridSystem : ISystem
{
    // Reference Entity once
    Entity invaderGridEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is null or no longer exists
        if (invaderGridEntity == Entity.Null || !state.EntityManager.Exists(invaderGridEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<InvaderGrid>())
            {
                return;
            }

            // Get reference to the singleton entity
            invaderGridEntity = SystemAPI.GetSingletonEntity<InvaderGrid>();
        }

        // Check if the initialization component is enabled
        if (!SystemAPI.IsComponentEnabled<Initialization>(invaderGridEntity))
        {
            /// <summary>
            /// 
            /// This is to ensure that the following code only executes once
            /// as there is no system available in this version of Unity DOTS
            /// similar to the standard Unity Awake() or Start() functions
            /// 
            /// </summary>

            return;
        }

        // Get the references to the necessary components of the Entity
        RefRO<InvaderGrid> invaderGrid = SystemAPI.GetComponentRO<InvaderGrid>(invaderGridEntity);
        RefRW<InvaderGridState> invaderGridState = SystemAPI.GetComponentRW<InvaderGridState>(invaderGridEntity);
        DynamicBuffer<InvaderTypes> invaderTypes = SystemAPI.GetBuffer<InvaderTypes>(invaderGridEntity);

        for (int row = 0; row < invaderGrid.ValueRO.rows; ++row)
        {
            // Calculate the width and height of the grid
            // based on the number rows and columns
            float width = 2.0f * (float)(invaderGrid.ValueRO.columns - 1);
            float height = 2.0f * (float)(invaderGrid.ValueRO.rows - 1);

            // Calcuate the centre of the grid
            float2 centering = new float2(-width / 2.0f, -height / 2.0f);

            // Calculate the first/next row position
            float3 rowPosition = new float3(centering.x, centering.y + (row * 2.0f), 0.0f);

            for (int col = 0; col < invaderGrid.ValueRO.columns; ++col)
            {
                // Instantiate the invader
                Entity invaderEntity = state.EntityManager.Instantiate(invaderTypes[row].invaderTypePrefabs);

                // Set the initial position of the invader
                SystemAPI.SetComponent(invaderEntity, LocalTransform.FromPosition(invaderGrid.ValueRO.position));

                // Get the references to the necessary components of the invader Entity
                RefRW<LocalTransform> invaderLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(invaderEntity);
                RefRW<InvaderShoot> invaderShoot = SystemAPI.GetComponentRW<InvaderShoot>(invaderEntity);

                // Calculate the first/next column position
                float3 position = rowPosition;
                position.x += col * 2.0f;

                // Update the position of the first/next invader
                invaderLocalTransform.ValueRW.Position += position;

                // Keep reference to the invader grid Entity
                invaderShoot.ValueRW.invaderGridEntity = invaderGridEntity;

                // Record the amount alive
                ++invaderGridState.ValueRW.amountAlive;
            }
        }

        // Calculate the total amount based on the number of rows and columns
        invaderGridState.ValueRW.totalAmount = invaderGrid.ValueRO.rows * invaderGrid.ValueRO.columns;

        // Calculate the initial percentage 
        invaderGridState.ValueRW.percentKilled =
            (float)invaderGridState.ValueRO.amountKilled / (float)invaderGridState.ValueRO.totalAmount;

        /// <summary>
        /// 
        /// The component is set to enableable to not make any structural changes to
        /// the Entity structure to not waste too much time on reorganising data in memory
        /// 
        /// </summary>

        // Disable the initialization component
        SystemAPI.SetComponentEnabled<Initialization>(invaderGridEntity, false);
    }
}