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
}

[Serializable]
public class SoundReference
{
    public StudioEventEmitter emitter;
    public AudioID id;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<SoundReference> m_sounds;

    public void PlaySound(AudioID p_id)
    {
        StudioEventEmitter abc = GetEventReference(p_id);
        abc.Play();
    }

    private StudioEventEmitter GetEventReference(AudioID p_id)
    {
        for (int i = 0; i < m_sounds.Count; i++)
        {
            if (m_sounds[i].id != p_id) continue;
            Debug.Log("foundSound " + p_id.ToString());
            return m_sounds[i].emitter;
        }

        return null;
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