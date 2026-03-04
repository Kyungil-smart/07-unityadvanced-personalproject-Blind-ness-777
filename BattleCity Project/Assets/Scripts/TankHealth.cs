using UnityEngine;

public class TankHealth : MonoBehaviour, IProjectileHittable
{
    [SerializeField] private int maxHp = 1;
    private int currentHp;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        TakeHit(1);
    }

    public void TakeHit(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}