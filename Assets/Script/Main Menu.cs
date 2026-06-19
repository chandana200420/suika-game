using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;




public class MainMenu : MonoBehaviour
{

    public Slider LoadingBar;
    public GameObject StartButton;
    public GameObject CancelButtom;
    void Start()
    {
        LoadingBar.gameObject.SetActive(false);
    }
    public void OnStartButton()
    {
        StartButton.SetActive(false);
        LoadingBar.gameObject.SetActive(true);
        StartCoroutine(loadnextscene());

    }
    IEnumerator loadnextscene()
    {
        LoadingBar.value = 0;

        for (int i = 0; i <= 100; i++)
        {
            LoadingBar.value = i;
            yield return new WaitForSeconds(0.02f);
        }
        SceneManager.LoadScene("Main Menu");
    }

    public void OnCancelButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // works on Android
#endif
    }

}
