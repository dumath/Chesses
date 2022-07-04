using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public const string DIFFICULT = "difficult";
    [SerializeField] private GameObject retryWindow;
    [SerializeField] private GameObject onRetryWindowButton;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject liteDifficult;
    [SerializeField] private GameObject hardDifficult;
    [SerializeField] private GameObject strategDifficult;
    private int difficult;
    // Start is called before the first frame update
    void Start()
    {
        difficult = 0;
        if(Application.loadedLevelName != "Game")
        {
            liteDifficult.SetActive(false);
            hardDifficult.SetActive(false);
            strategDifficult.SetActive(false);
        }
        else
            retryWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelectGameDifficult()
    {
        startButton.SetActive(false);
        liteDifficult.SetActive(true);
        hardDifficult.SetActive(true);
        strategDifficult.SetActive(true);
    }

    public void StartGame(int difficult)
    {
        PlayerPrefs.SetInt(DIFFICULT, difficult);
        Application.LoadLevel("Game");
    }

    public void OnRetryWindow()
    {
        onRetryWindowButton.SetActive(false);
        retryWindow.SetActive(true);
    }

    public void CloseRetryWindow()
    {
        onRetryWindowButton.SetActive(true);
        retryWindow.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
