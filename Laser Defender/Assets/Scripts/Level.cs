using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {

    [SerializeField] float deathDelay = 2f;

    public void LoadStartMenu() {
        SceneManager.LoadScene(0);
    }

    public void LoadGame() {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver() {
        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver() {
        yield return new WaitForSeconds(deathDelay);
        SceneManager.LoadScene("End Screen");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
