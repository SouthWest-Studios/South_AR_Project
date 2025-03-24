using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    private int radius = 50;
    private Vector3 initialPosition;
    private float spawnTime = 2;
    private float spawnTimeCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTimeCounter >= spawnTime)
        {
            SpawnEnemy(enemies[Random.Range(0, enemies.Length)]);

            spawnTimeCounter = 0;
        }
        spawnTimeCounter += Time.deltaTime;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Vector3 randomPoint = RandomPointInCircle();
        Instantiate(enemy, randomPoint, Quaternion.identity);
        enemy.GetComponent<EnemyBasieScript>().player = this.gameObject;
        Debug.Log("Enemy spawned at: " + randomPoint);
    }

    Vector3 RandomPointInCircle()
    {
        float angle = 2 * Mathf.PI * Random.Range(0, 360) / 360;

        float x = initialPosition.x + radius * Mathf.Cos(angle);
        float z = initialPosition.z + radius * Mathf.Sin(angle);

        return new Vector3(x, initialPosition.y, z);
    }
}
