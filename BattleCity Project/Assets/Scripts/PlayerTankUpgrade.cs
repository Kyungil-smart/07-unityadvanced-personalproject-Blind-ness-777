using UnityEngine;

public class PlayerTankUpgrade : MonoBehaviour
{
    [Header("강화 단계별 색상")]
    [SerializeField] private Color stage1Color = Color.blue;
    [SerializeField] private Color stage2Color = new Color(0.5f, 0.8f, 1f);
    [SerializeField] private Color stage3Color = new Color(1f, 0.9f, 0f);
    [SerializeField] private Color superColor = new Color(1f, 0.7f, 0f);

    [Header("강화 단계별 발사 쿨다운")]
    [SerializeField] private float stage1FireCooldown = 1.0f;
    [SerializeField] private float stage2FireCooldown = 0.8f;
    [SerializeField] private float stage3FireCooldown = 0.6f;
    [SerializeField] private float superFireCooldown = 0.4f;

    // 0=기본, 1=2단계, 2=3단계, 3=슈퍼탱크
    private int upgradeLevel = 0;
    private int starCount = 0;

    private MeshRenderer[] renderers;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        playerAttack = GetComponentInParent<PlayerAttack>();
        if (playerAttack == null) playerAttack = GetComponent<PlayerAttack>();

        ApplyUpgrade();
    }

    // 별 획득 시 강화 단계 상승. 3개 연속 획득 시 슈퍼탱크
    public void OnStarCollected()
    {
        if (upgradeLevel >= 3) return;

        starCount++;
        upgradeLevel = starCount >= 3 ? 3 : Mathf.Min(upgradeLevel + 1, 3);

        ApplyUpgrade();
    }

    // 피격 시 강화 초기화
    public void OnDestroyed()
    {
        upgradeLevel = 0;
        starCount = 0;
        ApplyUpgrade();
    }

    // 강화 단계에 따라 색상과 발사 쿨다운 적용
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

        foreach (var r in renderers)
            r.material.color = color;

        playerAttack?.SetFireCooldown(cooldown);
    }
}