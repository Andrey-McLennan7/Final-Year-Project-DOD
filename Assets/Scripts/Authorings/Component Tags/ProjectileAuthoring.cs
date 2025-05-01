using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// 'Component Tags' is a concept I came up with during development.
/// These are essentially hybrids between tags and components. They
/// allow for narrowing down which components to iterate through or
/// get a singleton from, and for passing in data from the inspector
/// (or set the default values) and the baking system
/// 
/// </summary>


public class ProjectileAuthoring : MonoBehaviour
{
    private class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Projectile>(entity);
        }
    }
}

public struct Projectile : IComponentData
{
    public Entity entityThatShot;
}