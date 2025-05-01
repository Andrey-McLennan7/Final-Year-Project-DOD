using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>
/// 

/// <summary>
/// 
/// Enableable components allows the program to remove components
/// without making any structural changes to the list of Entities,
/// as this would force the program to reorganise everything in the
/// memory, which takes time
/// 
/// </summary>

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