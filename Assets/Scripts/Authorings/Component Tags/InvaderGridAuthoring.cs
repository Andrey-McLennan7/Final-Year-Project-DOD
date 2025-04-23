using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

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