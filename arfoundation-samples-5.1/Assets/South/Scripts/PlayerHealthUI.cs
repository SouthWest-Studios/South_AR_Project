using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public GameObject heartPrefab;      
    public Transform heartsParent;      
    public int maxLives = 3;

    private List<GameObject> hearts = new List<GameObject>();

    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
 
        foreach (Transform child in heartsParent)
        {
            Destroy(child.gameObject);
        }

        hearts.Clear();

     
        for (int i = 0; i < maxLives; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartsParent);
            heart.transform.position = new Vector3(heart.transform.position.x + i * 50f, heart.transform.position.y, heart.transform.position.z);
            hearts.Add(heart);
        }
    }

    public void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < currentLives); 
        }
    }
}
