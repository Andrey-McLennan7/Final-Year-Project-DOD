using UnityEngine;
using Unity.Entities;

public class InvaderGridStateAuthoring : MonoBehaviour
{
    public float maxSpeed = 8.0f;
    private class Baker : Baker<InvaderGridStateAuthoring>
    {
        public override void Bake(InvaderGridStateAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new InvaderGridState
            {
                totalAmount = 0,
                amountKilled = 0,
                amountAlive = 0,
                percentKilled = 0.0f,
                maxSpeed = authoring.maxSpeed,
            });
        }
    }
}

public struct InvaderGridState : IComponentData
{
    public int totalAmount;
    public int amountKilled;
    public int amountAlive;
    public float percentKilled;
    public float maxSpeed;
}