using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    // Update is called once per frame
    void Update()
    {
       
    }
    public void LoadNextLevel()
    {

        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    IEnumerator LoadLevel(int levelIndex)
    {


        //play
       transition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(1.51f);
        //load
        SceneManager.LoadScene(levelIndex);
    }
        
    
 }