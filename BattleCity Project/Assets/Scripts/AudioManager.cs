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

    public void SetVolume(float volume)
    {
        if (bgmSource != null) bgmSource.volume = volume;
        if (sfxSource != null) sfxSource.volume = volume;
    }

    public void PlayTankExplosion()
    {
        sfxSource?.PlayOneShot(tankExplosion);
    }

    public void PlayShotFiring()
    {
        sfxSource?.PlayOneShot(shotFiring);
    }

    public void PlayShellExplosion()
    {
        sfxSource?.PlayOneShot(shellExplosion);
    }

    public void PlayPickupPowerUp()
    {
        sfxSource?.PlayOneShot(pickupPowerUp);
    }
}