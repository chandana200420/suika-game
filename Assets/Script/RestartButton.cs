using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void ResetGame()
    {
        Time.timeScale = 1f;
        Cloud.spawnedYet = "n";

        if (GameManager.instance != null)
            GameManager.instance.ResetScore(); 

        SceneManager.LoadScene("Main Menu"); 
    }
}
