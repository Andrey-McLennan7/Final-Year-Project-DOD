using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

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