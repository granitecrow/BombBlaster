using UnityEngine;
using System.Collections;


public class Bomb : Tile
{
    public int DELAY; // delay between bomb and trigger
    public PlayerController player; // who dropped the bomb
    public bool isKicked = false;
    public Vector2 kickedDir = new Vector2(0, 0);
    public AudioClip explosionSound;

    private int flameSize;
    private int timer;
    private float speed = 10f;



    public void Init(int i, int j, int flameSize, int timer, PlayerController player)
    {
        base.Init(i, j);

        this.flameSize = flameSize;
        this.timer = timer;
        this.player = player;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isExploding) return;
        timer -= 1;
        if (timer < 0)
        {
            Explode();
            //Remove();
        }
        if (isKicked)
        {
            var nextSpot = kickedDir + (Vector2)transform.position;
            Tile tile = board.GetTile((int)nextSpot.x, (int)nextSpot.y);

            if (tile.isWalkable)
            {
                board.RemoveTile(this);
                transform.Translate(new Vector2(kickedDir.x * Time.deltaTime * speed, kickedDir.y * Time.deltaTime * speed));
                base.Init((int)nextSpot.x, (int)nextSpot.y);
            }
            else
            {
                board.SetTile(i,j, this);
                isKicked = false;
            }
        }
    }

    ////if collide with flame then explode
    //void OnCollisionEnter2D(Collider2D coll)
    //{
    //    if (coll.gameObject.CompareTag("Flame"))
    //    {
    //        Explode();
    //    }
    //}

    //if collide with flame then explode
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Flame"))
        {
            Explode();
        }
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public override void Explode()
    {
        if (isExploding) return;
        isExploding = true;
        if (player != null) player.droppedBomb -= 1;
        gameObject.SetActive(false);
        board.RemoveTile(this);

        SoundManager.instance.PlaySingle(explosionSound);
        this.gameObject.GetComponent<Explosion>().Init(i, j, flameSize);
        Remove();
    }
}
