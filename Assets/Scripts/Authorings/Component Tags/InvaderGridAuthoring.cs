using UnityEngine;
using Unity.Entities;

public class InvaderGridAuthoring : MonoBehaviour
{
    public GameObject[] invaderTypes;

    public int rows = 5;
    public int columns = 11;

    private class Baker : Baker<InvaderGridAuthoring>
    {
        public override void Bake(InvaderGridAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new InvaderGrid
            {
                invader1 = GetEntity(authoring.invaderTypes[0], TransformUsageFlags.Dynamic),
                invader2 = GetEntity(authoring.invaderTypes[1], TransformUsageFlags.Dynamic),
                invader3 = GetEntity(authoring.invaderTypes[2], TransformUsageFlags.Dynamic),

                rows = authoring.rows,
                columns = authoring.columns,
            });
        }
    }
}

public struct InvaderGrid : IComponentData
{
    public Entity invader1;
    public Entity invader2;
    public Entity invader3;

    public int rows;
    public int columns;
}