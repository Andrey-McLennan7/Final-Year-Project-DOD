using UnityEngine;
using Unity.Entities;

public class DangerAuthoring : MonoBehaviour
{
    private class Baker : Baker<DangerAuthoring>
    {
        public override void Bake(DangerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<Danger>(entity);
        }
    }
}

public struct Danger : IComponentData { }