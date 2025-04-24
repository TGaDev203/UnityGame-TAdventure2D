using UnityEngine;

public class TrapAnimation : MonoBehaviour
{
    private Animator trapAnimation;

    private void Awake()
    {
        trapAnimation = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trapAnimation.SetBool("isTakenDamage", true);
            trapAnimation.SetBool("isReversed", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trapAnimation.SetBool("isReversed", true);
            trapAnimation.SetTrigger("takeDamage");
        }
    }
}