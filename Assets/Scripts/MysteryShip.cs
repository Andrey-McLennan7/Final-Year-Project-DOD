using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class MysteryShip : MonoBehaviour
{
    public MysteryShipSpawner mysteryShipSpawner;
    public float speed = 7.0f;
    public Vector3 direction { get; set; }
    public System.Action active;

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        if (transform.position.x <= (leftEdge.x - 3.0f) || transform.position.x >= (rightEdge.x + 3.0f))
        {
            DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        if (active != null)
        {
            active.Invoke();
        }

        Destroy(gameObject);
    }
}