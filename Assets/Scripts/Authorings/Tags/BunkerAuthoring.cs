using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

public class BunkerAuthoring : MonoBehaviour
{
    private class Baker : Baker<BunkerAuthoring>
    {
        public override void Bake(BunkerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);

            AddComponent<Bunker>(entity);
        }
    }
}

public struct Bunker : IComponentData { }