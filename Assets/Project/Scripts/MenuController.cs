using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;

    public void Play()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(1);
    }

    public void Options()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void Back()
    {
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
