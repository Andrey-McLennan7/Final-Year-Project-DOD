using UnityEngine;
using Unity.Entities;

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