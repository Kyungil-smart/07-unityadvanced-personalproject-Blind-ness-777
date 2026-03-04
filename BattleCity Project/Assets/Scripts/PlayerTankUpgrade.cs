using UnityEngine;

public class PlayerTankUpgrade : MonoBehaviour
{
    [Header("강화 단계별 색상")]
    [SerializeField] private Color stage1Color = Color.blue;
    [SerializeField] private Color stage2Color = new Color(0.5f, 0.8f, 1f); // 하늘색
    [SerializeField] private Color stage3Color = new Color(1f, 0.9f, 0f);   // 노란색
    [SerializeField] private Color superColor = new Color(1f, 0.7f, 0f);    // 황금색

    [Header("강화 단계별 발사 쿨다운")]
    [SerializeField] private float stage1FireCooldown = 1.0f;
    [SerializeField] private float stage2FireCooldown = 0.8f;
    [SerializeField] private float stage3FireCooldown = 0.6f;
    [SerializeField] private float superFireCooldown = 0.4f;

    private int upgradeLevel = 0; // 0=1단계, 1=2단계, 2=3단계, 3=슈퍼
    private int starCount = 0;    // 파괴 없이 연속으로 먹은 별 수

    private MeshRenderer[] renderers;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        if (playerAttack == null) playerAttack = GetComponent<PlayerAttack>();
        
        ApplyUpgrade();
    }

    public void OnStarCollected()
    {
        if (upgradeLevel >= 3) return; // 슈퍼탱크면 변화 없음

        starCount++;

        if (starCount >= 3)
        {
            upgradeLevel = 3; // 슈퍼탱크
        }
        else
        {
            upgradeLevel = Mathf.Min(upgradeLevel + 1, 3);
        }

        ApplyUpgrade();
    }

    public void OnDestroyed()
    {
        // 파괴 시 1단계로 초기화
        upgradeLevel = 0;
        starCount = 0;
        ApplyUpgrade();
    }

    private void ApplyUpgrade()
    {
        Color color = upgradeLevel switch
        {
            0 => stage1Color,
            1 => stage2Color,
            2 => stage3Color,
            3 => superColor,
            _ => stage1Color
        };

        float cooldown = upgradeLevel switch
        {
            0 => stage1FireCooldown,
            1 => stage2FireCooldown,
            2 => stage3FireCooldown,
            3 => superFireCooldown,
            _ => stage1FireCooldown
        };

        // 색상 적용
        foreach (var r in renderers)
        {
            r.material.color = color;
        }

        // 발사 쿨다운 적용
        if (playerAttack != null)
            playerAttack.SetFireCooldown(cooldown);
    }
}