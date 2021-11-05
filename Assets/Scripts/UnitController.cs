using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Side = GameController.Side;

public class UnitController : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject movableTile;
    Collider2D col;

    // References to unit sprites
    public Sprite blue_villager, blue_warrior, blue_armor, blue_archer;
    public Sprite red_villager, red_warrior, red_armor, red_archer;

    // Gameplay
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
    public void SetX(int _x)
    {
        x = _x;
    }
    public void SetY(int _y)
    {
        y = _y;
    }

    public void updatePositionOnMap()
    {
        var pos = new Vector3(this.x - 3.5f, this.y - 3.5f, -1);

        this.transform.position = pos;
    }

    public int GetHP() { return HP; }
    public void SetHP(int _HP) { HP = _HP; }

    public bool isRed() { return side == Side.RED; }

    public void destroyMovableTile()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovableTile");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void initMovePlates()
    {
        switch (this.name)
        {
            default:
                drawMovableTiles();
                break;
        }
    }

    private void drawMovableTiles()
    {
        var move = 2;

        GameController gC = this.controller.GetComponent<GameController>();

        for (int i = 1; i <= move; i++)
        {
            // draw left
            if (gC.isValidPos(this.x - i, this.y) && gC.GetUnitAt(this.x - i, this.y) == null)
            {
                drawMovableTile(this.x - i, this.y);
            }
            // draw up
            if (gC.isValidPos(this.x + i, this.y) && gC.GetUnitAt(this.x + i, this.y) == null)
            {
                drawMovableTile(this.x + i, this.y);
            }
            // draw right
            if (gC.isValidPos(this.x, this.y + i) && gC.GetUnitAt(this.x, this.y + i) == null)
            {
                drawMovableTile(this.x, this.y + i);
            }
            // draw down
            if (gC.isValidPos(this.x, this.y - i) && gC.GetUnitAt(this.x, this.y - i) == null)
            {
                drawMovableTile(this.x, this.y - i);
            }

            // draw diagonal left top
            if (i >= 2 && gC.isValidPos(this.x - (i - 1), this.y + (i - 1)) && gC.GetUnitAt(this.x - (i - 1), this.y + (i - 1)) == null)
            {
                drawMovableTile(this.x - (i - 1), this.y + (i - 1));
            }
            // draw diagonal right top
            if (i >= 2 && gC.isValidPos(this.x + (i - 1), this.y + (i - 1)) && gC.GetUnitAt(this.x + (i - 1), this.y + (i - 1)) == null)
            {
                drawMovableTile(this.x + (i - 1), this.y + (i - 1));
            }
            // draw diagonal left bot
            if (i >= 2 && gC.isValidPos(this.x - (i - 1), this.y - (i - 1)) && gC.GetUnitAt(this.x - (i - 1), this.y - (i - 1)) == null)
            {
                drawMovableTile(this.x - (i - 1), this.y - (i - 1));
            }
            // draw diagonal right bot
            if (i >= 2 && gC.isValidPos(this.x + (i - 1), this.y - (i - 1)) && gC.GetUnitAt(this.x + (i - 1), this.y - (i - 1)) == null)
            {
                drawMovableTile(this.x + (i - 1), this.y - (i - 1));
            }
        }
    }

    private void drawMovableTile(int x, int y)
    {
        GameObject mP = Instantiate(this.movableTile, new Vector3(x - 3.5f, y - 3.5f, -3.0f), Quaternion.identity);

        MovableTile mT = mP.GetComponent<MovableTile>();
        mT.setUnitOwnMovableTile(this.gameObject);
        mT.setWorldPosition(x, y);
    }

    private void OnMouseUp()
    {
        if (!this.controller.GetComponent<GameController>().isGameEnd() &&
            this.controller.GetComponent<GameController>().getCurPlayer() == this.side)
        {
            this.destroyMovableTile();

            this.initMovePlates();
        }


    }


    // -----------------------------------
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    public void HandleInput()
    {
        if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
        {
            Touch touch = Input.touches[0];
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);
            // Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            // RaycastHit2D raycastInfo;

            if (col == Physics2D.OverlapPoint(touchPos))
            {
                isChosen = true;
                //GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                isChosen = false;
                //GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    public void Activate()
    {
        controller = GameObject.FindWithTag("GameController");

        switch (this.name)
        {
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
    void Update()
    {
        // HandleInput();
    }
}
