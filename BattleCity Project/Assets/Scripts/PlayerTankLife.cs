using UnityEngine;

public class PlayerTankLife : MonoBehaviour, IProjectileHittable
{
    [SerializeField] private PlayerTankUpgrade tankUpgrade;

    private void Awake()
    {
        if (tankUpgrade == null) tankUpgrade = GetComponent<PlayerTankUpgrade>();
    }

    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        TakeHit();
    }

    // 피격 시 강화 초기화, 목숨 차감, UI 갱신, 리스폰 요청
    private void TakeHit()
    {
        AudioManager.Instance?.PlayTankExplosion();
        tankUpgrade?.OnDestroyed();
        GameManager.Instance?.LoseLife();
        FindObjectOfType<GameUIManager>()?.UpdateLives();
        FindObjectOfType<SpawnManager>()?.StartRespawn(gameObject);
    }
}