using UnityEngine;
using Unity.Entities;

public class PlayerShootAuthoring : MonoBehaviour
{
    public GameObject laserPrefab;

    private class Baker : Baker<PlayerShootAuthoring>
    {
        public override void Bake(PlayerShootAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerShoot
            {
                laserPrefab = GetEntity(authoring.laserPrefab, TransformUsageFlags.Dynamic),
                activeLaser = false,
            });
        }
    }
}

public struct PlayerShoot : IComponentData
{
    public Entity laserPrefab;
    public bool activeLaser;
}