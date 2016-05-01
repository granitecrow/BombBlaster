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
    public PlayerController[] players;
    public static Tile[,] tiles;

    private static List<MapItem> powerups;

    [Range(0, 1)]
    public float BRICK_PERCENT;

    [Header("POWER UP")]
    public int BOMB;
    public int FLAME;
    public int SPEED;
    public int PUNCH;
    public int KICK;
    public int DISEASE;
    public int SUPER_FLAME;

    [Header("GAME CONSTANTS")]
    public int MAX_BOMB;
    public int MAX_FLAME;
    public float MAX_SPEED;
    public float SPEED_INCREMENT;
    



    // FIXME MonoBehaviour cannot be created with 'new' keyword. should use AddComponent()
    //private static Tile emptyTile = new Tile(0, 0, true);

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

        alivePlayerCount = playerCount;     // number of total real and AI players

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
        Tile tile = tileObj.GetComponent<Tile>();
        tiles[i, j] = tile;
        tile.Init(i, j);
        tileObj.transform.SetParent(instance.transform);
        return tileObj.GetComponent<Tile>();
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
        //int[] powerUpSettings = {
        //    BOMB,
        //    FLAME,
        //    SPEED,
        //    PUNCH,
        //    DISEASE,
        //    KICK,
        //    SUPER_FLAME
        //};

        //for (int p = 0; p < powerUpSettings.Length; p++)
        //{
        //    for (int i = 0; i < p; i++)
        //    {
        //        powers.Add(new MapItem(bricks[i + p].x, bricks[i + p].y, TileType.POWERUP, p + 1));
        //    }
        //}


        // please refactor
        // this hurts on so many levels you lazy bag of crap
        for (int b = 0; b < BOMB; b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.BOMB));
        }
        for (int b = BOMB; b < (BOMB+FLAME); b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.FLAME));
        }
        for (int b = (BOMB+FLAME); b < (BOMB + FLAME + SPEED); b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.SPEED));
        }
        for (int b = (BOMB + FLAME + SPEED); b < (BOMB + FLAME + SPEED + PUNCH); b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.PUNCH));
        }
        for (int b = (BOMB + FLAME + SPEED + PUNCH); b < (BOMB + FLAME + SPEED + PUNCH + DISEASE); b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.DISEASE));
        }
        for (int b = (BOMB + FLAME + SPEED + PUNCH + DISEASE); b < (BOMB + FLAME + SPEED + PUNCH + DISEASE + KICK); b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.KICK));
        }
        for (int b = (BOMB + FLAME + SPEED + PUNCH + DISEASE + KICK); b < (BOMB + FLAME + SPEED + PUNCH + DISEASE + KICK + SUPER_FLAME); b++)
        {
            powers.Add(new MapItem(bricks[b].x, bricks[b].y, TileType.POWERUP, (int)PowerupCode.SUPER_FLAME));
        }


        return powers;
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
                if ((x % 2 == 0) && (y % 2 == 0))
                {
                    newMap.Set(new MapItem(x, y, TileType.WALL, 0));
                }
            }
        }

        int totalPositions = newMap.rows * newMap.columns;
        int randomBricks = (int)(totalPositions * BRICK_PERCENT);
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

        newMap.Set(new MapItem(rows-2, 1, TileType.SPAWN, 0));
        newMap.Set(new MapItem(rows-3, 1, TileType.EMPTY, 0));
        newMap.Set(new MapItem(rows-2, 2, TileType.EMPTY, 0));

        return newMap;
    }
    

    public void PlayerDeath(PlayerController player)
    {
        alivePlayerCount--;
        if (alivePlayerCount <= 1)
        {
            GameManager.instance.EndGame();
        }
    }
}
