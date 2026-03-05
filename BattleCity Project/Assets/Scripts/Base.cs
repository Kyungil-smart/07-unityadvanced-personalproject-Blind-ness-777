using UnityEngine;

public class Base : MonoBehaviour, IProjectileHittable
{
    [Header("State")]
    [SerializeField] private bool isDestroyed;

    [Header("Optional Visual")]
    [SerializeField] private GameObject baseVisual;

    // 기지가 피격되면 즉시 게임오버
    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        if (isDestroyed) return;

        isDestroyed = true;

        // baseVisual이 있으면 비주얼만 끄고, 없으면 오브젝트 자체 비활성화
        if (baseVisual != null)
            baseVisual.SetActive(false);
        else
            gameObject.SetActive(false);

        GameManager.Instance?.GameOver();
    }
}