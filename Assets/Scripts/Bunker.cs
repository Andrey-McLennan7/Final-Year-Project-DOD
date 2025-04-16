using UnityEngine;

public class Bunker : MonoBehaviour
{
    public GameObject healthBar;

    public int maxHealth = 10;
    private int health;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        float normalisedHealth = (float)health / (float)maxHealth;

        healthBar.transform.localScale = new Vector3(normalisedHealth, 1.0f, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Laser") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Missile"))
        {
            --health;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            gameObject.SetActive(false);
        }
    }
}