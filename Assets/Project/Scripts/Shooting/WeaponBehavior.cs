using System;
using System.Collections;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    public Action OnShoot;
    public Action OnDoubleShoot;
    public Action OnLaser;

    [SerializeField] private Transform m_weaponTransform;
    [SerializeField] private GameObject m_shot;

    void Start()
    {
        InputManager.Instance.OnDoubleClickAction += DoubleShoot;
    }

    private void OnEnable()
    {
        StartCoroutine(IEUpdate());
    }

    private IEnumerator IEUpdate()
    {
        while (true)
        {
            yield return null;
            if (InputManager.Instance.ClickPressedThisFrame)
            {
                //sendo triggerado no double click.... duas vezes,.,.,.,
                m_shot.GetComponent<ShootController>().gameObject.tag = "Shoot";
                Instantiate(m_shot, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos), Quaternion.identity);
            }
            else if (InputManager.Instance.ClickPressed)
            {
                // disparando sempre
                // m_shot.GetComponent<ShootController>().gameObject.tag = "Laser";
                // Instantiate(m_shot, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos), Quaternion.identity);
            }
            m_weaponTransform.LookAt(Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos));
        }
    }

    public void Shoot()
    {
        OnShoot?.Invoke();
    }

    public void DoubleShoot()
    {
        m_shot.GetComponent<ShootController>().gameObject.tag = "DoubleShoot";
        Instantiate(m_shot, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos), Quaternion.identity);
    }

    public void Laser()
    {
        OnLaser?.Invoke();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
