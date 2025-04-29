using UnityEngine;
using Unity.Entities;

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