using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

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