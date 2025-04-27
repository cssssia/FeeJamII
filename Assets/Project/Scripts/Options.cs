using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject options;

    public void OpenOptions()
    {
        Time.timeScale = 0f;
        options.SetActive(true);
    }

    public void Menu()
    {
        SceneLoader.Instance.LoadScene(0);
    }

    public void Back()
    {
        options.SetActive(false);
        Time.timeScale = 1f;
    }
}
