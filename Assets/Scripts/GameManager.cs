using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public GameObject boardPrefab;
    public AudioClip battleMusic;

    [Header("GAME CONSTANTS")]
    public int MAX_BOMB;
    public int MAX_FLAME;
    public float MAX_SPEED;
    public float SPEED_INCREMENT;


    private int level = 1;

    public int alivePlayerCount;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (Board.instance == null)
            Instantiate(boardPrefab);

        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        boardPrefab.GetComponent<Board>().SetupScene(level);
        SoundManager.instance.ChangeMusic(battleMusic);
    }

    public void PlayerDeath(PlayerController player)
    {
        alivePlayerCount--;
        if (alivePlayerCount <= 1)
        {
            StartCoroutine(GameOverCoroutine(4.0f));
        }
    }

    IEnumerator GameOverCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("GameOver");
    }


}
