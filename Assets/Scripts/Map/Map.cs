using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapUtility
{
    public class Map
    {
        public int rows;
        public int columns;

        public MapItem[,] items;

        public Map(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            this.items = new MapItem[rows, columns];

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    this.items[x, y] = new MapItem(0, 0, TileType.UNSET, 0);
                }
            }
        }

        public void Remove(int x, int y)
        {
            this.items[x, y] = null;
        }

        public void Set(MapItem item)
        {
            this.items[item.x, item.y] = new MapItem(item.x, item.y, item.type, item.powerupType);
        }

        public MapItem Get(int x, int y)
        {
            return this.items[x, y];
        }

        public MapItem[] Find(TileType tileType, int powerupType = 0)
        {
            List<MapItem> arr = new List<MapItem>();
            for (int y = 0; y < columns; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    MapItem item = items[x, y];
                    if (item == null) continue;
                    if (item.type == tileType && item.powerupType == powerupType)
                    {
                        arr.Add(item);
                    }
                }
            }
            return arr.ToArray();
        }

    }
}