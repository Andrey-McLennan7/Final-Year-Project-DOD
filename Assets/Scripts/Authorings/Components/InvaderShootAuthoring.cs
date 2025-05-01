using UnityEngine;
using Unity.Entities;

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

public class InvaderShootAuthoring : MonoBehaviour
{
    public GameObject missilePrefab;
    public int shootInfrequency;

    private class Baker : Baker<InvaderShootAuthoring>
    {
        public override void Bake(InvaderShootAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new InvaderShoot
            {
                missilePrefab = GetEntity(authoring.missilePrefab, TransformUsageFlags.Dynamic),
                shootInfrequency = authoring.shootInfrequency,
                activeMissile = false,
            });
        }
    }
}

public struct InvaderShoot : IComponentData
{
    public Entity invaderGridEntity;
    public Entity missilePrefab;

    public int shootInfrequency;
    public bool activeMissile;
}