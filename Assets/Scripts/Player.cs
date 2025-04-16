using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;

    public float movementSpeed = 5.0f;

    public int score = 0;

    private bool activeLaser = false;

    private void Update()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (transform.position.x < rightEdge.x - 1.0f)
            {
                transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            }
        }
        
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (transform.position.x > leftEdge.x + 1.0f)
            {
                transform.position += Vector3.left * movementSpeed * Time.deltaTime;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Missile") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Invader")  )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Shoot()
    {
        if (!activeLaser)
        {
            Projectile projectile = Instantiate(laserPrefab, transform.position, Quaternion.identity);

            projectile.destroyed += LaserDestroyed;

            activeLaser = true;
        }
    }

    private void LaserDestroyed()
    {
        activeLaser = false;
    }
}