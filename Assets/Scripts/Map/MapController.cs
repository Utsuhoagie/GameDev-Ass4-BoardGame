using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Terrain = MapTile.Terrain;

public class MapController : MonoBehaviour {
	Tilemap terrainTilemap;

    MapTile[,] mapTiles = new MapTile[8,8];
    bool[,] isVisited = new bool[8,8];

	//public Dictionary<Vector3, MapTile> map;


    // Sprites for comparison
    public Sprite plains, forest, fort, blueFort, redFort, base_, BFlag, RFlag;
    List<Sprite> compareSprites = new List<Sprite>();

    // ---------------------------------------

	void Awake() {
        compareSprites.Add(plains);
        compareSprites.Add(forest);
        compareSprites.Add(fort);
        compareSprites.Add(blueFort);
        compareSprites.Add(redFort);
        compareSprites.Add(base_);
        compareSprites.Add(BFlag);
        compareSprites.Add(RFlag);

		GetMapTiles();

        ResetVisit();
	}

    // ---- Functions -----------------------------

	// Use this for initialization
	void GetMapTiles() {
        terrainTilemap = GameObject.FindWithTag("MainTilemap").GetComponent<Tilemap>();

        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Vector3 pos = new Vector3((float)x - 4.0f, (float)y - 4.0f, 0.0f);
                Vector3Int cellPos = Vector3Int.FloorToInt(pos);
                Sprite spriteAt = terrainTilemap.GetSprite(cellPos);
                
                Terrain tileTerrain = GetTerrainFromSprite(spriteAt);

                mapTiles[x,y] = (MapTile)ScriptableObject.CreateInstance("MapTile");
                mapTiles[x,y].Instantiate(tileTerrain);
            }
        }
	}

    Terrain GetTerrainFromSprite(Sprite sprite) {
        for (int i = 0; i < compareSprites.Count; i++)
            if (sprite == compareSprites[i])
                return (Terrain)i;

        return Terrain.Unmovable;
    }

    public float GetTerrainDef(int x, int y) {
        //Vector3Int cellPos = new Vector3Int(x - 4, y - 4, 0);
        MapTile currentTile = mapTiles[x,y];
        return currentTile.GetDef();
    }

    public int GetTerrainCost(int x, int y) {
        MapTile currentTile = mapTiles[x,y];
        return currentTile.GetCost();
    }

    public bool IsVisited(int x, int y) { return isVisited[x,y]; }
    public void Visit(int x, int y) { 
        isVisited[x,y] = true;
    }
    public void ResetVisit() {
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                isVisited[x,y] = false;        
    }




    void Update() {
        // if (Input.GetMouseButtonDown(0)) {
        //     Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //     Vector3Int cellPosAtMouse = Vector3Int.FloorToInt(terrainTilemap.WorldToCell(mousePos));

        //     var tileAt = terrainTilemap.GetTile(cellPosAtMouse);
        //     var spriteAt = terrainTilemap.GetSprite(cellPosAtMouse);

        //     Vector3Int offset = new Vector3Int(-4, -4, 0);
        //     Vector3Int offsetPos = cellPosAtMouse - offset;

        //     Debug.Log($"Mouse clicked on Tile {offsetPos} with sprite name = {spriteAt.name}!");

            
        //     var tileSprite = terrainTilemap.GetSprite(cellPosAtMouse);
        //     //tileSprite;

        //     // if (terrainTilemap.color == Color.white) {
        //     //     terrainTilemap.color = Color.gray;
        //     // }
        //     // else if (terrainTilemap.color == Color.gray) {
        //     //     terrainTilemap.color = Color.white;
        //     // }
        // }
    }


}