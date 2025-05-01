using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// 'Component Tags' is a concept I came up with during development.
/// These are essentially hybrids between tags and components. They
/// allow for narrowing down which components to iterate through or
/// get a singleton from, and for passing in data from the inspector
/// (or set the default values) and the baking system
/// 
/// </summary>

public class MysteryShipSpawnerAuthoring : MonoBehaviour
{
    public GameObject mysteryShipPrefab;

    private class Baker : Baker<MysteryShipSpawnerAuthoring>
    {
        public override void Bake(MysteryShipSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new MysteryShipSpawner
            {
                mysteryShipPrefab = GetEntity(authoring.mysteryShipPrefab, TransformUsageFlags.Dynamic),
                timer = (float)Random.Range(10, 30),
                activeMysteryShip = false,
            });
        }
    }
}

public struct MysteryShipSpawner : IComponentData
{
    public Entity mysteryShipPrefab;
    public Entity mysteryShipEntity;

    public float timer;
    public bool activeMysteryShip;
}