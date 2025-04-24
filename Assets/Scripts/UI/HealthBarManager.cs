using UnityEngine;
using UnityEngine.UI;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    [SerializeField] private Slider slider;

    public void SetHealth(float health) => slider.value = health;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}