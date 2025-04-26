using UnityEngine;
using Unity.Entities;

public class EnemyAuthoring : MonoBehaviour
{
    private class Baker : Baker<EnemyAuthoring>
    {
        public override void Bake(EnemyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Enemy { });
        }
    }
}

public struct Enemy : IComponentData { }