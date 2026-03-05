using UnityEngine;

// 총알 피격 처리가 필요한 오브젝트가 구현하는 인터페이스
public interface IProjectileHittable
{
    void OnProjectileHit(Projectile projectile, RaycastHit hit);
}