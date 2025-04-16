using UnityEngine;

public class MysteryShipSpawner : MonoBehaviour
{
    public MysteryShip mysteryShipPrefab;

    private float timer;
    private bool activeMysteryShip = false;

    private void Start()
    {
        timer = (float)Random.Range(10, 30);
    }

    private void Update()
    {
        if (!activeMysteryShip)
        {
            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                timer = (float)Random.Range(10, 30);

                MysteryShip mysteryShip = Instantiate(mysteryShipPrefab);

                Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
                Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

                if (Random.Range(0, 11) % 2 == 0)
                {
                    mysteryShip.transform.position = new Vector3(rightEdge.x + 3.0f, 13.0f, 0.0f);
                    mysteryShip.direction = Vector3.left;
                }
                else
                {
                    mysteryShip.transform.position = new Vector3(leftEdge.x - 3.0f, 13.0f, 0.0f);
                    mysteryShip.direction = Vector3.right;
                }

                mysteryShip.active += MysteryShipIsMoving;

                activeMysteryShip = true;
            }
        }
    }

    private void MysteryShipIsMoving()
    {
        activeMysteryShip = false;
    }
}