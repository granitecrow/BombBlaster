using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public GameObject boardPrefab;
    private int level = 1;

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
    }

    public void EndGame()
    {
        StartCoroutine(WaitToLoadEndScreen(4.0f));
    }

    IEnumerator WaitToLoadEndScreen(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene("GameOver");
    }


}
