using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

public class MissileAuthoring : MonoBehaviour
{
    private class Baker : Baker<MissileAuthoring>
    {
        public override void Bake(MissileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Missile>(entity);
        }
    }
}

public struct Missile : IComponentData { }