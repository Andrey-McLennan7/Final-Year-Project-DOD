using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class ProjectileAuthoring : MonoBehaviour
{
    private class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Projectile { });
        }
    }
}

public struct Projectile : IComponentData
{
    public Entity entityThatShot;
}