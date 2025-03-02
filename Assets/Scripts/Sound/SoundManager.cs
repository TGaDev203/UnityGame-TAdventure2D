using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClip hitEnemySound;
    [SerializeField] private AudioClip coinEnemySound;
    [SerializeField] private AudioClip waterSplashSound;
    [SerializeField] private AudioClip waterWalkingSound;
    [SerializeField] private AudioClip menuButtonProgressSound;
    [SerializeField] private AudioClip menuButtonEndSound;
    [SerializeField] private AudioClip bouncingSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    public void PlayerHitSound() => PlaySound(hitEnemySound);
    public void PlayCoinSound() => PlaySound(coinEnemySound);
    public void PlayWaterSplashSound() => PlaySound(waterSplashSound);
    public void PlayMenuButtonProgressSound() => PlaySound(menuButtonProgressSound);
    public void PlayMenuButtonEndSound() => PlaySound(menuButtonEndSound);

    public void PlayBouncingSound()
    {
        if (audioSource == null || bouncingSound == null) return;
        audioSource.clip = bouncingSound;
        audioSource.Play();
    }

    public void PlayWaterWalkingSound()
    {
        if (audioSource == null || waterWalkingSound == null) return;
        audioSource.clip = waterWalkingSound;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void StopWaterWalkingSound()
    {
        if (audioSource == null) return;
        audioSource.loop = false;
        audioSource.Stop();
    }

    public void StopAllSound()
    {
        if (audioSource == null) return;
        audioSource.Stop();
    }
}
