using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int maxLives = 3;
    private int currentLives;

    public PlayerHealthUI healthUI;

    void Start()
    {
        currentLives = maxLives;
        healthUI.maxLives = maxLives;
        healthUI.InitHearts();
        healthUI.UpdateHearts(currentLives);
    }

    public void TakeDamage(int damage = 1)
    {
        currentLives -= damage;
        currentLives = Mathf.Max(currentLives, 0);

        healthUI.UpdateHearts(currentLives);

        if (currentLives <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        SceneManager.LoadScene("Die Scene");
    }
}
