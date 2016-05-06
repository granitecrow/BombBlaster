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

    private Vector2 dest = Vector2.zero;

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
        }
        if (isKicked)
        {
            dest = new Vector2((i + kickedDir.x), (j + kickedDir.y));

            if ((Vector2)transform.position == dest)
            {
                board.RemoveTile(this);
                base.Init((int)dest.x, (int)dest.y);
            }

            Tile tile = board.GetTile((int)dest.x, (int)dest.y);
            if (tile.isWalkable)
            {
                if (tile.GetComponent<Transform>().tag == "Flame")
                {
                    isKicked = false;
                    Explode();
                }
                transform.Translate(new Vector2(kickedDir.x * Time.deltaTime * speed, kickedDir.y * Time.deltaTime * speed));

            }
            else
            {
                board.SetTile(i, j, this);
                isKicked = false;
            }
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
        StartCoroutine(ExplosionCoroutine());
    }

    public IEnumerator ExplosionCoroutine()
    {
        for (int c = 0; c < DELAY; c++) yield return null;

        board.RemoveTile(this);

        SoundManager.instance.PlaySingle(explosionSound);
        this.gameObject.GetComponent<Explosion>().Init(i, j, flameSize);

        if (player != null) player.droppedBomb -= 1;

        Destroy(gameObject);
    }
}
