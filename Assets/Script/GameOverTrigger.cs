using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   


public class GameOverTrigger : MonoBehaviour
{
    [Header("Restart setting")]
    public float restartDelay = 2f;
    public float velocityThreshold = 0.1f;
    public float checkDelay = 1f;

    [Header("UI Reference")]
    public TextMeshProUGUI gameOverText;

    [Header("UI Buttons")]
    public Button restartButton;


    private bool gameOver = false;

    private void Start()
    {
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameOver)
            return;

        if (int.TryParse(other.tag, out int tagNumber))
        {
            membercloud member = other.GetComponent<membercloud>();
            if (member != null)
            {
                //❗ Ignore members still attached to cloud
                if (member.inthecloud == "y")
                    return;

                Rigidbody2D rb = other.attachedRigidbody;
                if (rb == null)
                    return;

                // ❗ IGNORE members that are still falling fast
                if (rb.velocity.y < -0.1f)
                    return;

                StartCoroutine(CheckForGameOver(other));
            }
        }
    }

    private IEnumerator CheckForGameOver(Collider2D other)
    {
        yield return new WaitForSeconds(checkDelay);

        if (other == null || gameOver)
            yield break;


        membercloud member = other.GetComponent<membercloud>();
        if (member == null)
            yield break;

        // ❗ STILL IGNORE if it was from the cloud
        if (member.inthecloud == "y")
            yield break;
        Rigidbody2D rb = other.attachedRigidbody;

        if (rb == null)
            yield break;

        if (rb.velocity.magnitude < velocityThreshold)
        {
            TriggerGameOver();
        }
    }

    /* private void TriggerGameOver()
     {
         if (gameOver)
             return;

         gameOver = true;
         Debug.Log("Game Over! Member stopped in trigger zone.");

         if (gameOverText != null)
         {
             gameOverText.gameObject.SetActive(true);
         }

         StartCoroutine(RestartGame(restartDelay));
     }*/
    private void TriggerGameOver()
    {
        if (gameOver)
            return;

        gameOver = true;
        Debug.Log("Game Over! Member stopped in trigger zone.");

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        if (restartButton != null)
            restartButton.gameObject.SetActive(true);  // 🔥 Show button

        Time.timeScale = 0f; // pause game
    }


    /*IEnumerator RestartGame(float delay)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f;
        Cloud.spawnedYet = "n";

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/
    public void RestartGameButton()
    {
        Time.timeScale = 1f;
        Cloud.spawnedYet = "n";
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
