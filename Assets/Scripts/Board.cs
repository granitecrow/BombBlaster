using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using MapUtility;

public class Board : MonoBehaviour
{
    public static Board instance;

    public int columns;
    public int rows;
    public int playerCount;
    public int alivePlayerCount;

    public GameObject barrierTile;
    public GameObject outerWallTile;
    public GameObject[] powerupTiles;
    public GameObject emptyTile;
    public GameObject[] player;

    [Range(0, 1)]
    public float BRICK_PERCENT;

    [Header("POWER UP")]
    public int BOMB;
    public int FLAME;
    public int SPEED;
    public int PUNCH;
    public int KICK;
    public int SUPER_FLAME;

    [Header("GAME CONSTANTS")]
    public int MAX_BOMB;
    public int MAX_FLAME;
    public float MAX_SPEED;
    public float SPEED_INCREMENT;
    
    public static Tile[,] tiles;
    private static MapItem[] powerups;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void SetupScene(int level)
    {
        tiles = new Tile[rows, columns];
        var gameMap = new Map(rows, columns);
        gameMap = BuildMap(gameMap);
        powerups = CreatePowerUpPlacement(gameMap);

        MapItem[] spawnPoints = gameMap.Find(TileType.SPAWN, 0);

        // build map
        for (int y = 0; y < columns; y++)
        {
            for (int x = 0; x < rows; x++)
            {
                MapItem item = gameMap.Get(x, y);
                if (item.type == TileType.WALL)
                {
                    AddTile(x, y, outerWallTile).Init(x, y);
                }
                if (item.type == TileType.BRICK)
                {
                    AddTile(x, y, barrierTile).Init(x, y);
                }
                if (item.type == TileType.UNSET)
                {
                    AddTile(x, y, emptyTile).Init(x, y);
                }
                if (item.type == TileType.EMPTY)
                {
                    AddTile(x, y, emptyTile).Init(x, y);
                }
                if (item.type == TileType.SPAWN)
                {
                    AddTile(x, y, emptyTile).Init(x, y);
                }

            }
        }

        alivePlayerCount = player.Length;
        for (int playerCount = 0; playerCount < player.Length; playerCount++)
        {
            GameObject.Instantiate(player[playerCount], new Vector3(spawnPoints[playerCount].x, spawnPoints[playerCount].y, 0f), Quaternion.identity);
        }

        // TODO: modify the camera to match the size of the level
    }



    public Tile AddTile(int i, int j, GameObject tilePrefab)
    {
        if (tilePrefab == null)
        {
            return emptyTile.GetComponent<Tile>();
        }
        GameObject tileObj = (GameObject)Instantiate(tilePrefab, new Vector3(i, j, 0f), Quaternion.identity);
        Tile tile = tileObj.GetComponent<Tile>();
        tiles[i, j] = tile;
        tile.Init(i, j);
        return tileObj.GetComponent<Tile>();
    }

    public void SetTile(int i, int j, Tile tile)
    {
        tiles[i, j] = tile;
    }

    public void SetTileToEmpty(Tile tile)
    {

        int pos = Array.FindIndex(powerups, p => (p.x == tile.i) && (p.y == tile.j));

        if (pos > -1)
        {
            int poweruptype = powerups[pos].powerupType;
            AddTile(tile.i, tile.j, powerupTiles[poweruptype]);
            powerups[pos].x = -1;
        }
        else
            AddTile(tile.i, tile.j, emptyTile).Init(tile.i, tile.j);
    }


    public Tile GetTile(int i, int j)
    {
        return tiles[i, j];
    }

    private MapItem[] CreatePowerUpPlacement(Map gameMap)
    {

        // find all bricks tiles in the map
        MapItem[] bricks = gameMap.Find(TileType.BRICK, 0);

        // shuffle bricks array using Knuth shuffle algorithm        
        for (int t= 0;  t< bricks.Length; t++)
        {
            int r = (int)UnityEngine.Random.Range(t, bricks.Length);
            MapItem temp = bricks[t];
            bricks[t] = bricks[r];
            bricks[r] = temp;
        }

        // next bricks get powerups inside
        int[] powerUpSettings = {
            BOMB,
            FLAME,
            SPEED,
            PUNCH,
            KICK,
            SUPER_FLAME
        };


        for (int b = 0; b < BOMB; b++)
        {
            bricks[b].powerupType = (int)PowerupCode.BOMB;
        }
        
       
        return bricks;
    }


    public Map BuildMap(Map newMap)
    {
        // set outer walls
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (x == 0 | x == (rows - 1) || y == 0 || y == (columns - 1))
                {
                    newMap.Set(new MapItem(x, y, TileType.WALL, 0));
                }
            }
        }

        int totalPositions = newMap.rows * newMap.columns;
        int randomBricks = (int)(totalPositions * 0.7f);
        for (int i = 0; i < randomBricks; i++)
        {
            int randX = Random.Range(0, (newMap.rows - 1));
            int randY = Random.Range(0, (newMap.columns - 1));
            if (newMap.Get(randX, randY).type == TileType.UNSET)
            {
                newMap.Set(new MapItem(randX, randY, TileType.BRICK, 0));
            }
        }

        // add spawn position
        newMap.Set(new MapItem(1, 1, TileType.SPAWN, 0));
        newMap.Set(new MapItem(1, 2, TileType.EMPTY, 0));
        newMap.Set(new MapItem(2, 1, TileType.EMPTY, 0));

        return newMap;
    }
    //public Map LoadMap()
    //{

    //    int newMap[13,13] =
    //    {
    //        {new MapItem(, 1, 1, 1, 1, 1 }


    //    return newMap;
    //}

    public void PlayerDeath(PlayerController player)
    {
        alivePlayerCount--;
    }
}
