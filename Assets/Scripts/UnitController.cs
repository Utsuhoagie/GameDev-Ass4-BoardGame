using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject moveRange;
    Collider2D col;

    // References to unit sprites
    public Sprite blue_villager, blue_warrior, blue_armor, blue_archer;
    public Sprite red_villager, red_warrior, red_armor, red_archer;

    // Gameplay
    enum Side { RED, BLUE }
    private Side side;
    bool isChosen = false;

    // Stats
    private int HP = 10;

    // Positions
    private int x;
    private int y;

    // -----------------------------------
    // Getters & Setters

    public int GetX() { return x; }
    public int GetY() { return y; }
    public void SetX(int _x) { x = _x; }
    public void SetY(int _y) { y = _y; }

    public int GetHP() { return HP; }
    public void SetHP(int _HP) { HP = _HP; }

    public bool isRed() { return side == Side.RED; }


    // -----------------------------------
    // Start is called before the first frame update
    void Start() {
        col = GetComponent<Collider2D>();
    }

    public void HandleInput() {
        if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began)) {
            Touch touch = Input.touches[0];
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
            // Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            // RaycastHit2D raycastInfo;

            if (col == Physics2D.OverlapPoint(touchPos)) {
                isChosen = true;
                //GetComponent<SpriteRenderer>().flipX = true;
            }
            else {
                isChosen = false;
                //GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    public void Activate() {
        controller = GameObject.FindWithTag("GameController");

        switch (this.name) {
            case "blue_villager":
                this.GetComponent<SpriteRenderer>().sprite = blue_villager;
                side = Side.BLUE;
                break;
            case "blue_warrior":
                this.GetComponent<SpriteRenderer>().sprite = blue_warrior;
                side = Side.BLUE;
                break;
            case "blue_armor":
                this.GetComponent<SpriteRenderer>().sprite = blue_armor;
                side = Side.BLUE;
                break;
            case "blue_archer":
                this.GetComponent<SpriteRenderer>().sprite = blue_archer;
                side = Side.BLUE;
                break;
            case "red_villager":
                this.GetComponent<SpriteRenderer>().sprite = red_villager;
                side = Side.RED;
                break;
            case "red_warrior":
                this.GetComponent<SpriteRenderer>().sprite = red_warrior;
                side = Side.RED;
                break;
            case "red_armor":
                this.GetComponent<SpriteRenderer>().sprite = red_armor;
                side = Side.RED;
                break;
            case "red_archer":
                this.GetComponent<SpriteRenderer>().sprite = red_archer;
                side = Side.RED;
                break;
        }

        //Debug.Log($"{name} is at {transform.position} or X,Y = ({x},{y})");
    }


    // Update is called once per frame
    void Update() {
        HandleInput();
    }
}
