using UnityEngine;
using Unity.Entities;

public class PlayerAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), new Player { });
        }
    }
}

public struct Player : IComponentData { }