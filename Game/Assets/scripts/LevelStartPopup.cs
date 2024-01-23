using UnityEngine;
using UnityEngine.UI;

public class LevelStartPopup : MonoBehaviour
{
    public GameObject popupPanel; // Assign your Panel in the Inspector

    // Start is called before the first frame update
    void Start()
    {
        ShowPopup();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowPopup();
        }
    }

    // Call this method to show the popup
    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    // Call this method when the "Got it" button is clicked
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}
