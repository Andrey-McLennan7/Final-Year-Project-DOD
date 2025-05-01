using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;

/// <summary>
/// 
/// You cannot directly attach a DOTS component to an Entity.
/// You first need to create a authoring class that inhertis
/// from MonoBehaviour and then bakes game objects and pass
/// in the data from the inspector (or set default values)
/// 
/// The Baking System is a System and converts Unity GameObjects
/// into Unity DOTS Entities
/// 
/// </summary>

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