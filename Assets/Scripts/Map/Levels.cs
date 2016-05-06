using UnityEngine;
using System.Collections;

namespace MapUtility
{
    public class Levels
    {
        public int rows = 15;
        public int columns = 15;

        
        public Map GetLevel(string mapName)
        {
            var mapBuilder = new Map(rows, columns);

            switch (mapName)
            {
                case "Level1":
                    mapBuilder = LoadLevelOne(); 
                    break;
                default:
                    break;
            }

            return mapBuilder;
        }

        private Map LoadLevelOne()
        {

            Map level = new Map(rows, columns);

            // set outer walls
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    if (x == 0 | x == (rows - 1) || y == 0 || y == (columns - 1))
                    {
                        level.Set(new MapItem(x, y, TileType.WALL, 0));
                    }
                    if ((x % 2 == 0) && (y % 2 == 0))
                    {
                        level.Set(new MapItem(x, y, TileType.WALL, 0));
                    }
                }
            }

            int totalPositions = level.rows * level.columns;
            int randomBricks = (int)(totalPositions * 0.8f);
            for (int i = 0; i < randomBricks; i++)
            {
                int randX = Random.Range(0, (level.rows - 1));
                int randY = Random.Range(0, (level.columns - 1));
                if (level.Get(randX, randY).type == TileType.UNSET)
                {
                    level.Set(new MapItem(randX, randY, TileType.BRICK, 0));
                }
            }

            // add spawn position
            level.Set(new MapItem(1, 1, TileType.SPAWN, 0));
            level.Set(new MapItem(1, 2, TileType.EMPTY, 0));
            level.Set(new MapItem(2, 1, TileType.EMPTY, 0));

            level.Set(new MapItem(rows - 2, 1, TileType.SPAWN, 0));
            level.Set(new MapItem(rows - 3, 1, TileType.EMPTY, 0));
            level.Set(new MapItem(rows - 2, 2, TileType.EMPTY, 0));

            return level;
        }
    }
}