using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour {

    public GameObject center;
    public GameObject horizontal;
    public GameObject vertical;
    public GameObject top;
    public GameObject bottom;
    public GameObject right;
    public GameObject left;

    protected Board board;

    public void Init(int i, int j, int size)
    {
        board = Board.instance;

        bool tp = true;
        bool bttm = true;
        bool rght = true;
        bool lft = true;

        AddFlame(center, i, j);

        for (int z = 1; z < size; z++)
        {
            lft = lft && AddFlame(horizontal, i - z, j);
            rght = rght && AddFlame(horizontal, i + z, j);
            bttm = bttm && AddFlame(vertical, i, j - z);
            tp = tp && AddFlame(vertical, i, j + z);
        }

        lft = lft && AddFlame(left, i - size, j);
        rght = rght && AddFlame(right, i + size, j);
        bttm = bttm && AddFlame(bottom, i, j - size);
        tp = tp && AddFlame(top, i, j + size);
    }

    private bool AddFlame(GameObject flameType, int i, int j)
    {
        Tile tile = board.GetTile(i, j);

        if (tile.isEmpty)
        {
            board.AddTile(i, j, flameType);
            return true;
        }

        if (tile.isExplodable)
        {
            tile.Explode();
        }

        return false;
    }

}
