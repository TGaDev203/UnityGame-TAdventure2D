using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource backgroundAudioSource;
    public AudioSource effectAudioSource;
    public AudioClip hitEnemySound;
    public AudioClip coinEnemySound;
    public AudioClip waterSplashSound;
    public AudioClip waterWalkingSound;
    public AudioClip menuButtonProgressSound;
    public AudioClip menuButtonEndSound;
    public AudioClip bouncingSound;
    public AudioClip mainMenuSound;
    public AudioClip gameplaySound;
    protected bool canPlayEndSound = true;
    protected bool canPlayProgressSound = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayLoopSound();
    }

    private void PlaySound(AudioClip clip)
    {
        if (effectAudioSource == null || clip == null) return;
        effectAudioSource.PlayOneShot(clip);
    }

    public void PlayerHitSound() => PlaySound(hitEnemySound);
    public void PlayCoinSound() => PlaySound(coinEnemySound);
    public void PlayWaterSplashSound() => PlaySound(waterSplashSound);
    public void PlayMenuButtonProgressSound() => PlaySound(menuButtonProgressSound);
    public void PlayMenuButtonEndSound() => PlaySound(menuButtonEndSound);

    public void PlayLoopSound()
    {
        if (backgroundAudioSource == null) return;

        string scene = SceneManager.GetActiveScene().name;
        backgroundAudioSource.loop = true;

        if (scene == "Main_Scene")
        {
            backgroundAudioSource.clip = mainMenuSound;
        }

        else
        {
            backgroundAudioSource.clip = gameplaySound;
        }

        backgroundAudioSource.Play();
    }

    public void PlayBouncingSound()
    {
        if (effectAudioSource == null || bouncingSound == null) return;
        effectAudioSource.PlayOneShot(bouncingSound);
    }

    public void PlayWaterWalkingSound()
    {
        if (effectAudioSource == null || waterWalkingSound == null) return;
        effectAudioSource.clip = waterWalkingSound;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }

    public void StopWaterWalkingSound()
    {
        if (effectAudioSource == null) return;

        effectAudioSource.Stop();
    }
}
