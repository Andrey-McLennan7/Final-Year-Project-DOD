using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct PlayerCollisionResponseSystem : ISystem
{
    Entity playerEntity;
    Entity resetGameEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (playerEntity == Entity.Null || !state.EntityManager.Exists(playerEntity))
        {
            if (!SystemAPI.HasSingleton<Player>())
            {
                return;
            }

            playerEntity = SystemAPI.GetSingletonEntity<Player>();
        }

        if (!SystemAPI.HasComponent<BoxCollider>(playerEntity))
        {
            return;
        }

        if (resetGameEntity == Entity.Null || !state.EntityManager.Exists(resetGameEntity))
        {
            if (!SystemAPI.HasSingleton<ResetGame>())
            {
                return;
            }

            resetGameEntity = SystemAPI.GetSingletonEntity<ResetGame>();
        }

        RefRO<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);
        RefRO<BoxCollider> playerBoxCollider = SystemAPI.GetComponentRO<BoxCollider>(playerEntity);

        foreach ((RefRO<LocalTransform> dangerLocalTransform, RefRO<BoxCollider> dangerBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Danger>())
        {
            if (!BoxCollisionResponseSystem.OnCollisionResponse(playerLocalTransform, playerBoxCollider,
                dangerLocalTransform, dangerBoxCollider))
            {
                continue;
            }

            SystemAPI.SetComponent(resetGameEntity, new ResetGame
            {
                reset = true,
            });

            break;
        }
    }
}