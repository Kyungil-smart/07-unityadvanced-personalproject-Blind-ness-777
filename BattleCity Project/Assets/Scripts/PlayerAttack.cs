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
        if (HasActiveProjectile()) return;
        if (Time.time < nextFireTime) return;
        if (!inputManager.ConsumeFireRequested()) return;
        
        Vector3 spawnPosition = firePointPosition.position;
        Quaternion spawnRotation = Quaternion.LookRotation(firePointDirection.forward);
        
        GameObject bulletObject = Instantiate(projectilePrefab, spawnPosition, spawnRotation);
        
        Projectile bullet = bulletObject.GetComponent<Projectile>();
        if (bullet != null)
            bullet.Launch(firePointDirection.forward, projectileSpeed);
        
        nextFireTime = Time.time + fireCooldown;
        // _activeProjectiles++;
    }
    
    private bool HasActiveProjectile()
    {
        // TODO: 현재 살아있는 발사체가 있는지 판단
        return false;
    }
}