using UnityEngine;
using Unity.Entities;

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

                    buffer.Add(new InvaderTypes { invaderTypePrefabs = invaderTypeEntity });
                }
            }
        }
    }
}

public struct InvaderTypes : IBufferElementData
{
    public Entity invaderTypePrefabs;
}