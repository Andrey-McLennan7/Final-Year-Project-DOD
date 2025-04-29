using UnityEngine;
using Unity.Entities;

public class PlayerAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Player
            {
                destroyed = false,
            });
        }
    }
}

public struct Player : IComponentData
{
    public bool destroyed;
}