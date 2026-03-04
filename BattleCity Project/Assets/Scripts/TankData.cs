using UnityEngine;

[CreateAssetMenu(fileName = "TankData", menuName = "BattleCity/TankData")]
public class TankData : ScriptableObject
{
    public int maxHp = 1;
    public int scoreValue = 100;
    public float fireCooldown = 3.0f;
}