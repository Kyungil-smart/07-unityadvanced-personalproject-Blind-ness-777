using UnityEngine;

public enum TileType
{
    Breakable,
    Unbreakable
}

public class Tiles : MonoBehaviour, IProjectileHittable
{
    [SerializeField] private TileType tileType;
    
    public void OnProjectileHit(Projectile projectile, RaycastHit hit)
    {
        switch (tileType)
        {
            case TileType.Breakable:
                gameObject.SetActive(false);
                break;
            case TileType.Unbreakable:
                break;
        }
    }
}
