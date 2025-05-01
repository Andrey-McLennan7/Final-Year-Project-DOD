using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// You cannot directly attach a DOTS component to an Entity.
/// You first need to create a authoring class that inhertis
/// from MonoBehaviour and then bakes game objects and pass
/// in the data from the inspector (or set default values)
/// 
/// The Baking System is a System and converts Unity GameObjects
/// into Unity DOTS Entities
/// 
/// </summary>

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
                healthBar = GetEntity(authoring.healthBar, TransformUsageFlags.NonUniformScale),
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