using UnityEngine;
using Unity.Entities;

public class BunkerAuthoring : MonoBehaviour
{
    public GameObject healthBar;

    public int maxHealth = 10;

    private class Baker : Baker<BunkerAuthoring>
    {
        public override void Bake(BunkerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Bunker
            {
                healthBar = GetEntity(authoring.healthBar, TransformUsageFlags.Dynamic),
                maxHealth = authoring.maxHealth,
                health = authoring.maxHealth,
            });
        }
    }
}

public struct Bunker : IComponentData
{
    public Entity healthBar;
    public int maxHealth;
    public int health;
}