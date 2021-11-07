using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTile : Tile {

    public string customName = "This is my custom tile!";
    
    public enum Terrain { Plains, Forest, Mountain, Fort, BlueFort, RedFort, Base, BaseFlag }
    Terrain terrain;
    
    int def;
    int gold;
    int mvCost;
    Sprite _sprite;

    // Below is needed for Breadth First Searching
    public bool IsExplored { get; set; }

    public MapTile ExploredFrom { get; set; }

    // ---- Getters & Setters -----------------------



    // ---- Main ----------------------
    public void Instantiate(Terrain _terrain) {
        terrain = _terrain;

        switch(terrain) {
            case Terrain.Plains:
                def = 0;
                mvCost = 1;
                break;
            case Terrain.Forest:
                mvCost = 2;
                def = 1;
                break;
            case Terrain.Mountain:
                mvCost = 100;
                def = 0;
                break;
            case Terrain.Fort:
            case Terrain.BlueFort:
            case Terrain.RedFort:
                mvCost = 1;
                def = 2;
                break;
            case Terrain.Base:
                mvCost = 1;
                def = 3;
                break;
            case Terrain.BaseFlag:
                mvCost = 1;
                def = 0;
                break;
        }

        // TODO: gold for building
        if (terrain == Terrain.Base) {

        }
    }

}
