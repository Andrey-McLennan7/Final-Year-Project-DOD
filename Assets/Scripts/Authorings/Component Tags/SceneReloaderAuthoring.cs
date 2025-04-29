using UnityEngine;
using Unity.Entities;

public class SceneReloaderAuthoring : MonoBehaviour
{
    private class Baker : Baker<SceneReloaderAuthoring>
    {
        public override void Bake(SceneReloaderAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SceneReloader
            {
                reload = false,
            });
        }
    }
}

public struct SceneReloader : IComponentData
{
    public bool reload;
}