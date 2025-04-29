using UnityEngine;
using Unity.Entities;

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