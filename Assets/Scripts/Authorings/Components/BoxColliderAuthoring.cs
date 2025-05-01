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

public class BoxColliderAuthoring : MonoBehaviour
{
    public float3 size;
    public float3 offset;

    private class Baker : Baker<BoxColliderAuthoring>
    {
        public override void Bake(BoxColliderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new BoxCollider
            {
                size = authoring.size,
                offset = authoring.offset,
            });
        }
    }
}

public struct BoxCollider : IComponentData
{
    public float3 size;
    public float3 offset;
}