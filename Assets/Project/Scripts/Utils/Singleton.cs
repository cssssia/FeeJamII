using UnityEngine;
using UnityEngine.Serialization;
using JetBrains.Annotations;
using System;

public abstract class Singleton<T> : Singleton where T : MonoBehaviour
{
    #region Fields

    [CanBeNull] private static T m_instance;

    [NotNull]
    // ReSharper disable once StaticMemberInGenericType
    private static readonly object LOCK = new object();

    static bool m_thereIsNoInstance = false;

    [FormerlySerializedAs("_persistent")] [SerializeField]
    private bool m_persistent = false;

    #endregion

    #region Properties

    [NotNull]
    public static T Instance
    {
        get
        {
            if (QUITTING)
            {
                //Debug.LogWarning("[Singleton <" + typeof(T) + ">] Instance will not be returned because the application is quitting.");
                // ReSharper disable once AssignNullToNotNullAttribute
                return null;
            }

            lock (LOCK)
            {
                if (m_instance != null) return m_instance;
                if (m_thereIsNoInstance) return null;

                var instances = FindObjectsByType<T>(FindObjectsSortMode.None);
                var count = instances.Length;
                if (count > 0)
                {
                    if (count == 1)
                        return m_instance = instances[0];
                    Debug.LogWarning("[Singleton < " + typeof(T) +
                                     ">] There should never be more than one {nameof(Singleton)} of type {typeof(T)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                        Destroy(instances[i]);
                    return m_instance = instances[0];
                }

                m_thereIsNoInstance = true;
                //Debug.Log("[Singleton < " + typeof(T) + ">] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                //return _instance = new GameObject("[MAN] " + typeof(T)).AddComponent<T>();
                return null;
            }
        }
    }

    #endregion

    #region Methods

    private void Awake()
    {
        if (m_instance != null
            && m_instance.gameObject != gameObject)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            QUITTING = false;
            m_instance = this as T;
            m_thereIsNoInstance = false;
        }

        if (m_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnDestroy()
    {
    }

    protected virtual void CheckManagerSpawned(Type p_managerType, Singleton p_class)
    {
    }

    protected virtual void CheckManagerDespawned(Type p_managerType)
    {
    }

    #endregion
}

public abstract class Singleton : MonoBehaviour
{
    #region Properties

    public static bool QUITTING { get; protected set; }

    #endregion

    #region Methods

    private void OnApplicationQuit()
    {
        QUITTING = true;
    }

    #endregion
}