using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour
{

    private Rigidbody2D body;

    public float speed;

    private Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            move = new Vector3(0, 1, 0);
            transform.position += move * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            move = new Vector3(-1, 0, 0);
            transform.position += move * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            move = new Vector3(0, -1, 0);
            transform.position += move * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            move = new Vector3(1, 0, 0);
            transform.position += move * Time.deltaTime * speed;
        }
    }
}
