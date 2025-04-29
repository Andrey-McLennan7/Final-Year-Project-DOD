using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
[UpdateBefore(typeof(DestroyProjectileSystem))]
partial struct PlayerProjectileResponseSystem : ISystem
{
    Entity playerEntity;

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

        RefRO<LocalTransform> playerLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);
        RefRO<BoxCollider> playerBoxCollider = SystemAPI.GetComponentRO<BoxCollider>(playerEntity);

        foreach ((RefRO<LocalTransform> projectileLocalTransform, RefRO<BoxCollider> projectileBoxCollider) in SystemAPI.Query<RefRO<LocalTransform>, RefRO<BoxCollider>>().WithPresent<Missile>())
        {
            if (!BoxCollisionResponseSystem.OnCollisionResponce(playerLocalTransform, playerBoxCollider,
                projectileLocalTransform, projectileBoxCollider))
            {
                continue;
            }

            UnityEngine.Debug.Log("YOU ARE DEAD NO BIG SUPRISE");

            break;
        }
    }
}