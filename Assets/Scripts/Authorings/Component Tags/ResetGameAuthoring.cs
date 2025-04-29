using UnityEngine;
using Unity.Entities;

public class ResetGameAuthoring : MonoBehaviour
{
    private class Baker : Baker<ResetGameAuthoring>
    {
        public override void Bake(ResetGameAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new ResetGame
            {
                reset = false,
            });
        }
    }
}

public struct ResetGame : IComponentData
{
    public bool reset;
}