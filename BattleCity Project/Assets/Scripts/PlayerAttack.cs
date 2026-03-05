using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject projectilePrefab;

    [Header("Fire Point")]
    [SerializeField] private Transform firePointPosition;
    [SerializeField] private Transform firePointDirection;

    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float projectileSpeed = 10f;

    private float nextFireTime;

    private void Awake()
    {
        if (inputManager == null) inputManager = GetComponent<InputManager>();
        if (firePointPosition == null) firePointPosition = transform;
        if (firePointDirection == null) firePointDirection = transform;
    }

    // PlayerController에서 매 프레임 호출. 입력 소비 후 총알 생성
    public void Tick()
    {
        if (Time.time < nextFireTime) return;
        if (!inputManager.ConsumeFireRequested()) return;

        Vector3 spawnPosition = firePointPosition.position;
        Quaternion spawnRotation = Quaternion.LookRotation(firePointDirection.forward);

        GameObject bulletObject = Instantiate(projectilePrefab, spawnPosition, spawnRotation);
        AudioManager.Instance?.PlayShotFiring();

        // 발사한 탱크 자신과의 충돌 무시
        Collider ownerCollider = GetComponent<Collider>();
        if (ownerCollider != null)
        {
            Collider bulletCollider = bulletObject.GetComponent<Collider>();
            if (bulletCollider != null)
                Physics.IgnoreCollision(ownerCollider, bulletCollider, true);
        }

        Projectile bullet = bulletObject.GetComponent<Projectile>();
        if (bullet != null)
            bullet.Launch(firePointDirection.forward, projectileSpeed, gameObject.layer);

        nextFireTime = Time.time + fireCooldown;
    }

    // PlayerTankUpgrade에서 강화 단계별 쿨다운 변경 시 호출
    public void SetFireCooldown(float cooldown)
    {
        fireCooldown = cooldown;
    }
}