using UnityEngine.SceneManagement;

public static class ReloadSceneSystem
{
    public static void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}