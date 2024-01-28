using UnityEngine;
using UnityEngine.UI;

public class SliderColorChange : MonoBehaviour
{
    private Slider slider;
    private Image fillImage;

    void Start()
    {
        slider = GetComponent<Slider>();
        fillImage = transform.Find("Fill Area/Fill").GetComponent<Image>();
    }

    void Update()
    {
        float value = slider.value;
        fillImage.color = Color.Lerp(Color.white, Color.red, value);
    }
}
