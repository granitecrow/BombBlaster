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

    public GameObject barrierTile;
    public GameObject outerWallTile;
    public GameObject[] powerupTiles;
    public GameObject emptyTile;
    public PlayerController[] players;
    public static Tile[,] tiles;

    private static List<MapItem> powerups;

    //[Range(0, 1)]
    //public float BRICK_PERCENT;

    [Header("POWER UP")]
    public int BOMB;
    public int FLAME;
    public int SPEED;
    public int PUNCH;
    public int KICK;
    public int DISEASE;
    public int SUPER_FLAME;


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
        var test = new Levels();
        var gameMap = test.GetLevel("Level" + level);
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

        GameManager.instance.alivePlayerCount = playerCount;     // number of total real and AI players

        for (int i = 0; i < players.Length; i++)
        {
            PlayerController player = (PlayerController)Instantiate(players[i], new Vector3(spawnPoints[i].x, spawnPoints[i].y, 0f), Quaternion.identity);
            player.Init(i);
        }

        Camera.main.transform.position = new Vector3(rows / 2, columns / 2, -10);
        Camera.main.orthographicSize = ((columns/0.8f) * 16) / (2 * 16);

    }

    public Tile AddTile(int i, int j, GameObject tilePrefab)
    {
        if (tilePrefab == null)
        {
            return emptyTile.GetComponent<Tile>();
        }
        GameObject tileObj = (GameObject)Instantiate(tilePrefab, new Vector3(i, j, 0f), Quaternion.identity);
        Tile tile = tiles[i,j] = tileObj.GetComponent<Tile>();
        tile.Init(i, j);
         tileObj.transform.SetParent(instance.transform);
        return tiles[i,j];
    }

    public void SetTile(int i, int j, Tile tile)
    {
        tiles[i, j] = tile;
    }

    public void RemoveTile(Tile tile)
    {

        int pos = powerups.FindIndex(p => (p.x == tile.i) && (p.y == tile.j));

        if (pos > -1)
        {
            Debug.Log("Found power-up at: " + tile.i + ", " + tile.j + " at index " + pos);

            int poweruptype = powerups[pos].powerupType;
            AddTile(tile.i, tile.j, powerupTiles[poweruptype]);
            powerups.Remove(powerups[pos]);
        }
        else
            AddTile(tile.i, tile.j, emptyTile).Init(tile.i, tile.j);
//            SetTile(tile.i, tile.j, emptyTile.GetComponent<Tile>());

            //Debug.Log("Removed tile: " + tile + "to i,j location: " + tile.i + ", " + tile.j);
    }

    public Tile GetTile(int i, int j)
    {
        return tiles[i, j];
    }

    private List<MapItem> CreatePowerUpPlacement(Map gameMap)
    {
        List<MapItem> powers = new List<MapItem>();
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
            DISEASE,
            KICK,
            SUPER_FLAME
        };

        for (int p = 0; p < powerUpSettings.Length; p++)
        {
            for (int i = 0; i < powerUpSettings[p]; i++)
            {
                if (p == 0)
                {
                    powers.Add(new MapItem(bricks[i].x, bricks[i].y, TileType.POWERUP, p));
                }
                else
                    powers.Add(new MapItem(bricks[i + powerUpSettings[p-1]].x, bricks[i + powerUpSettings[p - 1]].y, TileType.POWERUP, p));
            }
        }

        return powers;
    }

}
