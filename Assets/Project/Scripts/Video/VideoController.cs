using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    [FormerlySerializedAs("m_videoController")] [SerializeField]
    private VideoPlayer m_videoPlayer;

    private void OnEnable()
    {
        StartCoroutine(Video());
        StartCoroutine(IUpdate());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator IUpdate()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene(2);
            yield return null;
        }
    }
    private IEnumerator Video()
    {
        m_videoPlayer.Play();
        yield return new WaitForSeconds((float)m_videoPlayer.length);
        m_videoPlayer.Pause();
        SceneManager.LoadScene(2);
    }
}