using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum EnemyType
{
    Fire = 0,
    Ice,
    Wind,
    Earth
}

[System.Serializable]
public struct Wave
{
    public int cantidadEnemigos;
    public float velocidadEnemigos;
    public float spawnRatio;
    public GameObject[] enemies;
}

public class EnemySpawner : MonoBehaviour
{
    
    public float radius = 10f;
    private Vector3 initialPosition;
    public float spawnTime = 5;
    private float spawnTimeCounter = 0;
    private float heightRange = 6;
    public GameObject camara = null;
    public Wave[] Oleada;
    private int currentWaveIndex = -1;
    private float timeBetweenWaves = 5.0f;
    private float timeBetweenWavesCounter = 0;

    public static int enemiesInGame;
    private bool currentWaveRested = false;




    void Start()
    {
        initialPosition = camara.gameObject.transform.position;
    
    }

    // Update is called once per frame
    void Update()
    {

        if (timeBetweenWavesCounter >= timeBetweenWaves)
        {
            
            if (spawnTimeCounter >= Oleada[currentWaveIndex].spawnRatio)
            {
                enemiesInGame = Oleada[currentWaveIndex].cantidadEnemigos;
                if (Oleada[currentWaveIndex].cantidadEnemigos > 0)
                {
                    SpawnEnemy();
                    spawnTimeCounter = 0;
                    Oleada[currentWaveIndex].cantidadEnemigos--;
                }

                
            }
            spawnTimeCounter += Time.deltaTime;
        }
        if (enemiesInGame <= 0)
        {
            if (!currentWaveRested)
            {
                currentWaveIndex++;
                currentWaveRested = true;
            }
            timeBetweenWavesCounter = timeBetweenWavesCounter + Time.deltaTime;
        }


    }

    void SpawnEnemy()
    {
        Vector3 randomPoint = RandomPointInCircle();
        randomPoint.y = Random.Range(camara.gameObject.transform.position.y - heightRange, heightRange + camara.gameObject.transform.position.y);

        EnemyType type = (EnemyType)Random.Range(0, Oleada[currentWaveIndex].enemies.Length);

        //enemyGO.GetComponent<Enemy>().type = (EnemyTypes)index;

        GameObject enemy = Oleada[currentWaveIndex].enemies[(int)type];
        GameObject enemyGO = Instantiate(enemy, randomPoint, Quaternion.identity);
 
        Debug.Log("Enemy spawned at: " + randomPoint);
    }

    Vector3 RandomPointInCircle()
    {

        float angle = Mathf.PI * Random.Range(0f, 1f);
        float x = initialPosition.x + radius * Mathf.Cos(angle);
        float z = initialPosition.z + radius * Mathf.Sin(angle);

        return new Vector3(x, initialPosition.y, z);
    }



}
