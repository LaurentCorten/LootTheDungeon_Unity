using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] Image fillImage; // Image type "Filled", Fill Method Horizontal

    public void SetHealth(int current, int max)
    {
        fillImage.fillAmount = max > 0 ? (float)current / max : 0f;
    }
}