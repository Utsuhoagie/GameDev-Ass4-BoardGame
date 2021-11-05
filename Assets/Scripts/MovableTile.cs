using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : MonoBehaviour
{

    public GameObject controller;
    private GameObject unit = null; // which unit "owns" this movable tile

    private int x;
    private int y;

    public bool isAttack = false;

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

    public void OnMouseUp()
    {
        controller = GameObject.FindWithTag("GameController");

        if (this.isAttack)
        {
            //? Destroy obj currently on pos if move to that pos
            GameObject obj = controller.GetComponent<GameController>().GetUnitAt(x, y);
            Destroy(obj);
        }

        //? move obj to pos
        controller.GetComponent<GameController>().setPositionEmpty(
            this.unit.GetComponent<UnitController>().GetX(),
            this.unit.GetComponent<UnitController>().GetY()
            );

        this.unit.GetComponent<UnitController>().SetX(x);
        this.unit.GetComponent<UnitController>().SetY(y);

        this.unit.GetComponent<UnitController>().updatePositionOnMap();
        controller.GetComponent<GameController>().SetUnitAt(x, y, this.unit);

        this.unit.GetComponent<UnitController>().destroyMovableTile();
    }

    public void setUnitOwnMovableTile(GameObject obj)
    {
        this.unit = obj;
    }

    public void setWorldPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public GameObject getCurrentlyUnitInUsed()
    {
        return this.unit;
    }
}
