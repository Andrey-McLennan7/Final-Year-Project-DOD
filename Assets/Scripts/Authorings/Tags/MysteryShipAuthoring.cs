using UnityEngine;
using Unity.Entities;

public class MysteryShipAuthoring : MonoBehaviour
{
    private class Baker : Baker<MysteryShipAuthoring>
    {
        public override void Bake(MysteryShipAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MysteryShip { });
        }
    }
}

public struct MysteryShip : IComponentData {}