using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class InvaderGridMovementAuthoring : MonoBehaviour
{
    public float3 movementDirection;
    public float movementSpeed = 0.15f;

    private class Baker : Baker<InvaderGridMovementAuthoring>
    {
        public override void Bake(InvaderGridMovementAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new InvaderGridMovement
            {
                movementDirection = authoring.movementDirection,
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}

public struct InvaderGridMovement : IComponentData
{
    public float3 movementDirection;
    public float movementSpeed;
}