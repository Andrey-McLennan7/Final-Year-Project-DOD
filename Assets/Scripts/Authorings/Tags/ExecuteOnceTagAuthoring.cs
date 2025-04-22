using UnityEngine;
using Unity.Entities;

public class ExecuteOnceTagAuthoring : MonoBehaviour
{
    private class Baker : Baker<ExecuteOnceTagAuthoring>
    {
        public override void Bake(ExecuteOnceTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new ExecuteOnceTag { });
        }
    }
}

public struct ExecuteOnceTag : IComponentData { }