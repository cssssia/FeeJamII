using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public float velocity;

    public int LaneNumber;
    public SpriteRenderer enemySprite;
    public Animator animator;
    public EnemyType type;

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
        transform.Translate(velocity * Time.fixedDeltaTime * Vector2.left);
    }

    public EnemyController Setup(int p_laneNumber, Sprite p_sprite, RuntimeAnimatorController p_animatorController,
        string p_enemy, EnemyType p_type)
    {
        enemySprite.sprite = p_sprite;
        animator.runtimeAnimatorController = p_animatorController;
        LaneNumber = p_laneNumber;
        tag = p_enemy;
        name = p_enemy;
        type = p_type;
        velocity = Mathf.Min(2, EnemyManager.Instance.CurrentEnemySpeed);

        if (p_type == EnemyType.GREEN && LaneNumber == 0) enemySprite.flipX = true;

        return this;
    }
    public void Died()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D p_collider)
    {
        if (p_collider.tag.Contains(gameObject.tag))
        {
            EnemyManager.Instance.EnemyReachLaneEnd();
        }
        else if (!p_collider.CompareTag("Background"))
        {
            SoundManager.Instance?.PlaySound(AudioID.ENEMY_DEATH);
            EnemyManager.Instance.EnemyDied(this);
            // Destroy(gameObject);
        }
    }
}

public enum EnemyType
{
    NONE,
    BLUE,
    GREEN,
    ORANGE,
}