using UnityEngine;

public class StarItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 500;

    private void OnTriggerEnter(Collider other)
    {
        PlayerTankUpgrade upgrade = other.GetComponent<PlayerTankUpgrade>();
        if (upgrade == null) return;

        GameManager.Instance?.AddScore(scoreValue);
        upgrade.OnStarCollected();

        ItemManager.Instance?.OnItemCollected();
        gameObject.SetActive(false);
    }
}