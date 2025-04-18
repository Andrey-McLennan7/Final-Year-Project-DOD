using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerMovementAuthoring : MonoBehaviour
{
    public float movementSpeed;

    private class Baker : Baker<PlayerMovementAuthoring>
    {
        public override void Bake(PlayerMovementAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerMovement
            {
                movementSpeed = authoring.movementSpeed,
                movementDirection = new float3(0.0f),
            });
        }
    }
}

public struct PlayerMovement : IComponentData
{
    public float movementSpeed;
    public float3 movementDirection;
}