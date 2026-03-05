using UnityEngine;
using System.Collections;

public class PlayerTankLife : MonoBehaviour, IProjectileHittable
{
    [SerializeField] private PlayerTankUpgrade tankUpgrade;
    [SerializeField] private float respawnDelay = 2f;

    private void Awake()
    {
        if (tankUpgrade == null) tankUpgrade = GetComponent<PlayerTankUpgrade>();
    }

    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        TakeHit();
    }

    private void TakeHit()
    {
        AudioManager.Instance?.PlayTankExplosion();
        tankUpgrade?.OnDestroyed();
        GameManager.Instance?.LoseLife();
        FindObjectOfType<GameUIManager>()?.UpdateLives();
        FindObjectOfType<SpawnManager>()?.StartRespawn(gameObject);
    }
}