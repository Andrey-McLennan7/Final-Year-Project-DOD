using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class ProjectileAuthoring : MonoBehaviour
{
    public float3 movementDirection;
    public float movementSpeed;

    private class Baker : Baker<ProjectileAuthoring>
    {
        public override void Bake(ProjectileAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Projectile
            {
                movementDirection = authoring.movementDirection,
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}

public struct Projectile : IComponentData
{
    public Entity playerEntity;
    public float3 movementDirection;
    public float movementSpeed;
}