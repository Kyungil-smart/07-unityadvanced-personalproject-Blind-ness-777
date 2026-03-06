using UnityEngine;

public class EnemyTankHealth : MonoBehaviour, IProjectileHittable
{
    [SerializeField] private TankData tankData;
    private int currentHp;

    private void Awake()
    {
        currentHp = tankData != null ? tankData.maxHp : 1;
    }

    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        TakeHit(1);
    }

    public void TakeHit(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
            Die();
    }

    // 사망 시 점수 추가, 아이템 드랍 시도, 비활성화 후 SpawnManager에 통보
    private void Die()
    {
        Debug.Log($"[Die] ItemManager.Instance={ItemManager.Instance}");
        GameManager.Instance?.AddScore(tankData != null ? tankData.scoreValue : 0);
        ItemManager.Instance?.TrySpawnItem(transform.position);
        AudioManager.Instance?.PlayShellExplosion();
        gameObject.SetActive(false);
        FindObjectOfType<SpawnManager>()?.OnEnemyDied();
    }
}