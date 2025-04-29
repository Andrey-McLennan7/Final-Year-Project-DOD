using UnityEngine;
using Unity.Entities;

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