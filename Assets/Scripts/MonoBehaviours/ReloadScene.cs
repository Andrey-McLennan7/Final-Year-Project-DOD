using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    EntityManager entityManager;

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    private void Update()
    {
        if (!entityManager.CreateEntityQuery(typeof(ResetGame)).HasSingleton<ResetGame>())
        {
            return;
        }

        Entity resetGameEntity = entityManager.CreateEntityQuery(typeof(ResetGame)).GetSingletonEntity();

        ResetGame resetGame = entityManager.GetComponentData<ResetGame>(resetGameEntity);

        if (!resetGame.reset)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}