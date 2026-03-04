using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyBrain brain;

    [Header("Fire")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float fireCooldown = 0.6f;

    [Header("Optional")]
    [SerializeField] private Collider ownerCollider; // 탱크 본체 콜라이더(있으면 총알과 충돌 무시)

    private float nextFireTime;

    private void Awake()
    {
        if (brain == null) brain = GetComponent<EnemyBrain>();
        if (ownerCollider == null) ownerCollider = GetComponent<Collider>();
    }

    public void Tick()
    {
        if (projectilePrefab == null) return;
        if (firePoint == null) return;

        // 쿨다운
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireCooldown;

        // 생성
        Vector3 spawnPos = firePoint.position + firePoint.forward * 0.4f;
        GameObject bullet = Instantiate(projectilePrefab, spawnPos, firePoint.rotation);

        // 자기 자신과 충돌 무시(필요 시)
        if (ownerCollider != null)
        {
            Collider bulletCollider = bullet.GetComponent<Collider>();
            if (bulletCollider != null)
            {
                Physics.IgnoreCollision(ownerCollider, bulletCollider, true);
            }
        }

        // 발사 방향: 현재 바라보는 방향
        // (EnemyMovement가 rb.MoveRotation으로 방향을 만들고 있으니 transform.forward가 맞음)
        Projectile projectile = bullet.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Launch(firePoint.forward, projectileSpeed);
        }
    }
}