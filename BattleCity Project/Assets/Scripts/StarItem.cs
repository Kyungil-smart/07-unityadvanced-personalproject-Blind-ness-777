using UnityEngine;

public class StarItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 500;

    // 플레이어 탱크와 충돌 시 점수 추가, 강화 적용, 아이템 제거
    private void OnTriggerEnter(Collider other)
    {
        PlayerTankUpgrade upgrade = other.GetComponentInParent<PlayerTankUpgrade>();
        if (upgrade == null) return;

        AudioManager.Instance?.PlayPickupPowerUp();
        GameManager.Instance?.AddScore(scoreValue);
        upgrade.OnStarCollected();
        ItemManager.Instance?.OnItemCollected();
        gameObject.SetActive(false);
    }
}