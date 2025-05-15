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

public class InvaderTypesAuthoring : MonoBehaviour
{
    public GameObject[] invaderTypePrefabs;

    private class Baker : Baker<InvaderTypesAuthoring>
    {
        public override void Bake(InvaderTypesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            DynamicBuffer<InvaderTypes> buffer = AddBuffer<InvaderTypes>(entity);

            foreach (GameObject invaderType in authoring.invaderTypePrefabs)
            {
                if (invaderType != null)
                {
                    Entity invaderTypeEntity = GetEntity(invaderType, TransformUsageFlags.Dynamic);

                    buffer.Add(new InvaderTypes { invaderTypePrefab = invaderTypeEntity });
                }
            }
        }
    }
}

public struct InvaderTypes : IBufferElementData
{
    public Entity invaderTypePrefab;
}