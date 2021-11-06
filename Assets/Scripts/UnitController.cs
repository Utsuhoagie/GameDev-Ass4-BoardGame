using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Side = GameController.Side;

public class UnitController : MonoBehaviour
{
    // References
    public GameObject game;
    GameController gC;
    Collider2D col;
    SpriteRenderer spRend;
    public GameObject MoveTile;

    // References to unit sprites
    public Sprite blue_villager, blue_warrior, blue_armor, blue_archer;
    public Sprite red_villager, red_warrior, red_armor, red_archer;

    // Gameplay
    private Side side;
    bool isChosen = false;
    bool isMovable = false;

    // Stats
    int HP = 100;
    int move;

    // Positions
    int x;
    int y;

    // --- Getters & Setters ------------------
    public int GetX() { return x; }
    public int GetY() { return y; }
    public void SetX(int _x) { x = _x; }
    public void SetY(int _y) { y = _y; }

    public void UpdatePos()
    {
        var pos = new Vector3(x, y, -1);

        this.transform.position = pos;
    }

    public int GetHP() { return HP; }
    public void SetHP(int _HP) { HP = _HP; }

    public void SetMovable(bool movable) { 
        isMovable = movable;

        if (isMovable) {
            spRend.color = Color.white;
        }
        else {
            spRend.color = Color.gray;
        }
    }

    public bool isRed() { return side == Side.RED; }

    // -----------------------------------
    // Awake is called FIRST
    void Awake() {
        game = GameObject.FindWithTag("GameController");
        gC = game.GetComponent<GameController>();
        col = GetComponent<Collider2D>();
        spRend = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    public void HandleInput()
    {
        if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
        {
            Touch touch = Input.touches[0];
            Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);

            if (col == Physics2D.OverlapPoint(touchPos))
            {
                isChosen = true;
            }
            else
            {
                isChosen = false;
            }
        }
    }

    private void OnMouseUp()
    {
        if (!gC.isGameEnd() &&
            gC.getCurPlayer() == this.side && this.isMovable)
        {
            this.destroyTiles();

            this.initMovePlates();
        }
    }

    public void Activate()
    {
        this.isMovable = true;
        
        switch (this.name)
        {
            case "blue_villager":
                spRend.sprite = blue_villager;
                side = Side.BLUE;
                break;
            case "blue_warrior":
                spRend.sprite = blue_warrior;
                side = Side.BLUE;
                break;
            case "blue_armor":
                spRend.sprite = blue_armor;
                side = Side.BLUE;
                break;
            case "blue_archer":
                spRend.sprite = blue_archer;
                side = Side.BLUE;
                break;
            case "red_villager":
                spRend.sprite = red_villager;
                side = Side.RED;
                break;
            case "red_warrior":
                spRend.sprite = red_warrior;
                side = Side.RED;
                break;
            case "red_armor":
                spRend.sprite = red_armor;
                side = Side.RED;
                break;
            case "red_archer":
                spRend.sprite = red_archer;
                side = Side.RED;
                break;
        }

        if (spRend.sprite == red_villager || spRend.sprite == blue_villager)
            move = 3;
        else
            move = 2;
    }

    void Update()
    {
        // HandleInput();
    }


    // ---- Tiles ---------------------------

    public void initMovePlates()
    {
        switch (this.name)
        {
            default:
                drawMoveTiles();
                break;
        }
    }

    private void drawMoveTiles()
    {
        // TODO: CHANGE WITH FLOOD FILL!!!!
        for (int i = 1; i <= move; i++)
        {
            // draw left
            if (gC.isValidPos(x - i, y) && gC.GetUnitAt(x - i, y) == null)
                drawMoveTile(x - i, y);
            // draw up
            if (gC.isValidPos(x + i, y) && gC.GetUnitAt(x + i, y) == null)
                drawMoveTile(x + i, y);
            // draw right
            if (gC.isValidPos(x, y + i) && gC.GetUnitAt(x, y + i) == null)
                drawMoveTile(x, y + i);
            // draw down
            if (gC.isValidPos(x, y - i) && gC.GetUnitAt(x, y - i) == null)
                drawMoveTile(x, y - i);

            // draw diagonal left top
            if (i >= 2 && gC.isValidPos(x - (i - 1), y + (i - 1)) && gC.GetUnitAt(x - (i - 1), y + (i - 1)) == null)
                drawMoveTile(x - (i - 1), y + (i - 1));
            // draw diagonal right top
            if (i >= 2 && gC.isValidPos(x + (i - 1), y + (i - 1)) && gC.GetUnitAt(x + (i - 1), y + (i - 1)) == null)
                drawMoveTile(x + (i - 1), y + (i - 1));
            // draw diagonal left bot
            if (i >= 2 && gC.isValidPos(x - (i - 1), y - (i - 1)) && gC.GetUnitAt(x - (i - 1), y - (i - 1)) == null)
                drawMoveTile(x - (i - 1), y - (i - 1));
            // draw diagonal right bot
            if (i >= 2 && gC.isValidPos(x + (i - 1), y - (i - 1)) && gC.GetUnitAt(x + (i - 1), y - (i - 1)) == null)
                drawMoveTile(x + (i - 1), y - (i - 1));
        }
    }

    private void drawMoveTile(int x, int y)
    {
        GameObject mP = Instantiate(this.MoveTile, new Vector3(x, y, -3.0f), Quaternion.identity);

        MoveTile mT = mP.GetComponent<MoveTile>();
        mT.setUnitOwnMoveTile(this.gameObject);
        mT.setWorldPosition(x, y);
    }


    public void destroyTiles()
    {
        GameObject[] mvTiles = GameObject.FindGameObjectsWithTag("MoveTile");//"MvTile");
        for (int i = 0; i < mvTiles.Length; i++)
            Destroy(mvTiles[i]);
    }
}
