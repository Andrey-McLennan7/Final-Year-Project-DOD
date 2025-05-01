using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

public class InvaderAuthoring : MonoBehaviour
{
    private class Baker : Baker<InvaderAuthoring>
    {
        public override void Bake(InvaderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Invader>(entity);
        }
    }
}

public struct Invader : IComponentData { }