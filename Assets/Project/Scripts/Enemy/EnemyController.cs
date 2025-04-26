using System;
using UnityEditor.Animations;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private float m_velocity;

    public int LaneNumber;
    public SpriteRenderer enemySprite;
    public Animator animator;

    private void FixedUpdate()
    {
        Move();
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Move()
    {
        transform.Translate(m_velocity * Time.deltaTime * Vector2.left);
    }

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.tag.Contains(gameObject.tag))
        {
            EnemyManager.Instance.EnemyReachLaneEnd();
        }
    }

}
