using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject options;

    private void OnEnable()
    {
        SoundManager.Instance.SetMenuAmbience(true);
    }

    private void OnDisable()
    {
        SoundManager.Instance.SetMenuAmbience(false);
    }

    public void Play()
    {
        SoundManager.Instance?.PlaySound(AudioID.UI_CLICK);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(1);
    }

    public void Options()
    {
        SoundManager.Instance?.PlaySound(AudioID.UI_CLICK);
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void Back()
    {
        SoundManager.Instance?.PlaySound(AudioID.UI_CLICK);
        options.SetActive(false);
        menu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}