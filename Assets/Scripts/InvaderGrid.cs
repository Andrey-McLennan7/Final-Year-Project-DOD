using UnityEngine;
using UnityEngine.SceneManagement;

public class InvaderGrid : MonoBehaviour
{
    public Invader[] invaders;
    public Projectile missilePrefab;

    public int rows = 5;
    public int columns = 11;

    private float speed = 0.15f;

    public int totalAmount => rows * columns;
    public int amountKilled { get; private set; }
    public int amountAlive => totalAmount - amountKilled;
    public float percentKilled => (float)amountKilled / (float)totalAmount;

    private Vector3 direction = Vector3.right;

    private void Awake()
    {
        for (int row = 0; row < rows; row++)
        {
            float width = 2.0f * (columns - 1);
            float height = 2.0f * (rows - 1);

            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + row * 2.0f, 0.0f);

            for(int col = 0; col < columns; col++)
            {
                Invader invader = Instantiate(invaders[row], transform);

                Vector3 position = rowPosition;

                invader.killed += InvaderKilled;

                position.x += col * 2.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in transform)
        {
            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.0f) ||
                direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.0f))
            {
                AdvanceRow();
            }
        }

        if (Random.Range(0, 501) == 500)
        {
            foreach (Transform invader in transform)
            {
                if (Random.value < (1.0f / (float)amountAlive))
                {
                    Instantiate(missilePrefab, invader.position, Quaternion.identity);
                    break;
                }
            }
        }
    }

    private void AdvanceRow()
    {
        direction.x *= -1.0f;

        Vector3 position = transform.position;
        position.y -= 1.0f;
        transform.position = position;
    }

    private void InvaderKilled()
    {
        ++amountKilled;
        speed = percentKilled * 8.0f;

        if (amountKilled >= totalAmount)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}