using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    // References
    Text blueText;
    Text redText;

    // Turn
    public enum Side { BLUE, RED }
    [SerializeField] private Side curPlayer;
    bool isGameOver = false;


    GameObject[,] boardPos = new GameObject[8, 8];
    List<GameObject> blueUnits = new List<GameObject>();
    List<GameObject> redUnits = new List<GameObject>();

    
    public GameObject unitPrefab;

    // --- Getters & Setters ------------------
    public GameObject GetUnitAt(int x, int y) { return boardPos[x, y]; }

    public void SetUnitAt(int x, int y, GameObject unit) { boardPos[x, y] = unit; }

    public void setPositionEmpty(int x, int y) { boardPos[x, y] = null; }

    public Side getCurPlayer() { return this.curPlayer; }

    // --- Main --------------------------
    // Awake is called FIRST
    void Awake() {
        curPlayer = Side.BLUE;
        blueText = GameObject.FindWithTag("BlueText").GetComponent<Text>();
        redText = GameObject.FindWithTag("RedText").GetComponent<Text>();
    }

    void Start()
    {
        // show text
        blueText.enabled = true;
        redText.enabled = false;

        CreateUnit("blue_villager", 2, 6);
        CreateUnit("blue_archer", 3, 5);
        CreateUnit("red_warrior", 5, 3);
        CreateUnit("red_armor", 6, 2);
    }

    void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            this.isGameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    // --------------------------------

    void CreateUnit(string uName, int x, int y)
    {
        // world position, NOT board position
        var pos = new Vector3(x, y, -1);

        GameObject unit = Instantiate(unitPrefab, pos, Quaternion.identity);
        UnitController uC = unit.GetComponent<UnitController>();
        uC.name = uName;

        uC.SetX((int)(pos.x));
        uC.SetY((int)(pos.y));

        uC.Activate();

        if (uC.isRed())
            redUnits.Add(unit);
        else
            blueUnits.Add(unit);

        boardPos[uC.GetX(), uC.GetY()] = unit;
    }

    // --- Turns & Game -----------------

    public void SetAllMovable(bool movable, bool isRed) {
        if (isRed) {
            foreach (GameObject redUnit in redUnits)
                redUnit.GetComponent<UnitController>().SetMovable(movable);
        }
        else {
            foreach (GameObject blueUnit in blueUnits)
                blueUnit.GetComponent<UnitController>().SetMovable(movable);
        }
    }

    public void StartBlueTurn()
    {
        if (curPlayer == Side.RED) {
            SetAllMovable(true, true);

            this.curPlayer = Side.BLUE;
            blueText.enabled = true;
            redText.enabled = false;

            foreach (GameObject blueUnit in blueUnits)
                blueUnit.GetComponent<UnitController>().SetMovable(true);
        }
    }

    public void StartRedTurn()
    {
        if (curPlayer == Side.BLUE) {
            SetAllMovable(true, false);

            this.curPlayer = Side.RED;
            redText.enabled = true;
            blueText.enabled = false;

            foreach (GameObject redUnit in redUnits)
                redUnit.GetComponent<UnitController>().SetMovable(true);
        }
    }

    public bool isGameEnd() { return this.isGameOver; }

    public void endGame(Side winner)
    {
        this.isGameOver = true;

        // TODO:
        GameObject.FindGameObjectWithTag("LeftText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("LeftText").GetComponent<Text>().text = $"{winner} wins!";
    }


    // --- Helpers ---------------
    public bool isValidPos(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 8 || y >= 8)
            return false;
        else return true;
    }
}
