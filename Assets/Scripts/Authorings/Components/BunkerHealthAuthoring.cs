using UnityEngine;
using Unity.Entities;

public class BunkerHealthAuthoring : MonoBehaviour
{
    public GameObject healthBar;

    public int maxHealth = 10;

    private class Baker : Baker<BunkerHealthAuthoring>
    {
        public override void Bake(BunkerHealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);

            AddComponent(entity, new BunkerHealth
            {
                healthBar = GetEntity(authoring.healthBar, TransformUsageFlags.Renderable),
                maxHealth = authoring.maxHealth,
                health = authoring.maxHealth,
            });
        }
    }
}

public struct BunkerHealth : IComponentData
{
    public Entity healthBar;
    public int maxHealth;
    public int health;
}