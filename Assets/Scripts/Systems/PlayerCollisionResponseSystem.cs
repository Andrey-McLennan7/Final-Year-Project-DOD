using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct PlayerCollisionResponseSystem : ISystem
{
    // Reference Entities once
    Entity playerEntity;
    Entity resetGameEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Check if the entity reference is Null or the Entity no longer exists
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

        // Skip the code if the Entity does not have the following component
        if (!SystemAPI.HasComponent<BoxCollider>(playerEntity))
        {
            return;
        }

        // Check if the entity reference is Null or the Entity no longer exists
        if (resetGameEntity == Entity.Null || !state.EntityManager.Exists(resetGameEntity))
        {
            // Also check if of the singleton Entity exists in the scene
            if (!SystemAPI.HasSingleton<ResetGame>())
            {
                return;
            }

            // Get reference to the singleton entity
            resetGameEntity = SystemAPI.GetSingletonEntity<ResetGame>();
        }

        // Get the refernces of the necessary components of the Entity
        RefRO<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);
        RefRO<BoxCollider> playerBoxCollider = SystemAPI.GetComponentRO<BoxCollider>(playerEntity);

        foreach ((RefRO<LocalTransform> dangerLocalTransform, RefRO<BoxCollider> dangerBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Danger>())
        {
            // Skip the code if no collision is detected
            if (!BoxCollisionResponseSystem.OnCollisionResponse(playerLocalTransform, playerBoxCollider,
                dangerLocalTransform, dangerBoxCollider))
            {
                continue;
            }

            // Reset the game
            SystemAPI.SetComponent(resetGameEntity, new ResetGame
            {
                reset = true,
            });

            break;
        }
    }
}