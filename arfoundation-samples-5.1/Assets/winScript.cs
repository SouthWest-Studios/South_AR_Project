using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winScript : MonoBehaviour
{
    // Start is called before the first frame update


    public CanvasGroup canvasGroup;      
    public float fadeDuration = 5f;       // Tiempo de fadeOut
    void Start()
    {
        StartCoroutine(FadeOutAndSwitch());


    }

    //IEnumerator LoadMainMenuAfterDelay()
    //{
    //    yield return new WaitForSeconds(5f); // Espera 5 segundos
    //    SceneManager.LoadScene("MainMenu");
    //}

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator FadeOutAndSwitch()
    {
        float time = 0f;
        canvasGroup.alpha = 0f; 

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = time / fadeDuration; 
            yield return null;
        }

        canvasGroup.alpha = 1f; 
        yield return new WaitForSeconds(0.5f); 

        SceneManager.LoadScene("MainMenu");
    }
}
