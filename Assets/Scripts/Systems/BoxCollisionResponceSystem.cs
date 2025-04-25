using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public static class BoxCollisionResponceSystem
{
    public static bool OnCollisionResponce(RefRO<LocalTransform> localTransform1, RefRO<BoxCollider> boxCollider1,
                                           RefRO<LocalTransform> localTransform2, RefRO<BoxCollider> boxCollider2)
    {
        float3 a = localTransform1.ValueRO.Position + boxCollider1.ValueRO.offset;
        float3 b = localTransform2.ValueRO.Position + boxCollider2.ValueRO.offset;

        float3 ahs = boxCollider1.ValueRO.size / 2.0f;
        float3 bhs = boxCollider2.ValueRO.size / 2.0f;

        return math.abs(a.x - b.x) <= (ahs.x + bhs.x) &&
               math.abs(a.y - b.y) <= (ahs.y + bhs.y) &&
               math.abs(a.z - b.z) <= (ahs.z + bhs.z);
    }
}