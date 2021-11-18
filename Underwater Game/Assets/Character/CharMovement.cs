using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharMovement : MonoBehaviour
{

    private Rigidbody2D body;

    public float speed;
    public float jumpSpeed;

    public bool door;

    private Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Collisions
    void OnTriggerEnter2D(Collider2D info)
    {
        if (info.tag == "Door")
            door = true;
    }

    void OnTriggerExit2D(Collider2D info)
    {
        if (info.tag == "Door")
            door = false;
    }
    // Update is called once per frame
    void Update()
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

        if (Input.GetKeyDown(KeyCode.Return) && door)
        {
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string nextScene = null;
        if (currentScene.name == "Main")
            nextScene = "Home";
        if (currentScene.name == "Home")
            nextScene = "Main";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(nextScene));

        SceneManager.UnloadSceneAsync(currentScene);

        door = false;
    }
}
