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

        // if is null remove
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                Destroy(enemyIndicators[i].gameObject);
                enemyIndicators.RemoveAt(i);
                enemies.RemoveAt(i);
            }
        }

        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        float borderOffset = 50f;

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(enemies[i].transform.position);
            RectTransform indicator = enemyIndicators[i];

            bool isBehind = screenPos.z < 0;
            Vector2 dir = ((Vector2)screenPos - screenCenter).normalized;

            if (screenPos.x >= 0 && screenPos.x <= Screen.width &&
                screenPos.y >= 0 && screenPos.y <= Screen.height &&
                !isBehind)
            {
                indicator.gameObject.SetActive(false);
            }
            else
            {
                indicator.gameObject.SetActive(true);
                if (isBehind) dir *= -1f;

                Vector2 clampedScreenPos = screenCenter + dir * (Mathf.Min(screenCenter.x, screenCenter.y) - borderOffset);
                Vector2 anchoredPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvasTransform as RectTransform, clampedScreenPos, null, out anchoredPos);
                indicator.anchoredPosition = anchoredPos;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                indicator.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
    }


}
