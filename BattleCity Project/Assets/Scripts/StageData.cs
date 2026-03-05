using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "BattleCity/StageData")]
public class StageData : ScriptableObject
{
    [Header("스테이지 정보")]
    public int totalEnemyCount;
    public int maxActiveEnemies;
    public float spawnInterval;

    [Header("스폰 위치")]
    public Vector3[] spawnPositions;

    // 합계가 100이 되어야 GetRandomPrefab()이 정상 동작
    [Header("탱크 등장 비율 (합계 100)")]
    [Range(0, 100)] public int basicRatio = 100;
    [Range(0, 100)] public int mediumRatio = 0;
    [Range(0, 100)] public int heavyRatio = 0;

    [Header("탱크 프리팹")]
    public GameObject basicPrefab;
    public GameObject mediumPrefab;
    public GameObject heavyPrefab;
}