using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// You cannot directly attach a DOTS component to an Entity.
/// You first need to create a authoring class that inhertis
/// from MonoBehaviour and then bakes game objects and pass
/// in the data from the inspector (or set default values)
/// 
/// The Baking System is a System and converts Unity GameObjects
/// into Unity DOTS Entities
/// 
/// </summary>

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