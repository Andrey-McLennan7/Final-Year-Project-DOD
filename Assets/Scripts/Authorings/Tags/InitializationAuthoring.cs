using UnityEngine;
using Unity.Entities;

public class InitializationAuthoring : MonoBehaviour
{
    private class Baker : Baker<InitializationAuthoring>
    {
        public override void Bake(InitializationAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Initialization>(entity);
        }
    }
}

public struct Initialization : IComponentData, IEnableableComponent { }