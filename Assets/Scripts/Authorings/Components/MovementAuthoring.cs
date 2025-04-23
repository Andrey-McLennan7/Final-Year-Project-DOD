using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class MovementAuthoring : MonoBehaviour
{
    public float movementSpeed = 0.0f;
    public float3 movementDirection = new float3(0.0f);

    private class Baker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Movement
            {
                movementSpeed = authoring.movementSpeed,
                movementDirection = authoring.movementDirection,
            });
        }
    }
}

public struct Movement : IComponentData
{
    public float movementSpeed;
    public float3 movementDirection;
}