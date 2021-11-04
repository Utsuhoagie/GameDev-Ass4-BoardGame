using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Turn
    enum Side { RED, BLUE }
    private Side curPlayer;
    private bool isGameOver = false;

    private GameObject[,] boardPos = new GameObject[8,8];
    private List<GameObject> redUnits = new List<GameObject>();
    private List<GameObject> blueUnits = new List<GameObject>();

    public GameObject unitPrefab;


    // ---------------------------------
    // Start is called before the first frame update
    void Start() {
        //unitObj.name = "blu_villager";
        CreateUnit("blue_villager", 0, 0);
        CreateUnit("blue_archer", 3, 3);
        CreateUnit("red_warrior", 1, 1);
        CreateUnit("red_armor", 2, 2);
        
        // for (int i = 0; i < 4; i++)
        //     Debug.Log($"BoardPos({i},{i}) has: bbbbbbb {boardPos[i,i]} bbbbbbbbbbbb");
    }

    // Update is called once per frame
    void Update() {
        
    }

    // --------------------------------

    void CreateUnit(string uName, int x, int y) {
        // world position, NOT board position
        var pos = new Vector3(x-3.5f, y-3.5f, -1);
        
        GameObject unitObj = Instantiate(unitPrefab, pos, Quaternion.identity);
        UnitController unit = unitObj.GetComponent<UnitController>();
        unit.name = uName;

        // make positions start at (0,0) at bottom left
        unit.SetX((int)(pos.x + 3.5f));  
        unit.SetY((int)(pos.y + 3.5f));

        unit.Activate();

        if (unit.isRed()) {
            redUnits.Add(unitObj);
        }
        else {
            blueUnits.Add(unitObj);
        }

        boardPos[unit.GetX(), unit.GetY()] = unitObj;
    }

    public bool isValidPos(int x, int y) {
        if (x < 0 || y < 0 || x >= 8 || y >= 8) 
            return false;
        else return true;
    }

    public GameObject GetUnitAt(int x, int y) {
        return boardPos[x,y];
    }

    public void SetUnitAt(int x, int y, GameObject unit) {
        
    }
}
