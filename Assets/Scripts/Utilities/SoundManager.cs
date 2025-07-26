using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip buttonProgressSound;
    public AudioClip buttonEndSound;
    public AudioClip bouncingSound;
    public AudioClip crocodileSound;
    public AudioClip coinCollectedSound;
    public AudioClip deathSound;
    public AudioClip endGameSound;
    public AudioClip goalDeniedSound;
    public AudioClip gameplaySound;
    public AudioClip hitEnemySound;
    public AudioClip healthPickupSound;
    public AudioClip mainMenuSound;
    public AudioClip rockFallingSound;
    public AudioClip waterSplashSound;
    public AudioClip waterWalkingSound;
    public AudioClip zombieSound;
    public AudioSource backgroundAudioSource;
    public AudioSource effectAudioSource;

    public void PlayButtonProgressSound() => PlaySound(buttonProgressSound);
    public void PlayButtonEndSound() => PlaySound(buttonEndSound);
    public void PlayCrocodileSound() => PlaySound(crocodileSound);
    public void PlayCoinSound() => PlaySound(coinCollectedSound);
    public void PlayDeathSound() => PlaySound(deathSound);
    public void PlayGoalDeniedSound() => PlaySound(goalDeniedSound);
    public void PlayHealthPickupSound() => PlaySound(healthPickupSound);
    public void PlayerHitSound() => PlaySound(hitEnemySound);
    public void PlayRockFallingSound() => PlaySound(rockFallingSound);
    public void PlayWaterSplashSound() => PlaySound(waterSplashSound);
    public void PlayZombieSound() => PlaySound(zombieSound);

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
        backgroundAudioSource.clip = (scene == "MainMenu_Scene") ? mainMenuSound : gameplaySound;

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

    public void PlayEndGameSound()
    {
        if (effectAudioSource == null || endGameSound == null) return;
        effectAudioSource.clip = endGameSound;
        effectAudioSource.loop = true;
        effectAudioSource.Play();
    }

    public void StopWaterWalkingSound()
    {
        if (effectAudioSource == null) return;

        effectAudioSource.Stop();
    }
}