using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasieScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player = null;

    private Rigidbody rb;

    public float speed = 0.5f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 direction = player.transform.position - this.transform.position;

            rb.velocity = direction * speed;
        }
    }
}
