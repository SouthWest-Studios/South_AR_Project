using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    [Header("Basic")]
    public GameObject[] enemies;
    public Transform bossGraphics;
    


    [Header("Misc")]
    public int bossLife = 10;
    public int faceFacing = 0;
    public float rotationDuration = 0.3f;
    public float timeInvulneravility = 10.0f;
    public float alphaDuration = 0.5f;
    [Range(0f, 1f)]  
    public float alphaTarget = 0.3f;

    [Header("Movement")]
    public float radiusMovement = 10;
    public float movementDuration = 3;
    public float maxMinVerticalHight = 2;

    [Header("Enemies")]
    public float timeBetweenEnemies = 3.5f;
    public float timeBetweenEnemiesInInvulnerabilityTime = 1;
    public float speedEnemies = 0.2f;

    [Header("Particles")]
    public ParticleSystem[] particles;
    public ParticleSystem changeFaseParticles;
    public ParticleSystem dieParticles;
    public Gradient colorOverLifeTimeNormal;
    public Gradient colorOverLifeTimeInvulnerability;



    private Coroutine rotateCoroutine;
    private int lastFaceFacing = 0;

    private bool invulnerability = false;
    private bool isMoving = false;

    private float timerStartCounter = 5.0f;
    private bool startingSpawner = false;
    public TextMeshProUGUI timerText;
    private bool died = false;
    private bool diedChecked = false;

    private float timerDeadBoss = 5;

    private EnemyType[] bossTypes = { EnemyType.Earth, EnemyType.Fire, EnemyType.Wind, EnemyType.Ice };


    // Start is called before the first frame update
    void Start()
    {
        MessageManager.Instance.RegisterEnemy(this.gameObject);
        lastFaceFacing = faceFacing;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (died)
        {
            timerDeadBoss -= Time.deltaTime;
            if (timerDeadBoss < 0)
            {
                SceneManager.LoadScene("MainMenu");
            }
            if (!diedChecked)
            {
                diedChecked = true;
                StopAllCoroutines();
                Transform[] childs = bossGraphics.GetComponentsInChildren<Transform>();

                //Fuerzas cabezas
                foreach (Transform child in childs)
                {
                    Vector3 explosionPosition = new Vector3(Random.Range(-10.0f, 10.0f), -150, Random.Range(-10.0f, 10.0f));
                    Rigidbody rb = child.gameObject.AddComponent<Rigidbody>();
                    rb.AddExplosionForce(500, transform.position, 100);
                    rb.AddTorque(new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)), ForceMode.Impulse);
                }

                //Apagar particulas
                foreach (ParticleSystem ps in particles)
                {
                    var psMain = ps.main;
                    psMain.loop = false;
                }
                dieParticles.Emit(20);

                Enemy[] enemiesInGame = GameObject.FindObjectsOfType<Enemy>();
                for(int i = 0; i<enemiesInGame.Length; i++)
                {
                    Destroy(enemiesInGame[i].gameObject);
                }

            }
            return;

        }


        BossFacing();
        if (!isMoving) MoveBossToRandomPoint();

        if(timerStartCounter >= 0 && !startingSpawner)
        {
            timerStartCounter -= Time.deltaTime;

            timerText.text = timerStartCounter.ToString("0");

            if (timerStartCounter <= 0)
            {
                timerText.text = "";
                startingSpawner = true;
                StartCoroutine(SpawnNormalTimeEnemy());
            }
        }

    }


    void BossFacing()
    {
        this.transform.LookAt(Camera.main.transform.position);
        if (faceFacing != lastFaceFacing)
        {
            if (rotateCoroutine != null)
                StopCoroutine(rotateCoroutine);

            rotateCoroutine = StartCoroutine(RotateOverTime(faceFacing));
            lastFaceFacing = faceFacing;
        }
    }

    IEnumerator RotateOverTime(int faceFacing)
    {
        Quaternion startRotation = bossGraphics.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0, 90f * faceFacing, 0);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float t = elapsed / rotationDuration;
            bossGraphics.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossGraphics.localRotation = targetRotation;
    }

    IEnumerator AlphaMaterial(float targetAlpha)
    {
        Renderer[] renderers = bossGraphics.GetComponentsInChildren<Renderer>();

        List<Material> materials = new List<Material>();
        List<float> startAlphas = new List<float>();

        foreach (Renderer rend in renderers)
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetFloat("_Mode", 2);
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;

                materials.Add(mat);
                startAlphas.Add(mat.color.a);
            }
        }

        float elapsed = 0f;

        while (elapsed < alphaDuration)
        {
            float t = elapsed / alphaDuration;
            for (int i = 0; i < materials.Count; i++)
            {
                Color col = materials[i].color;
                col.a = Mathf.Lerp(startAlphas[i], targetAlpha, t);
                materials[i].color = col;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        
        for (int i = 0; i < materials.Count; i++)
        {
            Color col = materials[i].color;
            col.a = targetAlpha;
            materials[i].color = col;
        }
    }

    IEnumerator Invulnerability()
    {
        invulnerability = true;
        ApplyGradientToParticles(colorOverLifeTimeInvulnerability);
        StartCoroutine(SpawnInvulnerabilityTimeEnemy());
        StartCoroutine(AlphaMaterial(alphaTarget));
        yield return new WaitForSeconds(timeInvulneravility);
        StartCoroutine(AlphaMaterial(1f));
        invulnerability = false;
        ApplyGradientToParticles(colorOverLifeTimeNormal);
    } 

    IEnumerator SpawnInvulnerabilityTimeEnemy()
    {

        SpawnEnemy();
        yield return new WaitForSeconds(timeBetweenEnemiesInInvulnerabilityTime);
        if (invulnerability)
        {
            StartCoroutine(SpawnInvulnerabilityTimeEnemy());
        }
        
    }

    IEnumerator SpawnNormalTimeEnemy()
    {
        SpawnEnemy();
        yield return new WaitForSeconds(timeBetweenEnemies);
        StartCoroutine(SpawnNormalTimeEnemy());

    }

    void SpawnEnemy()
    {
        GameObject enemyGO = Instantiate(enemies[Random.Range(0, enemies.Length)], this.transform.position, Quaternion.identity);
        enemyGO.GetComponent<Enemy>().speed = speedEnemies;
    }


    public EnemyType GetBossType()
    {
        if (faceFacing >= 0 && faceFacing < bossTypes.Length)
        {
            return bossTypes[faceFacing];
        }
        return EnemyType.Earth;
    }

    public void TakeDamage()
    {
        if (invulnerability || died) return;

        bossLife--;
        if(bossLife <= 0)
        {
            died = true;
            return;
        }

        changeFaseParticles.Emit(40);

        int randomFaceFacing = Random.Range(0, 4);
        while (randomFaceFacing == faceFacing) { randomFaceFacing = Random.Range(0, 4); }
        faceFacing = randomFaceFacing;

        StartCoroutine(Invulnerability());
    }



    void MoveBossToRandomPoint()
    {
        isMoving = true;
        Vector3 randomPoint = RandomPointInCircle();
        StartCoroutine(MoveBoss(randomPoint));
    }

    IEnumerator MoveBoss(Vector3 targetPosition)
    {
        float elapsedTime = 0f;

        Vector3 startPos = transform.position;

        Vector3 direction = (targetPosition - Camera.main.transform.position).normalized;
        float targetY = targetPosition.y;
        targetPosition = Camera.main.transform.position + direction * radiusMovement;
        targetPosition.y = targetY;


        while (elapsedTime < movementDuration)
        {
            float t = elapsedTime / movementDuration;
            Vector3 newPos = Vector3.Lerp(startPos, targetPosition, t);
            newPos = (newPos - Camera.main.transform.position).normalized * radiusMovement + Camera.main.transform.position;

            
            newPos.y = Mathf.Clamp(newPos.y, Camera.main.transform.position.y - maxMinVerticalHight, Camera.main.transform.position.y + maxMinVerticalHight);

            transform.position = newPos;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isMoving = false;
        transform.position = targetPosition;
    }

    public Vector3 RandomPointInCircle()
    {
        Vector3 initialPosition = Camera.main.transform.position;
        float angle = Mathf.PI * Random.Range(0f, 1f);
        float x = initialPosition.x + radiusMovement * Mathf.Cos(angle);
        float z = initialPosition.z + radiusMovement * Mathf.Sin(angle);
        float y = Random.Range(Camera.main.transform.position.y - maxMinVerticalHight, maxMinVerticalHight + Camera.main.transform.position.y);
        return new Vector3(x, y, z);
    }

    void ApplyGradientToParticles(Gradient gradient)
    {
        foreach (var particle in particles)
        {
            // Cambiar el gradiente de color sobre el lifetime
            var colorModule = particle.colorOverLifetime;
            colorModule.color = gradient; // Aplicar el gradiente
        }
    }

}
