using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    private void Update()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        if (!entityManager.CreateEntityQuery(typeof(Player)).HasSingleton<Player>())
        {
            return;
        }

        Entity playerEntity = entityManager.CreateEntityQuery(typeof(Player)).GetSingletonEntity();

        Player player = entityManager.GetComponentData<Player>(playerEntity);

        if (!player.destroyed)
        {
            return;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}