using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public GameObject invObject;

    private bool invOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        invObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !invOpen)
        {
            invObject.SetActive(true);
            invOpen = true;
        }
        else if(Input.GetKeyDown(KeyCode.I) && invOpen)
        {
            invObject.SetActive(false);
            invOpen = false;
        }
    }
}
