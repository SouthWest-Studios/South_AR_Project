using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject camara = null;

    private Rigidbody rb;

    public float speed = 0.5f;

    private GameObject gameManager;

    public EnemyType type;

    public GameObject player;

    EnemySpawner scriptSpawner;
    void Start()
    {
        camara = GameObject.Find("Main Camera");
        gameManager = GameObject.Find("GameManager");
        scriptSpawner = gameManager.GetComponent<EnemySpawner>();

        player = GameObject.Find("Player");

        rb = this.GetComponent<Rigidbody>();

        if (MessageManager.Instance != null)
        {
            MessageManager.Instance.RegisterEnemy(gameObject);
        }
        this.transform.LookAt(camara.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (camara != null)
        {
            Vector3 direction = camara.transform.position - this.transform.position;

            rb.velocity = direction * speed;
            this.transform.LookAt(camara.transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("MainCamera"))
        {

            gameManager.GetComponent<ARBallController>().ShowCanvas();
           
            if (!scriptSpawner.isInfinite)
            {
                player.GetComponent<Player>().TakeDamage(1);
                EnemySpawner.enemiesInGame = EnemySpawner.enemiesInGame - 1;
            }
            
            Destroy(this.gameObject);
            
        }
    }
}
