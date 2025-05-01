using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// 'Component Tags' is a concept I came up with during development.
/// These are essentially hybrids between tags and components. They
/// allow for narrowing down which components to iterate through or
/// get a singleton from, and for passing in data from the inspector
/// (or set the default values) and the baking system
/// 
/// </summary>

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