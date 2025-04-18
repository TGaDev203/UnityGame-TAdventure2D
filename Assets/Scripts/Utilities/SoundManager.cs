using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource backgroundAudioSource;
    public AudioSource effectAudioSource;
    public AudioClip hitEnemySound;
    public AudioClip coinSound;
    public AudioClip waterSplashSound;
    public AudioClip waterWalkingSound;
    public AudioClip menuButtonProgressSound;
    public AudioClip menuButtonEndSound;
    public AudioClip bouncingSound;
    public AudioClip mainMenuSound;
    public AudioClip gameplaySound;
    public AudioClip rockFallingSound;
    public AudioClip zombieSound;
    public AudioClip crocodileSound;

    public void PlayerHitSound() => PlaySound(hitEnemySound);
    public void PlayCoinSound() => PlaySound(coinSound);
    public void PlayWaterSplashSound() => PlaySound(waterSplashSound);
    public void PlayRockFallingSound() => PlaySound(rockFallingSound);
    public void PlayZombieSound() => PlaySound(zombieSound);
    public void PlayCrocodileSound() => PlaySound(crocodileSound);
    public void PlayMenuButtonProgressSound() => PlaySound(menuButtonProgressSound);
    public void PlayMenuButtonEndSound() => PlaySound(menuButtonEndSound);

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

    public void PlayLoopSound()
    {
        if (backgroundAudioSource == null) return;

        backgroundAudioSource.loop = true;

        string scene = SceneManager.GetActiveScene().name;
        backgroundAudioSource.clip = (scene == "Main_Scene") ? mainMenuSound : gameplaySound;

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