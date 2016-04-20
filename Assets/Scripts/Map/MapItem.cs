using UnityEngine;
using System.Collections;

namespace MapUtility
{
    public class MapItem
    {
        public int x;
        public int y;
        public TileType type;
        public int powerupType;


        public MapItem(int x, int y, TileType type, int powerupType)
        {
            this.x = x;
            this.y = y;
            this.type = type;
            this.powerupType = powerupType;
        }

        public MapItem(MapItem item)
        {
            x = item.x;
            y = item.y;
            type = item.type;
            powerupType = item.powerupType;
        }

    }
}