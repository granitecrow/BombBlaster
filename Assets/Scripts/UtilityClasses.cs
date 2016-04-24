using System.Collections;

namespace MapUtility
{
    public enum TileType
    {
        UNSET,
        EMPTY,
        WALL,
        BRICK,
        SPAWN,
        FLAME,
        POWERUP
    }

    public enum PowerupCode
    {
        NULL,
        BOMB,
        FLAME,
        SPEED,
        PUNCH,
        DISEASE,
        KICK,
        SUPER_FLAME
    }

}