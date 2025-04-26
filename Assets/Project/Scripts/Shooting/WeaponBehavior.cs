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

    private Vector2 m_mousePos;
    private Vector2 m_lookDir;
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
                // disparando sempre q clica
                // spawnando muitos ao inves de 1 só
                // m_shot.GetComponent<ShootController>().gameObject.tag = "Laser";
                // Instantiate(m_shot, Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos), Quaternion.identity);
            }
            
            
            Vector3 l_position = transform.position;
            m_mousePos = Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos);
            m_lookDir = m_mousePos - new Vector2(l_position.x, l_position.y);
            
            float l_angle = Mathf.Atan2(m_lookDir.y, m_lookDir.x) * Mathf.Rad2Deg - 90f;

            m_weaponTransform.rotation = Quaternion.Euler(0f, 0f, l_angle);
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
