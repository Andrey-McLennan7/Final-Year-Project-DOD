using UnityEngine;
using Unity.Entities;

public class PlayerAuthoring : MonoBehaviour
{
    public GameObject laserPrefab;

    public float movementSpeed;

    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Player
            {
                laserPrefab = GetEntity(authoring.laserPrefab, TransformUsageFlags.Dynamic),
                movementSpeed = authoring.movementSpeed,

                score = 0,
                activeLaser = false,
            });
        }
    }
}

public struct Player : IComponentData
{
    public Entity laserPrefab;
    public float movementSpeed;
    public int score;
    public bool activeLaser;
}