using UnityEngine;
using Unity.Entities;

public class PlayerMovementAuthoring : MonoBehaviour
{
    public float movementSpeed;

    private class Baker : Baker<PlayerMovementAuthoring>
    {
        public override void Bake(PlayerMovementAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerMovement
            {
                movementSpeed = authoring.movementSpeed,
            });
        }
    }
}

public struct PlayerMovement : IComponentData
{
    public float movementSpeed;
}