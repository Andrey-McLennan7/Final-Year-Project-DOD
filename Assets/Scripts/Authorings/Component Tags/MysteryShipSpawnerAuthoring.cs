using UnityEngine;
using Unity.Entities;

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