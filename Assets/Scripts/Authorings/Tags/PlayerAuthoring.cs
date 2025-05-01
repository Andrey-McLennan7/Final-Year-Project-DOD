using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

public class PlayerAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Player>(entity);
        }
    }
}

public struct Player : IComponentData { }