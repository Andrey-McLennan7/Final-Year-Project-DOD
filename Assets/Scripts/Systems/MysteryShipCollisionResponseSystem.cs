using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
[UpdateBefore(typeof(DestroyOffSreenMysteryShipSystem))]
partial struct MysteryShipCollisionResponseSystem : ISystem
{
    // Reference Entity once
    Entity mysteryShipSpawnerEntity;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        /// <summary>
        /// 
        /// The Entity Command Buffer is best suited for destroying looped entity,
        /// as it does not destroy them until the end of an interator is reached
        /// 
        /// </summary>

        // Make sure that the Entity Command Buffer exists before running this system
        state.RequireForUpdate<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get reference to the Entity Command Buffer
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        // Check if the entity reference is Null or the Entity no longer exists
        if (mysteryShipSpawnerEntity == Entity.Null || !state.EntityManager.Exists(mysteryShipSpawnerEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<MysteryShipSpawner>())
            {
                return;
            }

            // Get reference to the singleton entity
            mysteryShipSpawnerEntity = SystemAPI.GetSingletonEntity<MysteryShipSpawner>();
        }

        // Get the references of the necessary components of the Entity
        RefRW<MysteryShipSpawner> mysteryShipSpawner = SystemAPI.GetComponentRW<MysteryShipSpawner>(mysteryShipSpawnerEntity);

        // Skip code if the Entity reference is Null or the Entity no longer exists
        if (mysteryShipSpawner.ValueRO.mysteryShipEntity == Entity.Null ||
            !state.EntityManager.Exists(mysteryShipSpawner.ValueRO.mysteryShipEntity))
        {
            return;
        }

        // Get the refernces of the necessary components of the Entity
        Entity mysteryShipEntity = mysteryShipSpawner.ValueRO.mysteryShipEntity;

        // Skip the code if the Entity does not have the following component
        if (!SystemAPI.HasComponent<BoxCollider>(mysteryShipEntity))
        {
            return;
        }

        // Get the refernces of the necessary components of the Entity
        RefRO<LocalTransform> mysteryShipLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(mysteryShipEntity);
        RefRO<BoxCollider> mysteryShipBoxCollider = SystemAPI.GetComponentRO<BoxCollider>(mysteryShipEntity);

        foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Laser>())
        {
            // Skip the code if no collision is detected
            if (!BoxCollisionResponseSystem.OnCollisionResponse(mysteryShipLocalTransform, mysteryShipBoxCollider,
                projectileLocalTransform, projectileBoxCollider))
            {
                continue;
            }

            mysteryShipSpawner.ValueRW.activeMysteryShip = false;

            // Queue the Entity to be destroyed
            entityCommandBuffer.DestroyEntity(mysteryShipEntity);

            break;
        }
    }
}