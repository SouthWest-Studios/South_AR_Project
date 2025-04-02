using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    public GameObject enemyIndicatorPrefab;  // Indicator UI icon
    public Transform canvasTransform;        // UI Canvas

    private Camera mainCamera;
    private List<GameObject> enemies = new List<GameObject>();
    private List<RectTransform> enemyIndicators = new List<RectTransform>();

    void Awake()
    {
        // Ensure the MessageManager is a singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        mainCamera = Camera.main;  // Get the main camera
    }

    void Update()
    {
        // Update the enemy indicator status
        CheckAndShowIndicators();
    }

    // Register an enemy
    public void RegisterEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);

            // Create the enemy's indicator (e.g., an arrow)
            GameObject indicator = Instantiate(enemyIndicatorPrefab, canvasTransform);
            enemyIndicators.Add(indicator.GetComponent<RectTransform>());
            indicator.gameObject.SetActive(false);  // Default is hidden
        }
    }

    // Remove an enemy
    public void RemoveEnemy(GameObject enemy)
    {
        int index = enemies.IndexOf(enemy);
        if (index != -1)
        {
            Destroy(enemyIndicators[index].gameObject);  // Delete the indicator
            enemyIndicators.RemoveAt(index);
            enemies.RemoveAt(index);
        }
    }

    // Check if the enemy is out of view, and show or hide the indicator
    private void CheckAndShowIndicators()
    {
        if (mainCamera == null) return;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null) continue;

            // Get the enemy's world position and convert it to screen coordinates
            Vector3 screenPos = mainCamera.WorldToScreenPoint(enemies[i].transform.position);

            // Check if the enemy is out of view
            bool isOutOfView = screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height || screenPos.z < 0;

            if (isOutOfView)
            {
                // If the enemy is out of view, show the indicator
                enemyIndicators[i].gameObject.SetActive(true);
                // Set the indicator position to the screen center
                enemyIndicators[i].anchoredPosition = new Vector2(Screen.width / 2, Screen.height / 2);
            }
            else
            {
                // If the enemy is in view, hide the indicator
                enemyIndicators[i].gameObject.SetActive(false);
            }
        }
    }
}
