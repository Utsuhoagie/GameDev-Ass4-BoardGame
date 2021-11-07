using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Terrain = MapTile.Terrain;

public class MapController : MonoBehaviour {
	public Tilemap terrainTilemap;

    public MapTile[,] mapTiles = new MapTile[8,8];

	public Dictionary<Vector3, MapTile> map;


    // Sprites for comparison
    public Sprite plains, forest, mountain, fort, blueFort, redFort, base_, baseFlag;



	void Awake() {
		// if (instance == null) 
		// 	instance = this;
		// else if (instance != this)
		// 	Destroy(gameObject);

		GetMapTiles();
	}

	// Use this for initialization
	void GetMapTiles() {
        terrainTilemap = GameObject.FindWithTag("MainTilemap").GetComponent<Tilemap>();

        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {


                // mapTiles[x,y].Instantiate(
                //     _terrain: Terrain.Plains
                // );
            }
        }

        // tiles = new Dictionary<Vector3, MapTile>();

		// foreach (Vector3Int pos in terrainTilemap.cellBounds.allPositionsWithin) {
		// 	var localPos = new Vector3Int(pos.x, pos.y, pos.z);

		// 	if (!terrainTilemap.HasTile(localPos)) continue;
			
        //     var tile = new MapTile {
		// 		LocalPos = localPos,
		// 		WorldPos = terrainTilemap.CellToWorld(localPos),
		// 		TileBase = terrainTilemap.GetTile(localPos),
		// 		TilemapMember = terrainTilemap,
		// 		Name = localPos.x + "," + localPos.y,
		// 		Cost = 1 // TODO: Change this with the proper cost from ruletile
		// 	};
			
		// 	tiles.Add(tile.WorldPos, tile);
		// }
	}

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosAtMouse = Vector3Int.FloorToInt(terrainTilemap.WorldToCell(mousePos));

            var tileAt = terrainTilemap.GetTile(cellPosAtMouse);

            Vector3Int offset = new Vector3Int(-4, -4, 0);
            Vector3Int offsetPos = cellPosAtMouse - offset;

            Debug.Log($"Mouse clicked on Tile {offsetPos}!");

            
            var tileSprite = terrainTilemap.GetSprite(cellPosAtMouse);
            //tileSprite;

            // if (terrainTilemap.color == Color.white) {
            //     terrainTilemap.color = Color.gray;
            // }
            // else if (terrainTilemap.color == Color.gray) {
            //     terrainTilemap.color = Color.white;
            // }
        }
    }
}