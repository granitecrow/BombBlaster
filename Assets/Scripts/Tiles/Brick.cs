using UnityEngine;
using System.Collections;

public class Brick : Tile {

    public override void Explode()
    {
        if (isExploding) return;
        isExploding = true;
        gameObject.SetActive(false);

        board.AddTile(i, j, destroyPrefab).Init(i, j);
        Remove();
    }

    public override void Remove()
    {
        // if this i,j location contains a power-up then instantiate else set to empty
        board.RemoveTile(this);
        Destroy(gameObject);
    }
}
