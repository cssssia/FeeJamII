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

    public EnemyController Setup(int p_laneNumber, Sprite p_sprite, RuntimeAnimatorController p_animatorController, string p_enemy)
    {

        enemySprite.sprite = p_sprite;
        animator.runtimeAnimatorController = p_animatorController;
        LaneNumber = p_laneNumber;
        tag = p_enemy;
        name = p_enemy;

        return this;
    }

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.tag.Contains(gameObject.tag))
        {
            EnemyManager.Instance.EnemyReachLaneEnd();
        }
        else if (!p_collider.CompareTag("Background"))
        {
            EnemyManager.Instance.EnemyDied(this);
            Destroy(gameObject);
        }
    }

}
