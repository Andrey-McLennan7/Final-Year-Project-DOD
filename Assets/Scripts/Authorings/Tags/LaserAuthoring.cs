using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

public class LaserAuthoring : MonoBehaviour
{
    private class Baker : Baker<LaserAuthoring>
    {
        public override void Bake(LaserAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Laser>(entity);
        }
    }
}

public struct Laser : IComponentData { }