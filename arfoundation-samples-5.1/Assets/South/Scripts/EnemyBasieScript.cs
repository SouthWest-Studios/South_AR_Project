using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasieScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject camara = null;

    private Rigidbody rb;

    public float speed = 0.5f;

    private GameObject gameManager;

    void Start()
    {
        camara = GameObject.Find("Main Camera");
        gameManager = GameObject.Find("GameManager");
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (camara != null)
        {
            Vector3 direction = camara.transform.position - this.transform.position;

            rb.velocity = direction * speed;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("MainCamera"))
        {
            gameManager.GetComponent<ARBallController>().ShowCanvas();
            Destroy(this.gameObject);
            
        }
    }
}
