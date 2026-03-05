using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyBrain brain;
    [SerializeField] private TankData tankData;

    [Header("Fire")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;

    [Header("Optional")]
    [SerializeField] private Collider ownerCollider;

    private float nextFireTime;

    private void Awake()
    {
        if (brain == null) brain = GetComponent<EnemyBrain>();
        if (ownerCollider == null) ownerCollider = GetComponent<Collider>();
    }

    // EnemyController에서 매 프레임 호출. Brain이 발사 요청 시에만 실행
    public void Tick()
    {
        if (projectilePrefab == null || firePoint == null) return;

        float cooldown = tankData != null ? tankData.fireCooldown : 3.0f;
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + cooldown;

        Vector3 spawnPos = firePoint.position + firePoint.forward * 0.4f;
        GameObject bullet = Instantiate(projectilePrefab, spawnPos, firePoint.rotation);

        // 발사한 탱크 자신과의 충돌 무시
        if (ownerCollider != null)
        {
            Collider bulletCollider = bullet.GetComponent<Collider>();
            if (bulletCollider != null)
                Physics.IgnoreCollision(ownerCollider, bulletCollider, true);
        }

        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
            projectile.Launch(firePoint.forward, projectileSpeed, gameObject.layer);
    }
}