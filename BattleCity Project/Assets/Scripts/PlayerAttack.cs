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
    
    private int _maxActiveProjectiles;
    private int _activeProjectiles;
    
    private void Awake()
    {
        if (inputManager == null) inputManager = GetComponent<InputManager>();
        
        if (firePointPosition == null) firePointPosition = transform;
        if (firePointDirection == null) firePointDirection = transform;
    }
    
    public void Tick()
    {
        if (Time.time < nextFireTime) return;
        if (!inputManager.ConsumeFireRequested()) return;
        
        Vector3 spawnPosition = firePointPosition.position;
        Quaternion spawnRotation = Quaternion.LookRotation(firePointDirection.forward);
        
        GameObject bulletObject = Instantiate(projectilePrefab, spawnPosition, spawnRotation);
        
        // 자기 자신과 충돌 무시 추가
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
    
    public void SetFireCooldown(float cooldown)
    {
        fireCooldown = cooldown;
    }
}