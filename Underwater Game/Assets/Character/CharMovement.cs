using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharMovement : MonoBehaviour
{

    private Rigidbody2D body;

    public float speed;
    public float jumpSpeed;
    public float waterSpeed;
    public float waterDeceleration;

    public bool door;
    private bool door2;
    private bool dock;
    private bool dock2;
    private bool isGrounded;

    private Vector3 move;
    private Vector2 waterAcceleration;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        StartCoroutine(ChangeScene("Main"));
    }

    //Trigger Collisions (this is for the transitional stuff/changing scenes)
    void OnTriggerEnter2D(Collider2D info)
    {
        if (info.gameObject.name == "Door")
            door = true;

        if (info.gameObject.name == "Door2")
            door2 = true;


        if (info.gameObject.name == "Dock")
            dock = true;

        if (info.gameObject.name == "Dock2")
            dock2 = true;
    }

    void OnTriggerExit2D(Collider2D info)
    {
        if (info.gameObject.name == "Door")
            door = false;

        if (info.gameObject.name == "Door2")
            door2 = false;


        if (info.gameObject.name == "Dock")
            dock = false;

        if (info.gameObject.name == "Dock2")
            dock2 = false;
    }

    //Regular Collisions (mostly just floors to check when the object is grounded)
    void OnCollisionEnter2D(Collision2D info)
    {
        if (info.gameObject.name == "Floor")
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D info)
    {
        if (info.gameObject.name == "Floor")
            isGrounded = false;
    }

    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Water")
            waterMovement();
        else
            normalMovement();

        if (dock)
        {
          StartCoroutine(ChangeScene("Water"));
          dock = false;
        }

        if (dock2)
        {
            StartCoroutine(ChangeScene("Main"));
            dock2 = false;
        }

        if (door && Input.GetKeyDown(KeyCode.Space))
        {
          StartCoroutine(ChangeScene("Home"));
          door = false; 
        }

        if (door2 && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ChangeScene("Main"));
            door2 = false;
        }
    }

    // Movement of the character when it's not in the water
    public void normalMovement()
    {
        body.gravityScale = 1;

        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            body.velocity = new Vector2(0, jumpSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            move = new Vector3(-1, 0, 0);
            transform.position += move * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            move = new Vector3(1, 0, 0);
            transform.position += move * Time.deltaTime * speed;
        }
    }

    // Movement of the character when it's in the water
    public void waterMovement()
    {
        body.velocity = new Vector2(body.velocity.x + waterAcceleration.x * Time.deltaTime,
                                    body.velocity.y + waterAcceleration.y * Time.deltaTime);
        body.gravityScale = 0;

        if (Input.GetKey(KeyCode.W))
        {
            body.velocity = new Vector2(body.velocity.x, waterSpeed);
            waterAcceleration = new Vector2(waterAcceleration.x, -waterDeceleration);
        }

        if (Input.GetKey(KeyCode.A))
        {
            body.velocity = new Vector2(-waterSpeed, body.velocity.y);
            waterAcceleration = new Vector2(waterDeceleration, waterAcceleration.y);
        }

        if (Input.GetKey(KeyCode.D))
        {
            body.velocity = new Vector2(waterSpeed, body.velocity.y);
            waterAcceleration = new Vector2(-waterDeceleration, waterAcceleration.y);
        }

        if (Input.GetKey(KeyCode.S))
        {
            body.velocity = new Vector2(body.velocity.x, -waterSpeed);
            waterAcceleration = new Vector2(waterAcceleration.x, waterDeceleration);
        }

        if (body.velocity.y >= -0.1 && body.velocity.y <= 0.1)
        {
            waterAcceleration = new Vector2(waterAcceleration.x, 0);
            body.velocity = new Vector2(body.velocity.x, 0);
        }

        if (body.velocity.x >= -0.1 && body.velocity.x <= 0.1)
        {
            waterAcceleration = new Vector2(0, waterAcceleration.y);
            body.velocity = new Vector2(0, body.velocity.y);
        }
    }

    // Method to change scenes
    IEnumerator ChangeScene(string nextScene)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
            yield return null;

        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(nextScene));

        SceneManager.UnloadSceneAsync(currentScene);
    }
}

