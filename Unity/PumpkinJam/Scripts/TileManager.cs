using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;

    Tilemap tilemap;

    [SerializeField] private TileBase NormalTile;
    [SerializeField] private TileBase UsedTile;
    [SerializeField] private TileBase StartTile;
    [SerializeField] private TileBase EndTile;
    [SerializeField] private TileBase EndTile2;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        GameObject currentMap = StageController.CurrentMap;
        if (currentMap) { tilemap = currentMap.GetComponent<Tilemap>(); }
    }
    public void SetTile(Vector3Int tilePos, string tileName)
    {
        if (tileName == "Normal")
        { tilemap.SetTile(tilePos, NormalTile); }
        else if (tileName == "Used")
        { tilemap.SetTile(tilePos, UsedTile); }
        else if (tileName == "Start")
        { tilemap.SetTile(tilePos, StartTile); }
        else if (tileName == "End")
        { tilemap.SetTile(tilePos, EndTile); }
        else if (tileName == "End2")
        { tilemap.SetTile(tilePos, EndTile2); }
    }
    public bool CheckTile(Vector3Int tilePos, string tileName)
    {
        bool result = false;

        void Check(TileBase tile)
        {
            if (tile == tilemap.GetTile(tilePos))
            { result = true; }
        }
        if (tileName == "Normal")
        { Check(NormalTile); }
        else if (tileName == "Used")
        { Check(UsedTile); }
        else if (tileName == "Start")
        { Check(StartTile); }
        else if (tileName == "End")
        { Check(EndTile); }
        return result;
    }

    public Vector3Int FindStartTile(GameObject theMap)
    {
        Tilemap processMap = theMap.GetComponent<Tilemap>();
        processMap.CompressBounds(); // To only read the tiles that we have painted
        foreach (var pos in processMap.cellBounds.allPositionsWithin)
        {
            Tile tile = processMap.GetTile<Tile>(pos);
            if (tile && tile == StartTile) { print(pos.ToString()); return pos; }
        };
        print("not found");
        return default;
    }

    public bool IsMapCleared()
    {
        Tilemap processMap = tilemap;
        processMap.CompressBounds(); // To only read the tiles that we have painted
        int amount = 0;
        foreach (var pos in processMap.cellBounds.allPositionsWithin)
        {
            Tile tile = processMap.GetTile<Tile>(pos);
            if  (tile && tile == NormalTile) { amount += 1; }
        }
        print("amount left: "+amount.ToString());
        if (amount == 0) { return true; }
        return false;
        
    }

}
