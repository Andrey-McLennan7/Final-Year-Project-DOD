using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

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
                movementDirection = new float3(0.0f),
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}

public struct MysteryShipMover : IComponentData
{
    public float3 movementDirection;
    public float movementSpeed;
}