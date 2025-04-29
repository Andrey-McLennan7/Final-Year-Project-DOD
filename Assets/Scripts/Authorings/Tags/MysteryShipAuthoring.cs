using UnityEngine;
using Unity.Entities;

public class MysteryShipAuthoring : MonoBehaviour
{
    private class Baker : Baker<MysteryShipAuthoring>
    {
        public override void Bake(MysteryShipAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent<MysteryShip>(entity);
        }
    }
}

public struct MysteryShip : IComponentData {}