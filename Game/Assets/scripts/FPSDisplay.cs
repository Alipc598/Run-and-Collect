using UnityEngine;
using TMPro; // Add this line

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // Change type to TextMeshProUGUI
    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = string.Format("{0:0.} fps", fps);
    }
}
