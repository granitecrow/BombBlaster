using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {

    //public GameObject gameManager;
    //public GameObject soundManager;

	// Use this for initialization
	void Awake() {
        //if (GameManager.instance == null)
        //    Instantiate(gameManager);
        //if (SoundManager.instance == null)
        //    Instantiate(soundManager);
	}

    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void GameOver()
    {
        enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
