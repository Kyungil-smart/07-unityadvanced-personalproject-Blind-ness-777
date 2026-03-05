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

    // stageData.spawnInterval마다 적 스폰 시도. 총 스폰 수 초과 시 중단
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(stageData.spawnInterval);

            if (spawnedEnemyCount >= stageData.totalEnemyCount)
            {
                CheckStageClear();
                yield break;
            }

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

        int remaining = stageData.totalEnemyCount - spawnedEnemyCount;
        FindObjectOfType<GameUIManager>()?.UpdateEnemyCount(remaining);
    }

    // StageData의 비율에 따라 랜덤으로 탱크 프리팹 반환
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

    // 모든 적이 스폰되고 전멸 시 스테이지 클리어
    private void CheckStageClear()
    {
        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (spawnedEnemyCount >= stageData.totalEnemyCount && activeEnemies.Count == 0)
            GameManager.Instance?.StageClear();
    }

    // 스폰 위치를 순서대로 순환
    private Vector3 GetNextSpawnPosition()
    {
        if (stageData.spawnPositions == null || stageData.spawnPositions.Length == 0)
            return enemySpawnPoint.position;

        Vector3 pos = stageData.spawnPositions[spawnPositionIndex];
        spawnPositionIndex = (spawnPositionIndex + 1) % stageData.spawnPositions.Length;
        return pos;
    }

    // EnemyTankHealth.Die()에서 호출. 적 사망 시 카운트 갱신 및 클리어 체크
    public void OnEnemyDied()
    {
        activeEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
        int remaining = stageData.totalEnemyCount - spawnedEnemyCount;
        FindObjectOfType<GameUIManager>()?.UpdateEnemyCount(remaining);
        CheckStageClear();
    }

    public Vector3 GetPlayerSpawnPosition()
    {
        if (playerSpawnPoint != null)
            return playerSpawnPoint.position;
        return Vector3.zero;
    }

    // 피격 후 2초 대기 후 스폰 위치에서 부활
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