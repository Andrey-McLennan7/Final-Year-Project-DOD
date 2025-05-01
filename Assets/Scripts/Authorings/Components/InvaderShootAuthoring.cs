using UnityEngine;
using Unity.Entities;

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