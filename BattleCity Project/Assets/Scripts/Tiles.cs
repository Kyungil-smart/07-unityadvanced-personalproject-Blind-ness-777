using UnityEngine;

public enum TileType
{
    Breakable,
    Unbreakable
}

public class Tiles : MonoBehaviour, IProjectileHittable
{
    [SerializeField] private TileType tileType;

    // Breakable이면 피격 시 비활성화, Unbreakable이면 무시
    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        if (tileType == TileType.Breakable)
            gameObject.SetActive(false);
    }

    public bool IsBreakable() => tileType == TileType.Breakable;
}