﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Side = GameController.Side;
using Terrain = MapTile.Terrain;

public class UnitController : MonoBehaviour
{
    // References
    public GameObject game;
    GameController gC;
    Collider2D col;
    SpriteRenderer spRend;
    GameObject textHP;
    GameObject healImg;
    MapController mapCtrl;

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
    // bool isChosen = false;
    // bool isMoved = false;

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
    public Vector2Int GetPos() { return new Vector2Int(x, y); }
    public void UpdatePos()
    {
        var pos = new Vector3(x, y, -1);
        //isMoved = true;
        this.transform.position = pos;
    }

    public UType GetUType() { return this.unitType; }
    public int GetAtkRange() { return this.range; }

    public int GetHP() { return HP; }
    public void SetHP(int _HP) { 
        HP = _HP;

        textHP.GetComponent<Text>().text = $"{HP}%";
    }

    public void SetState(State _state) { 
        state = _state;

        if (state == State.END)
            mapCtrl.ResetVisit();

        if (state == State.END)
            spRend.color = new Color(0.4f, 0.4f, 0.4f, 1.0f);
        else if (state == State.MOVED)
            spRend.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
        else
            spRend.color = Color.white;

        if (state == State.MOVED)
            drawAtkTiles();
        
        if (state == State.READY) {
            //if (this.unitType == UType.Villager)
            //isMoved = false;

            bool onFort = (mapCtrl.GetTerrain(this.GetPos()) == Terrain.Fort);
            if (onFort) {
                SetHP(Mathf.Clamp(this.HP + 20, 0, 100));
                StartCoroutine(Heal());

            }
        }
    }
    IEnumerator Heal() {
        healImg.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        healImg.SetActive(false);
        yield break;
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

        textHP = transform.Find("Canvas/HP").gameObject;
        healImg = transform.Find("Canvas/Heal").gameObject;

        mapCtrl = GameObject.FindWithTag("MapController").GetComponent<MapController>();
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

    public void HandleInput()
    {
        // if ((Input.touchCount > 0) && (Input.touches[0].phase == TouchPhase.Began))
        // {
        //     Touch touch = Input.touches[0];
        //     Vector2 touchPos = (Vector2)Camera.main.ScreenToWorldPoint(touch.position);

        //     if (col == Physics2D.OverlapPoint(touchPos))
        //     {
        //         isChosen = true;
        //     }
        //     else
        //     {
        //         isChosen = false;
        //     }
        // }
    }

    private void OnMouseUp()
    {
        if (!gC.isGameEnd() &&
            gC.getCurPlayer() == this.side &&
            this.state == State.READY)
        {
            this.destroyTiles("Both");

            this.initMoveTiles();
        }


        // Vector3 mousePos = Input.mousePosition;
        // Vector2 worldPos = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);

        // if (col == Physics2D.OverlapPoint(worldPos) &&
        //     !gC.isGameEnd() &&
        //     gC.getCurPlayer() == this.side &&
        //     this.state == State.READY &&
        //     !this.isChosen)
        //     {
        //         Debug.Log($"{this.name} clicked");

        //         this.isChosen = true;

        //         this.destroyTiles("Both");
        //         this.initMoveTiles();
        //     }

        // else {
        //     Debug.Log($"{this.name} unclicked");
        //     this.isChosen = false;
        //     if (this.isMoved) {
        //         Debug.Log("move destroy");
        //         this.destroyTiles("MoveTile");
        //     }
        //     else {
        //         Debug.Log("both destroy");
        //         this.destroyTiles("Both");
        //     }
        // }
    }

    public void Die() {
        gC.RemoveUnitFromGame(this.gameObject);
        Destroy(this.gameObject);
    }

    // ---- Tiles ---------------------------

    public void initMoveTiles()
    {
        Vector2Int thisPos = new Vector2Int(this.x, this.y);

        mapCtrl.ResetVisit();
        FloodFill(thisPos, this.move);
    }


    private void FloodFill(Vector2Int pos, int moveRemaining) {
        int costOfTile = mapCtrl.GetTerrainCost(pos);

        if (moveRemaining == 0 || moveRemaining < costOfTile || (mapCtrl.IsVisited(pos) && pos != this.GetPos())) 
            return;

        

        if (gC.isValidPos(pos) && 
            (gC.GetUnitAt(pos) == null || gC.GetUnitAt(pos) == this.gameObject))
            
            drawMoveTile(pos.x, pos.y);

        mapCtrl.Visit(pos.x, pos.y);

        if (pos != new Vector2Int(this.x, this.y))
            moveRemaining -= costOfTile;


        Vector2Int posL = new Vector2Int(pos.x - 1, pos.y);
        Vector2Int posR = new Vector2Int(pos.x + 1, pos.y);
        Vector2Int posU = new Vector2Int(pos.x, pos.y + 1);
        Vector2Int posD = new Vector2Int(pos.x, pos.y - 1);

        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(posL); q.Enqueue(posR); q.Enqueue(posU); q.Enqueue(posD); 

        while (q.Count > 0) {
            Vector2Int posToVisit = q.Dequeue();

            if (gC.isValidPos(posToVisit) && !mapCtrl.IsVisited(posToVisit) && 
               (gC.GetUnitAt(pos) == null || isSameSide(gC.GetUnitAt(pos).GetComponent<UnitController>())))
                {
                    FloodFill(posToVisit, moveRemaining);
                }
        }
    }

    // private void drawMoveTiles()
    // {
    //     // TODO: CHANGE WITH FLOOD FILL!!!!

    //     drawMoveTile(x, y);

    //     for (int i = 1; i <= move; i++)
    //     {
    //         // draw left
    //         if (gC.isValidPos(x - i, y) && gC.GetUnitAt(x - i, y) == null)
    //             drawMoveTile(x - i, y);
    //         // draw up
    //         if (gC.isValidPos(x + i, y) && gC.GetUnitAt(x + i, y) == null)
    //             drawMoveTile(x + i, y);
    //         // draw right
    //         if (gC.isValidPos(x, y + i) && gC.GetUnitAt(x, y + i) == null)
    //             drawMoveTile(x, y + i);
    //         // draw down
    //         if (gC.isValidPos(x, y - i) && gC.GetUnitAt(x, y - i) == null)
    //             drawMoveTile(x, y - i);

    //         // draw diagonal left top
    //         if (i >= 2 && gC.isValidPos(x - (i - 1), y + (i - 1)) && gC.GetUnitAt(x - (i - 1), y + (i - 1)) == null)
    //             drawMoveTile(x - (i - 1), y + (i - 1));
    //         // draw diagonal right top
    //         if (i >= 2 && gC.isValidPos(x + (i - 1), y + (i - 1)) && gC.GetUnitAt(x + (i - 1), y + (i - 1)) == null)
    //             drawMoveTile(x + (i - 1), y + (i - 1));
    //         // draw diagonal left bot
    //         if (i >= 2 && gC.isValidPos(x - (i - 1), y - (i - 1)) && gC.GetUnitAt(x - (i - 1), y - (i - 1)) == null)
    //             drawMoveTile(x - (i - 1), y - (i - 1));
    //         // draw diagonal right bot
    //         if (i >= 2 && gC.isValidPos(x + (i - 1), y - (i - 1)) && gC.GetUnitAt(x + (i - 1), y - (i - 1)) == null)
    //             drawMoveTile(x + (i - 1), y - (i - 1));
    //     }
    // }

    private void drawMoveTile(int x, int y)
    {
        GameObject mP = Instantiate(this.MoveTile, new Vector3(x, y, -3.0f), Quaternion.identity);

        MoveTile mvTile = mP.GetComponent<MoveTile>();
        mvTile.setUnitOwnMoveTile(this.gameObject);
        mvTile.setWorldPosition(x, y);
    }


    private void drawAtkTiles()
    {

        // draw left
        if (gC.isValidPos(x - this.range, y) && 
            gC.GetUnitAt(x - this.range, y) != null && 
            !isSameSide(gC.GetUnitAt(x - this.range, y).GetComponent<UnitController>()))
            drawAtkTile(x - this.range, y);
        // draw up
        if (gC.isValidPos(x + this.range, y) && 
            gC.GetUnitAt(x + this.range, y) != null &&
            !isSameSide(gC.GetUnitAt(x + this.range, y).GetComponent<UnitController>()))
            drawAtkTile(x + this.range, y);
        // draw right
        if (gC.isValidPos(x, y + this.range) && 
            gC.GetUnitAt(x, y + this.range) != null &&
            !isSameSide(gC.GetUnitAt(x, y + this.range).GetComponent<UnitController>()))
            drawAtkTile(x, y + this.range);
        // draw down
        if (gC.isValidPos(x, y - this.range) && 
            gC.GetUnitAt(x, y - this.range) != null &&
            !isSameSide(gC.GetUnitAt(x, y - this.range).GetComponent<UnitController>()))
            drawAtkTile(x, y - this.range);

        if (this.range == 2) {
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
