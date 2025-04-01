using UnityEngine;

public class CheckPointAnimation : MonoBehaviour
{
    private ParticleSystem endEffect;

    private void Awake()
    {
        endEffect = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            endEffect.Play();
        }
    }
}
