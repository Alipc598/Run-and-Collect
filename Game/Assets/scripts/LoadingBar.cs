using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO; 


public class LoadingBar : MonoBehaviour
{
    public Slider loadingSlider;
    public float loadingTime = 5f;

    void Start()
    {
        if (loadingSlider != null)
        {
            StartCoroutine(FillLoadingBar());
        }
    }

    IEnumerator FillLoadingBar()
    {
        float elapsedTime = 0f;

        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            loadingSlider.value = Mathf.Clamp01(elapsedTime / loadingTime);
            yield return null;
        }

        string nextScenePath = PlayerPrefs.GetString("NextScene", "DefaultSceneName");
        Debug.Log("Loading next scene: " + nextScenePath); // Add this line for debugging
        SceneManager.LoadScene(Path.GetFileNameWithoutExtension(nextScenePath));
    }

}
