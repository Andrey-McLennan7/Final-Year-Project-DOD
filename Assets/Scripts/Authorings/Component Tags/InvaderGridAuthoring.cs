using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// 
/// 'Component Tags' is a concept I came up with during development.
/// These are essentially hybrids between tags and components. They
/// allow for narrowing down which components to iterate through or
/// get a singleton from, and for passing in data from the inspector
/// (or set the default values) and the baking system
/// 
/// </summary>

public class InvaderGridAuthoring : MonoBehaviour
{
    public int rows = 5;
    public int columns = 11;

    private class Baker : Baker<InvaderGridAuthoring>
    {
        public override void Bake(InvaderGridAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new InvaderGrid
            {
                rows = authoring.rows,
                columns = authoring.columns,
                position = new float3(authoring.transform.position),
            });
        }
    }
}

public struct InvaderGrid : IComponentData
{
    public int rows;
    public int columns;

    public float3 position;
}