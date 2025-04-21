using UnityEngine;
using Unity.Entities;

public class MysteryShipMoverAuthoring : MonoBehaviour
{
    public float movementSpeed;

    private class Baker : Baker<MysteryShipMoverAuthoring>
    {
        public override void Bake(MysteryShipMoverAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MysteryShipMover
            {
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}

public struct MysteryShipMover : IComponentData
{
    public Entity mysteryShipSpawnerEntity;
    public float movementSpeed;
}