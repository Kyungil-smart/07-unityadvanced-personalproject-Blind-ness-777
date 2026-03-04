using UnityEngine;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [SerializeField] private GameObject starItemPrefab;
    [SerializeField] private List<Vector3> spawnPositions;
    [SerializeField] private float itemSpawnChance = 0.15f; // 15% 확률

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

    public void SpawnItem()
    {
        // 이미 아이템이 필드에 있으면 제거
        if (currentItem != null && currentItem.activeInHierarchy)
            currentItem.SetActive(false);

        if (starItemPrefab == null) return;
        if (spawnPositions == null || spawnPositions.Count == 0) return;

        Vector3 spawnPos = spawnPositions[Random.Range(0, spawnPositions.Count)];
        currentItem = Instantiate(starItemPrefab, spawnPos, Quaternion.identity);
    }

    public void OnItemCollected()
    {
        currentItem = null;
    }
    
    public void TrySpawnItem()
    {
        if (Random.value < itemSpawnChance)
            SpawnItem();
    }
}