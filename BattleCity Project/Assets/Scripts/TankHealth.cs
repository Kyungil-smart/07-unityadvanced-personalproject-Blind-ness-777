using UnityEngine;

public class TankHealth : MonoBehaviour, IProjectileHittable
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

    private void Die()
    {
        GameManager.Instance?.AddScore(tankData != null ? tankData.scoreValue : 0);
        gameObject.SetActive(false);
    }
}