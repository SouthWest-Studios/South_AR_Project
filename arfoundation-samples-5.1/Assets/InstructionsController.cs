using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionsController : MonoBehaviour
{
    public float durationToggle = 1.2f;

    public static InstructionsController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = Color.clear;
        ShowInstruccions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInstruccions()
    {
        StartCoroutine(ShowInstructionsC());
    }


    IEnumerator ShowInstructionsC()
    {
        float elapsed = 0f;
        while (elapsed < durationToggle)
        {
            float t = elapsed / durationToggle;
            Color nuevoColor = Color.Lerp(Color.clear, Color.white, t);
            GetComponent<Image>().color = nuevoColor;
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(5);

        elapsed = 0f;
        while (elapsed < durationToggle)
        {
            float t = elapsed / durationToggle;
            Color nuevoColor = Color.Lerp(Color.white, Color.clear, t);
            GetComponent<Image>().color = nuevoColor;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

}
