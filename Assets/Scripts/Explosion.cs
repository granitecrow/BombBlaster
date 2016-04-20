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

        List<Tile> flameList = new List<Tile>();

        AddFlame(flameList, center, i, j);

        bool tp = true;
        bool bttm = true;
        bool rght = true;
        bool lft = true;

        for (int z = 1; z < size; z++)
        {
            lft = lft && AddFlame(flameList, horizontal, i - z, j);
            rght = rght && AddFlame(flameList, horizontal, i + z, j);
            bttm = bttm && AddFlame(flameList, vertical, i, j - z);
            tp = tp && AddFlame(flameList, vertical, i, j + z);
        }

        lft = lft && AddFlame(flameList, left, i - size, j);
        rght = rght && AddFlame(flameList, right, i + size, j);
        bttm = bttm && AddFlame(flameList, bottom, i, j - size);
        tp = tp && AddFlame(flameList, top, i, j + size);
    }

    private bool AddFlame(List<Tile> flameArray, GameObject flameType, int i, int j)
    {
        Tile tile = board.GetTile(i, j);

        if (tile.isEmpty)
        {
            flameArray.Add(flameType.GetComponent<Tile>());
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
