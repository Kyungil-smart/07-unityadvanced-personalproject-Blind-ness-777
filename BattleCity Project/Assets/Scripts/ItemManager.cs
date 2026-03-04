using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private GameObject starItemPrefab;
    [SerializeField] private float itemSpawnChance = 0.15f;

    private GameObject currentItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void TrySpawnItem(Vector3 position)
    {
        if (Random.value >= itemSpawnChance) return;

        if (currentItem != null && currentItem.activeInHierarchy)
            currentItem.SetActive(false);

        if (starItemPrefab == null) return;
        currentItem = Instantiate(starItemPrefab, position, Quaternion.identity);
    }

    public void OnItemCollected()
    {
        currentItem = null;
    }
}