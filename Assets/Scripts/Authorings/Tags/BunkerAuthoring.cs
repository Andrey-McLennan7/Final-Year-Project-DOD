using UnityEngine;
using Unity.Entities;

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