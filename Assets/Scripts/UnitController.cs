using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Side = GameController.Side;

public class UnitController : MonoBehaviour
{
    // References
    public GameObject game;
    GameController gC;
    Collider2D col;
    SpriteRenderer spRend;
    GameObject textHP;

    // References to unit sprites
    public Sprite blue_villager, blue_warrior, blue_armor, blue_archer;
    public Sprite red_villager, red_warrior, red_armor, red_archer;

    // Prefabs
    public GameObject MoveTile;
    public GameObject AtkTile;

    // Gameplay
    public enum State { WAIT, READY, MOVED, END }
    State state;
    Side side;

    // Stats
    public enum UType { Villager, Warrior, Armor, Archer }
    UType unitType;
    int HP = 100;
    int move;
    int range;

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

    public UType GetUType() { return this.unitType; }

    public int GetHP() { return HP; }
    public void SetHP(int _HP) { 
        HP = _HP;

        textHP.GetComponent<Text>().text = $"{HP}%";
    }

    public void SetState(State _state) { 
        state = _state;

        if (state != State.END)
            spRend.color = Color.white;
        else
            spRend.color = Color.gray;

        if (state == State.MOVED)
            drawAtkTiles();
    }

    public bool isRed() { return side == Side.RED; }
    public Side GetSide() { return side; }
    public bool isSameSide(UnitController other) { 
        return side == other.GetSide();
    }

    // -----------------------------------
    // Awake is called FIRST
    void Awake() {
        game = GameObject.FindWithTag("GameController");
        gC = game.GetComponent<GameController>();

        col = GetComponent<Collider2D>();
        spRend = GetComponent<SpriteRenderer>();

        if (transform.Find("Canvas/HP") == null)
            Debug.Log("NULL!!!!");

        textHP = transform.Find("Canvas/HP").gameObject;
    }

    void Start() {}

    public void Activate()
    {        
        switch (this.name)
        {
            case "blue_villager":
                unitType = UType.Villager;
                spRend.sprite = blue_villager;
                side = Side.BLUE;
                break;
            case "blue_warrior":
                unitType = UType.Warrior;
                spRend.sprite = blue_warrior;
                side = Side.BLUE;
                break;
            case "blue_armor":
                unitType = UType.Armor;
                spRend.sprite = blue_armor;
                side = Side.BLUE;
                break;
            case "blue_archer":
                unitType = UType.Archer;
                spRend.sprite = blue_archer;
                side = Side.BLUE;
                break;
            case "red_villager":
                unitType = UType.Villager;
                spRend.sprite = red_villager;
                side = Side.RED;
                break;
            case "red_warrior":
                unitType = UType.Warrior;
                spRend.sprite = red_warrior;
                side = Side.RED;
                break;
            case "red_armor":
                unitType = UType.Armor;
                spRend.sprite = red_armor;
                side = Side.RED;
                break;
            case "red_archer":
                unitType = UType.Archer;
                spRend.sprite = red_archer;
                side = Side.RED;
                break;
        }

        switch(unitType) {
            case UType.Villager:
                move = 3;
                range = 0;
                break;
            case UType.Warrior:
                move = 2;
                range = 1;
                break;
            case UType.Armor:
                move = 2;
                range = 1;
                break;
            case UType.Archer:
                move = 2;
                range = 2;
                break;
        }

        if (side == Side.RED)
            spRend.flipX = true;

        state = State.READY;
    }

    void Update() {}

    // public void HandleInput()
    // {
    //     if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
    //     {
    //         Touch touch = Input.touches[0];
    //         Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);

    //         if (col == Physics2D.OverlapPoint(touchPos))
    //         {
    //             isChosen = true;
    //         }
    //         else
    //         {
    //             isChosen = false;
    //         }
    //     }
    // }

    private void OnMouseUp()
    {
        if (!gC.isGameEnd() &&
            gC.getCurPlayer() == this.side &&
            this.state == State.READY)
        {
            this.destroyTiles("Both");

            this.initMoveTiles();
        }
    }


    // ---- Tiles ---------------------------

    public void initMoveTiles()
    {
        drawMoveTiles();
    }

    private void drawMoveTiles()
    {
        // TODO: CHANGE WITH FLOOD FILL!!!!

        drawMoveTile(x, y);

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

        MoveTile mvTile = mP.GetComponent<MoveTile>();
        mvTile.setUnitOwnMoveTile(this.gameObject);
        mvTile.setWorldPosition(x, y);
    }


    private void drawAtkTiles()
    {

        if (this.range == 1) {
            // draw left
            if (gC.isValidPos(x - 1, y) && 
                gC.GetUnitAt(x - 1, y) != null && 
                !isSameSide(gC.GetUnitAt(x - 1, y).GetComponent<UnitController>()))
                drawAtkTile(x - 1, y);
            // draw up
            if (gC.isValidPos(x + 1, y) && 
                gC.GetUnitAt(x + 1, y) != null &&
                !isSameSide(gC.GetUnitAt(x + 1, y).GetComponent<UnitController>()))
                drawAtkTile(x + 1, y);
            // draw right
            if (gC.isValidPos(x, y + 1) && 
                gC.GetUnitAt(x, y + 1) != null &&
                !isSameSide(gC.GetUnitAt(x, y + 1).GetComponent<UnitController>()))
                drawAtkTile(x, y + 1);
            // draw down
            if (gC.isValidPos(x, y - 1) && 
                gC.GetUnitAt(x, y - 1) != null &&
                !isSameSide(gC.GetUnitAt(x, y - 1).GetComponent<UnitController>()))
                drawAtkTile(x, y - 1);
        }

        else if (this.range == 2) {
            // draw diagonal left top
            if (gC.isValidPos(x - 1, y + 1) &&
                gC.GetUnitAt(x - 1, y + 1) != null &&
                !isSameSide(gC.GetUnitAt(x - 1, y + 1).GetComponent<UnitController>()))
                drawAtkTile(x - 1, y + 1);
            // draw diagonal right top
            if (gC.isValidPos(x + 1, y + 1) && 
                gC.GetUnitAt(x + 1, y + 1) != null &&
                !isSameSide(gC.GetUnitAt(x + 1, y + 1).GetComponent<UnitController>()))
                drawAtkTile(x + 1, y + 1);
            // draw diagonal left bot
            if (gC.isValidPos(x - 1, y - 1) && 
                gC.GetUnitAt(x - 1, y - 1) != null &&
                !isSameSide(gC.GetUnitAt(x - 1, y - 1).GetComponent<UnitController>()))
                drawAtkTile(x - 1, y - 1);
            // draw diagonal right bot
            if (gC.isValidPos(x + 1, y - 1) && 
                gC.GetUnitAt(x + 1, y - 1) != null &&
                !isSameSide(gC.GetUnitAt(x + 1, y - 1).GetComponent<UnitController>()))
                drawAtkTile(x + 1, y - 1);
        }
    }

    private void drawAtkTile(int x, int y)
    {
        GameObject aP = Instantiate(this.AtkTile, new Vector3(x, y, -3.0f), Quaternion.identity);

        AtkTile atkTile = aP.GetComponent<AtkTile>();
        atkTile.setUnitOwnAtkTile(this.gameObject);
        atkTile.setWorldPosition(x, y);
    }


    public void destroyTiles(string toDestroy)
    {
        if (toDestroy == "MoveTile" || toDestroy == "Both") {
            GameObject[] mvTiles = GameObject.FindGameObjectsWithTag("MoveTile");
            foreach (GameObject mvTile in mvTiles)
                Destroy(mvTile);
        }
        if (toDestroy == "AtkTile" || toDestroy == "Both") {
            GameObject[] atkTiles = GameObject.FindGameObjectsWithTag("AtkTile");
            foreach (GameObject atkTile in atkTiles)
                Destroy(atkTile);
        }
    }
}
