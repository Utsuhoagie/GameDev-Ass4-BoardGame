using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : MonoBehaviour
{

    public GameObject controller;
    private GameObject unit = null; // which unit "owns" this movable tile

    private int x;
    private int y;

    //--------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //-----------------------------------------

    public void OnTap() {
        controller = GameObject.FindWithTag("GameController");

        con
    }

}
