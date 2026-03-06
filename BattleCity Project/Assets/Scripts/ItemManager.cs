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
        DontDestroyOnLoad(gameObject);
    }

    // 적 사망 시 확률적으로 별 아이템 스폰. 필드에 아이템이 이미 있으면 제거 후 재생성
    public void TrySpawnItem(Vector3 position)
    {
        if (Random.value >= itemSpawnChance) return;

        if (currentItem != null && currentItem.activeInHierarchy)
            currentItem.SetActive(false);

        if (starItemPrefab == null) return;
        currentItem = Instantiate(starItemPrefab, position, Quaternion.identity);
    }

    // StarItem에서 획득 시 호출
    public void OnItemCollected()
    {
        currentItem = null;
    }
}