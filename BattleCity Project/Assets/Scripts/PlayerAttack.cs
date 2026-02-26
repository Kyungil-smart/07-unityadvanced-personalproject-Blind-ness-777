using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    
    [SerializeField] private float fireCooldown = 0.25f;
    [SerializeField] private float projectileSpeed = 10f;
    
    private float nextFireTime;
    private int _maxActiveProjectiles;
    private int _activeProjectiles;
    
    private void Awake()
    {
        if (inputManager == null) inputManager = GetComponent<InputManager>();
    }
    
    public void Tick()
    {
        if (HasActiveProjectile()) return;
        if (Time.time < nextFireTime) return;
        if (!inputManager.ConsumeFireRequested()) return;
        
        GameObject bulletObject = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        
        Projectile bullet = bulletObject.GetComponent<Projectile>();
        if (bullet != null)
            bullet.Launch(firePoint.forward, projectileSpeed);
        
        nextFireTime = Time.time + fireCooldown;
        // _activeProjectiles++;
    }
    
    private bool HasActiveProjectile()
    {
        // TODO: 현재 살아있는 발사체가 있는지 판단
        return false;
    }
}