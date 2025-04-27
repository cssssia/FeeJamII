using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int p_sceneIndex)
    {
        SceneManager.LoadScene(p_sceneIndex);
    }
}
