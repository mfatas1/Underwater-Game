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
    public bool dock;

    private Vector3 move;
    private Vector2 waterAcceleration;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Collisions
    void OnTriggerEnter2D(Collider2D info)
    {
        if (info.gameObject.name == "Door")
            door = true;

        if (info.gameObject.name == "Door2")
            door2 = true;

        if (info.gameObject.name == "Dock")
            dock = true;
    }

    void OnTriggerExit2D(Collider2D info)
    {
        if (info.gameObject.name == "Door")
            door = false;

        if (info.gameObject.name == "Dock")
            dock = false;

        if (info.gameObject.name == "Door2")
            door2 = false;
    }

    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Water")
            waterMovement();
        else
            normalMovement();

        if (dock)
        {
          StartCoroutine(ChangeScene());
          dock = false;
        }
        if((door || door2) && Input.GetKeyDown(KeyCode.Space))
        {
          StartCoroutine(ChangeScene());
          if(door2)
              transform.position = new Vector3(-8.464f, 0.4131f, 0f); print("hello");
          door = false; 
          door2 = false;
        }
    }

    // Movement of the character when it's not in the water
    public void normalMovement()
    {
        if (Input.GetKey(KeyCode.W))
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
    IEnumerator ChangeScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string nextScene = null;

        if (door)
            nextScene = "Home";

        if(door2)
            nextScene = "Main";

        if (dock)
        {
            if (currentScene.name == "Main")
            {
                nextScene = "Water";
                body.gravityScale = 0;
                body.velocity = new Vector3(0, 0, 0);
            }
            if (currentScene.name == "Water")
            {
                nextScene = "Main";
                body.gravityScale = 1;
            }
        }


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
            yield return null;

        if(currentScene.name == "Main")
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(nextScene));

        SceneManager.UnloadSceneAsync(currentScene);

        door = false;
        dock = false;
    }
}

