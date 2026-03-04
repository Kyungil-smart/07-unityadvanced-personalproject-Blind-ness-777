using UnityEngine;

public class Base : MonoBehaviour, IProjectileHittable
{
    [Header("State")]
    [SerializeField] private bool isDestroyed;

    [Header("Optional Visual")]
    [SerializeField] private GameObject baseVisual;

    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        if (isDestroyed) return;

        isDestroyed = true;
        
        // 1) 비주얼만 끄고 싶으면 baseVisual 사용
        if (baseVisual != null)
        {
            baseVisual.SetActive(false);
        }
        else
        {
            // 2) 비주얼이 따로 없으면 베이스 자체 비활성화
            gameObject.SetActive(false);
        }

        Debug.Log("GAME OVER: Base destroyed");
        GameManager.Instance?.GameOver();
    }
}