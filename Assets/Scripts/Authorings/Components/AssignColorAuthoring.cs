using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;

public class AssignColorAuthoring : MonoBehaviour
{
    public float R;
    public float G;
    public float B;
    public float A;

    private class Baker : Baker<AssignColorAuthoring>
    {
        public override void Bake(AssignColorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new URPMaterialPropertyBaseColor
            {
                Value = new float4(authoring.R,
                                   authoring.G,
                                   authoring.B,
                                   authoring.A),
            });
        }
    }
}