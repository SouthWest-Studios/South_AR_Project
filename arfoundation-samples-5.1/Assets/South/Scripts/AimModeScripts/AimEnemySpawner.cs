using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AimEnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public float radius = 10f;
    private Vector3 initialPosition;
    private float spawnTime = 5;
    private float spawnTimeCounter = 0;
    private float heightRange = 6;
    public GameObject camara = null;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = camara.gameObject.transform.position;
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
        randomPoint.y = Random.Range(camara.gameObject.transform.position.y - heightRange, heightRange + camara.gameObject.transform.position.y);
        GameObject enemyGO = Instantiate(enemy, randomPoint, Quaternion.identity);
        MessageManager.Instance.RegisterEnemy(enemyGO);
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
