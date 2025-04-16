using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Projectile missilePrefab;

    public System.Action killed;
    public int score = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            killed.Invoke();

            Destroy(gameObject);
        }
    }
}