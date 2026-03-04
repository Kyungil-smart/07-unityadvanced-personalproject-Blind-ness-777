using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private StageData stageData;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private GameObject playerPrefab;

    private int spawnedEnemyCount = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private int spawnPositionIndex = 0;

    private void Start()
    {
        SpawnPlayer();
        StartCoroutine(SpawnRoutine());
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null || playerSpawnPoint == null) return;
        Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(stageData.spawnInterval);

            // 총 스폰 수 초과 시 중단
            if (spawnedEnemyCount >= stageData.totalEnemyCount)
            {
                CheckStageClear();
                yield break;
            }

            // 죽은 적 정리
            activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

            // 동시 활성화 수 미만일 때만 스폰
            while (activeEnemies.Count < stageData.maxActiveEnemies
                   && spawnedEnemyCount < stageData.totalEnemyCount)
            {
                SpawnEnemy();
            }

            CheckStageClear();
        }
    }

    private void SpawnEnemy()
    {
        GameObject prefab = GetRandomPrefab();
        if (prefab == null) return;

        Vector3 spawnPos = GetNextSpawnPosition();
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        activeEnemies.Add(enemy);
        spawnedEnemyCount++;
        
        // 남은 적 수 = 총 적 수 - 현재까지 스폰된 수
        int remaining = stageData.totalEnemyCount - spawnedEnemyCount;
        GameUIManager.Instance?.UpdateEnemyCount(remaining);
    }

    private GameObject GetRandomPrefab()
    {
        int roll = Random.Range(0, 100);

        if (roll < stageData.basicRatio)
            return stageData.basicPrefab;
        else if (roll < stageData.basicRatio + stageData.mediumRatio)
            return stageData.mediumPrefab;
        else
            return stageData.heavyPrefab;
    }
    
    private void CheckStageClear()
    {
        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (spawnedEnemyCount >= stageData.totalEnemyCount && activeEnemies.Count == 0)
            GameManager.Instance?.StageClear();
    }
    
    private Vector3 GetNextSpawnPosition()
    {
        if (stageData.spawnPositions == null || stageData.spawnPositions.Length == 0)
            return enemySpawnPoint.position;

        Vector3 pos = stageData.spawnPositions[spawnPositionIndex];
        spawnPositionIndex = (spawnPositionIndex + 1) % stageData.spawnPositions.Length;
        return pos;
    }
    
    public void OnEnemyDied()
    {
        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
        CheckStageClear();
    }
    
    public Vector3 GetPlayerSpawnPosition()
    {
        if (playerSpawnPoint != null)
            return playerSpawnPoint.position;
        return Vector3.zero;
    }
    
    public void StartRespawn(GameObject player)
    {
        StartCoroutine(RespawnRoutine(player));
    }

    private IEnumerator RespawnRoutine(GameObject player)
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2f);
        player.transform.position = playerSpawnPoint.position;
        player.SetActive(true);
    }
}