using UnityEngine;
using Unity.Entities;

/// <summary>
/// 
/// Tags can help narrow down which Entities to iterate through
/// or which singleton Entity to get
/// 
/// </summary>

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