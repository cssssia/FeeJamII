using System.Collections;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [SerializeField] private Transform m_weaponTransform;
    [SerializeField] private GameObject m_shot;
    
    private void OnEnable()
    {
        StartCoroutine(IEUpdate());
    }

    private IEnumerator IEUpdate()
    {
        while (true)
        {
            yield return null;
            if (InputManager.Instance.ClickPressedThisFrame) Instantiate(m_shot);
            m_weaponTransform.LookAt(Camera.main.ScreenToWorldPoint(InputManager.Instance.MousePos));
        }
    }

    public void Shoot()
    {
        
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
