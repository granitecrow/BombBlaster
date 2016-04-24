using UnityEngine;
using System.Collections;

public class Bomb : Tile {
    public int DELAY; // delay between bomb and trigger
    public PlayerController player; // who dropped the bomb

    private int flameSize;
    private int timer;

    public void Init(int i, int j, int flameSize, int timer, PlayerController player)
    {
        base.Init(i, j);

        this.flameSize = flameSize;
        this.timer = timer;
        this.player = player;
    }
	
	// Update is called once per frame
	void Update () {
        if (isExploding) return;
        timer -= 1;
        if (timer < 0)
        {
            Explode();
            //Remove();
        }
	}
    public override void Remove()
    {
        //board.SetTileToEmpty(this);
        Destroy(gameObject);
    }

    public override void Explode()
    {
        if (isExploding) return;
        isExploding = true;
        if (player != null) player.droppedBomb -= 1;
        gameObject.SetActive(false);
        board.SetTileToEmpty(this);

        this.gameObject.GetComponent<Explosion>().Init(i, j, flameSize);
        Remove();
    }
}
