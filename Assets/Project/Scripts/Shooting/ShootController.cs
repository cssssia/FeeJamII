using UnityEngine;

public class ShootController : MonoBehaviour
{
    public bool shouldDestroyOnShoot = true;

    private void KillEnemy(GameObject p_gameObject)
    {
        int l_penality = 0;

        if (!p_gameObject.tag.Contains(p_gameObject.GetComponent<EnemyController>().LaneNumber.ToString()))
        {
            l_penality = PointsController.Instance.Penality();
        }

        if (l_penality < 3)
        {
            EnemyManager.Instance.EnemyDied(p_gameObject.GetComponent<EnemyController>());
            Destroy(p_gameObject);
            if (shouldDestroyOnShoot) Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D p_collision)
    {
        if (gameObject.CompareTag("Shoot") && p_collision.gameObject.CompareTag("Enemy0"))
        {
            KillEnemy(p_collision.gameObject);
        }
        else if (gameObject.CompareTag("DoubleShoot") && p_collision.gameObject.CompareTag("Enemy2"))
        {
            KillEnemy(p_collision.gameObject);
        }
        else if (gameObject.CompareTag("Laser") && p_collision.gameObject.CompareTag("Enemy1"))
        {
            KillEnemy(p_collision.gameObject);
        }
        else if (shouldDestroyOnShoot)
        {
            Destroy(gameObject);
        }
    }
}