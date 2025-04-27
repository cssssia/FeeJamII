using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public enum AudioID
{
    NONE,
    ENEMY_DEATH,
    PLAYER_DAMAGED,
    UI_CLICK,
    SHOT_1,
    SHOT_2,
}

[Serializable]
public class SoundReference
{
    public EventReference reference;
    public AudioID id;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<SoundReference> m_sounds;

    [SerializeField] private StudioEventEmitter m_menuHover;
    [SerializeField] private StudioEventEmitter m_menuAmbience;
    [SerializeField] private StudioEventEmitter m_gameMusic;

    public void PlaySound(AudioID p_id)
    {
        EventReference l_ref = GetEventReference(p_id);
        RuntimeManager.PlayOneShot(l_ref);
    }

    public void SetMenuAmbience(bool p_state)
    {
        if (p_state) m_menuAmbience.Play();
        else m_menuAmbience.Stop();
    }

    public void SetGameMusic(bool p_state)
    {
        if (p_state) m_gameMusic.Play();
        else m_gameMusic.Stop();
    }

    public void PlayButtonHover() => m_menuHover.Play();
    public void StopButtonHover() => m_menuHover.Stop();

    private EventReference GetEventReference(AudioID p_id)
    {
        for (int i = 0; i < m_sounds.Count; i++)
        {
            if (m_sounds[i].id != p_id) continue;
            Debug.Log("foundSound " + p_id.ToString());
            return m_sounds[i].reference;
        }

        return new EventReference();
    }

    #region Debug

    [Header("Debug"), SerializeField] private bool m_debug;

    [FormerlySerializedAs("m_eventID")] [SerializeField, ShowIf("m_debug")]
    private AudioID m_debugAudioID;

    [Button, ShowIf("m_debug")]
    private void DebugPlaySound() => PlaySound(m_debugAudioID);

    #endregion

    #region Other

    public System.Action OnLoadBanks;

    [HorizontalLine, Header("FMOD Debug"), BankRef, ReadOnly]
    public List<string> Banks = new List<string>();

    private void Start()
    {
#if UNITY_WEBGL
        if (!RuntimeManager.HaveAllBanksLoaded)
        {
            for (int i = 0; i < Banks.Count; i++)
            {
                RuntimeManager.LoadBank(Banks[i], true);
            }
        }
#endif
    }

    #endregion
}