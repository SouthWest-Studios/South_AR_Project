using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

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
    public float timeBetweenWaves = 4.0f;
    private float timeBetweenWavesCounter = 0;

    public static int enemiesInGame;
    private bool currentWaveRested = false;

    public TextMeshProUGUI timerText;

    public static EnemySpawner instance;

    public bool isInfinite = false;
    public TextMeshProUGUI waveText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }


    void Start()
    {
        initialPosition = camara.gameObject.transform.position;
        spawnTimeCounter = Oleada[0].spawnRatio;
        if (isInfinite)
        {
            currentWaveIndex = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isInfinite)
        {
            waveText.text = "(" + (currentWaveIndex + 1) + "/5)";
        }
            

        if (currentWaveIndex >= Oleada.Length)
        {
            if (isInfinite)
            {
                currentWaveIndex = 0;
            }
            else
            {
                SceneManager.LoadScene("LevelPassed");
            }
               
        }

        if (timeBetweenWavesCounter >= timeBetweenWaves)
        {
            if (currentWaveIndex == -1)
            {
                currentWaveIndex = 0;
            }

            if (spawnTimeCounter >= Oleada[currentWaveIndex].spawnRatio)
            {
                currentWaveRested = false;
                

                if (Oleada[currentWaveIndex].cantidadEnemigos > 0)
                {
                    SpawnEnemy();
                    spawnTimeCounter = 0;
                    if (!isInfinite)
                    {
                        Oleada[currentWaveIndex].cantidadEnemigos--;
                    }
                }

                
            }
            spawnTimeCounter += Time.deltaTime;
            
        }
        if (enemiesInGame <= 0)
        {
            GameObject[] enemiesInGame = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemiesInGame.Length == 0)
            {
                if (!currentWaveRested && currentWaveIndex != -1 && Oleada[currentWaveIndex].cantidadEnemigos <= 0)
                {
                    InstructionsController.instance.ShowInstruccions();
                    currentWaveIndex++;
                    currentWaveRested = true;
                    timeBetweenWavesCounter = 0;
                    if (currentWaveIndex < Oleada.Length) spawnTimeCounter = Oleada[currentWaveIndex].spawnRatio;
                }
                
                timeBetweenWavesCounter = timeBetweenWavesCounter + Time.deltaTime;
                if ((timeBetweenWaves - timeBetweenWavesCounter) >= 0)
                {
                    timerText.text = ((int)(timeBetweenWaves - timeBetweenWavesCounter)).ToString();
                }
                else
                {
                    if (currentWaveIndex == -1)
                    {
                        currentWaveIndex = 0;
                    }
                }
            }
            
            
        }
        

        

    }

    void SpawnEnemy()
    {
        Vector3 randomPoint = RandomPointInCircle();
        //randomPoint.y = Random.Range(camara.gameObject.transform.position.y - heightRange, heightRange + camara.gameObject.transform.position.y);

        EnemyType type = (EnemyType)Random.Range(0, Oleada[currentWaveIndex].enemies.Length);

        //enemyGO.GetComponent<Enemy>().type = (EnemyTypes)index;

        GameObject enemy = Oleada[currentWaveIndex].enemies[(int)type];
        GameObject enemyGO = Instantiate(enemy, randomPoint, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().speed = Oleada[currentWaveIndex].velocidadEnemigos;
    }

    public Vector3 RandomPointInCircle()
    {
        initialPosition = camara.gameObject.transform.position;
        float angle = Mathf.PI * Random.Range(0f, 1f);
        float x = initialPosition.x + radius * Mathf.Cos(angle);
        float z = initialPosition.z + radius * Mathf.Sin(angle);
        float y = Random.Range(camara.gameObject.transform.position.y - heightRange, heightRange + camara.gameObject.transform.position.y);
        return new Vector3(x, y, z);
    }
}
