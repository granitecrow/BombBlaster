using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MapUtility;

public class PlayerController : MonoBehaviour {

    public GameObject bombPrefab;
    protected Board board;

    Vector2 dest = Vector3.zero;

    // player properties
    public float speed = 0.1f;
    public int flame;
    public int bomb;
    public bool canKick;
    public bool canThrow;
    public bool canPunch;
    public bool isInvicible;
    public bool hasDisease;
    public bool isAlive;

    public int droppedBomb; // how many bombs this bomberman have on the stage
    [HideInInspector] public int i; // position in tile land
    [HideInInspector] public int j;


    private List<Tile> collectedPowerups = new List<Tile>();
    private Animator animator;
    private Rigidbody2D rb2d;




    void Start () {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        board = Board.instance;
        isAlive = true;
        dest = transform.position;
        droppedBomb = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isAlive) return;
    

        // Move closer to Destination
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        // Check for Input if not moving
        if ((Vector2)transform.position == dest)
        {
            if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
                dest = (Vector2)transform.position + Vector2.up;
            if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
                dest = (Vector2)transform.position + Vector2.right;
            if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
                dest = (Vector2)transform.position - Vector2.up;
            if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
                dest = (Vector2)transform.position - Vector2.right;     
        }
        if (Input.GetKey(KeyCode.Space) && droppedBomb < bomb)
        {
            if (board.GetTile(i, j).isEmpty)
            {
                ((Bomb)board.AddTile(i, j, bombPrefab)).Init(i, j, flame, 200, this);
                droppedBomb += 1;
            }
        }

        // Animation Parameters
        Vector2 dir = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
        GetComponent<Animator>().SetFloat("DirY", dir.y);

        i = (int)Mathf.Round(transform.position.x);
        j = (int)Mathf.Round(transform.position.y);


    }

    bool valid(Vector2 dir)
    {
        // Cast Line from 'next to Player' to 'Player'
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        var isTile = hit.collider.transform.GetComponent<Tile>();
        if(hit.collider.transform.GetComponent<Tile>() != null)
        {
            return (hit.transform.GetComponent<Tile>().isWalkable);
        }
        else
            return (hit.collider == GetComponent<Collider2D>());
    }


    //if collide with powerup then collect
    //if collide with flame then die
    void OnTriggerEnter2D(Collider2D other)
    {
        //Check the provided Collider2D parameter other to see if it is tagged "PowerUp", if it is...
        if (other.gameObject.CompareTag("PowerUp"))
        {
            CollectItem(other.gameObject.GetComponent<PowerUp>());
            other.gameObject.SetActive(false);
        }
        if(other.gameObject.CompareTag("Flame"))
        {
            Die();
        }
    }

    private void CollectItem(PowerUp powerup)
    {
        collectedPowerups.Add(powerup);
        switch (powerup.code)
        {
            case PowerupCode.NULL:
                break;
            case PowerupCode.BOMB:
                if (bomb < Board.instance.MAX_BOMB)
                {
                    bomb++;
                }
                break;
            case PowerupCode.FLAME:
                if (flame < Board.instance.MAX_FLAME)
                {
                    flame++;
                }
                break;
            case PowerupCode.SPEED:
                if (speed < Board.instance.MAX_SPEED)
                {
                    speed += Board.instance.SPEED_INCREMENT;
                }
                break;
            case PowerupCode.PUNCH:
                canPunch = true;
                break;
            case PowerupCode.DISEASE:
                hasDisease = true;
                break;
            case PowerupCode.KICK:
                canKick = true;
                break;
            case PowerupCode.SUPER_FLAME:
                flame = Board.instance.MAX_FLAME;
                break;
            default:
                break;
        }
        powerup.Remove();
    }

    private void Die()
    {
        //play death animation
        //remove from board
        //spawn power-ups elsewhere
        isAlive = false;
        StartCoroutine(PlayDeath(4.0f));
        //Destroy(gameObject);
        board.PlayerDeath(this);
    }

    IEnumerator PlayDeath(float delayTime)
    {
        GetComponent<Animator>().SetBool("die", true);
        yield return new WaitForSeconds(delayTime);
        Destroy(gameObject);
    }
    //if collide with flame then die
}
