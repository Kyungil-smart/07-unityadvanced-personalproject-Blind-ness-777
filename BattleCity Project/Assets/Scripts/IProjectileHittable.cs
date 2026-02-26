using UnityEngine;

public interface IProjectileHittable
{
    public void OnProjectileHit(Projectile projectile, RaycastHit hit) {}
}
