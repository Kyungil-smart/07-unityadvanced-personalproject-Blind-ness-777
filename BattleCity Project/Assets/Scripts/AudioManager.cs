using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip musicSteady;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip tankExplosion;
    [SerializeField] private AudioClip shotFiring;
    [SerializeField] private AudioClip shellExplosion;
    [SerializeField] private AudioClip pickupPowerUp;

    // 씬 전환 후에도 오디오 유지
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

    private void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (bgmSource == null || musicSteady == null) return;
        bgmSource.clip = musicSteady;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null)
            bgmSource.Stop();
    }

    // OptionPopup 볼륨 슬라이더와 연동
    public void SetVolume(float volume)
    {
        if (bgmSource != null) bgmSource.volume = volume;
        if (sfxSource != null) sfxSource.volume = volume;
    }

    // PlayOneShot으로 재생해서 SFX 중첩 가능
    public void PlayTankExplosion() => sfxSource?.PlayOneShot(tankExplosion);
    public void PlayShotFiring() => sfxSource?.PlayOneShot(shotFiring);
    public void PlayShellExplosion() => sfxSource?.PlayOneShot(shellExplosion);
    public void PlayPickupPowerUp() => sfxSource?.PlayOneShot(pickupPowerUp);
}