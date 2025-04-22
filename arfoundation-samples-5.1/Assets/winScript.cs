using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMainMenuAfterDelay());


    }

    IEnumerator LoadMainMenuAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Espera 5 segundos
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
