using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

public class DangerAuthoring : MonoBehaviour
{
    private class Baker : Baker<DangerAuthoring>
    {
        public override void Bake(DangerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Danger>(entity);
        }
    }
}

public struct Danger : IComponentData { }