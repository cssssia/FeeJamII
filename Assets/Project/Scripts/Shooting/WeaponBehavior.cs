using System;
using System.Collections;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public Action OnShoot;
    public Action OnDoubleShoot;

    [SerializeField] private Transform m_weaponTransform;
    [SerializeField] private GameObject m_shot;
    [SerializeField] private GameObject m_laserObject;
    [SerializeField] private GameObject m_laserVFXObject;
    [SerializeField] private GameObject m_laserFeixeObject;
    [SerializeField] private Animator m_laserBaseExplosion;
    [SerializeField] private Animator m_laserTargetExplosion;

    // [SerializeField] private AnimatorController m_laserBaseAnim;
    // [SerializeField] private AnimatorController m_laserTargetAnim;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // m_laserBaseAnim = m_laserBaseExplosion.GetComponent<AnimatorController>();
        // m_laserTargetAnim = m_laserTargetExplosion.GetComponent<AnimatorController>();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnPerformHold += OnPerformHold;
        InputManager.Instance.OnReleaseHold += OnReleaseHold;
        m_laserObject.SetActive(false);
        m_laserVFXObject.SetActive(false);
        StartCoroutine(IEUpdate());
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPerformHold -= OnPerformHold;
        InputManager.Instance.OnReleaseHold -= OnReleaseHold;

        StopAllCoroutines();
    }

    private bool m_isLaserActive;

    private void OnPerformHold()
    {
        m_isLaserActive = true;
        m_laserObject.SetActive(m_isLaserActive);
        m_laserVFXObject.SetActive(m_isLaserActive);
        StartCoroutine(PlayLaserAnims());
    }

    private float m_delayBetweenLaserExplosions = .1f;

    private IEnumerator PlayLaserAnims()
    {
        m_laserBaseExplosion.Play("LaserBase");
        // m_laserBaseExplosion.Play("LaserBase");
        Vector2 newPos = new Vector2(Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos).x, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos).y);
        m_laserTargetExplosion.gameObject.transform.position = newPos;
        yield return new WaitForSeconds(m_delayBetweenLaserExplosions);
        m_laserTargetExplosion.Play("LaserTarget");
    }

    private void OnReleaseHold()
    {
        m_isLaserActive = false;
        m_laserObject.SetActive(m_isLaserActive);
        m_laserVFXObject.SetActive(m_isLaserActive);
    }

    private Vector2 m_mousePos;
    private Vector2 m_lookDir;

    private IEnumerator IEUpdate()
    {
        while (true)
        {
            yield return null;

            if (InputManager.Instance.DoubleClickPressedThisFrame)
            {
                DoubleShoot();
            }
            else if (InputManager.Instance.ClickPressedThisFrame)
            {
                NormalShoot();
            }
            else if (InputManager.Instance.ClickHeld)
            {
                UpdateLaserPositionAndVFXSize();
            }

            Vector3 l_position = transform.position;
            m_mousePos = Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos);
            m_lookDir = m_mousePos - new Vector2(l_position.x, l_position.y);

            float l_angle = Mathf.Atan2(m_lookDir.y, m_lookDir.x) * Mathf.Rad2Deg - 90f;

            m_weaponTransform.rotation = Quaternion.Euler(0f, 0f, l_angle);
        }
    }

    public void NormalShoot()
    {
        OnShoot?.Invoke();
        GameObject shot = Instantiate(m_shot,
            Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos),
            Quaternion.identity);
        shot.transform.position = new Vector3(shot.transform.position.x, shot.transform.position.y, 0);
        shot.tag = "Shoot";
        m_laserBaseExplosion.Play("ShootBase");
        Vector2 newPos = new Vector2(Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos).x, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos).y);
        m_laserTargetExplosion.gameObject.transform.position = newPos;
        m_laserTargetExplosion.Play("ShootTarget");
    }

    public void DoubleShoot()
    {
        GameObject shot = Instantiate(m_shot, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos),
            Quaternion.identity);
        shot.transform.position = new Vector3(shot.transform.position.x, shot.transform.position.y, 0);
        shot.tag = "DoubleShoot";
        m_laserBaseExplosion.Play("DoubleShootBase");
        Vector2 newPos = new Vector2(Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos).x, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos).y);
        m_laserTargetExplosion.gameObject.transform.position = newPos;
        m_laserTargetExplosion.Play("DoubleShootTarget");
        OnDoubleShoot?.Invoke();
    }

    private void UpdateLaserPositionAndVFXSize()
    {
        m_laserObject.transform.position = Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos);
        m_laserObject.transform.position =
            new Vector3(m_laserObject.transform.position.x,
                m_laserObject.transform.position.y, 0);

        float distance = Vector2.Distance(transform.position, m_laserObject.transform.position);
        m_laserFeixeObject.transform.localScale = new Vector3(m_laserFeixeObject.transform.localScale.x, distance,
            m_laserFeixeObject.transform.localScale.z);
    }
}