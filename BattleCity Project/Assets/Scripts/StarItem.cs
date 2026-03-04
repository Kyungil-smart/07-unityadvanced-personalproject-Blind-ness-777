using UnityEngine;

public class StarItem : MonoBehaviour
{
    [SerializeField] private int scoreValue = 500;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[StarItem] OnTriggerEnter: {other.gameObject.name}");
    
        // GetComponent 대신 GetComponentInParent로 변경
        PlayerTankUpgrade upgrade = other.GetComponentInParent<PlayerTankUpgrade>();
        if (upgrade == null)
        {
            Debug.Log("[StarItem] PlayerTankUpgrade 없음");
            return;
        }
        
        GameManager.Instance?.AddScore(scoreValue);
        upgrade.OnStarCollected();
    
        ItemManager.Instance?.OnItemCollected();
        gameObject.SetActive(false);
    }
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     Debug.Log($"[StarItem] OnCollisionEnter: {other.gameObject.name}");
    // }
}