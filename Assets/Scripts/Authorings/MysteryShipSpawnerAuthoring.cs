using UnityEngine;
using Unity.Entities;
using System.Threading;

public class MysteryShipSpawnerAuthoring : MonoBehaviour
{
    public GameObject mysteryShipPrefab;

    private class Baker : Baker<MysteryShipSpawnerAuthoring>
    {
        public override void Bake(MysteryShipSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

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
    public float timer;
    public bool activeMysteryShip;
}