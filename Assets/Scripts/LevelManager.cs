using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public event Action OnBallLivesEnded;
    public int BallLives;
    public TMP_Text AttemptsText;
    public GameObject EndGamePanel;
    public GameObject WinGamePanel;
    public Button RestartButton;
    public Button NextLevelButton;
    public Button[] MainMenuButton;
    public GameObject AttemptsTexts;

    private Animator _animatorEndPanel;
    private Animator _animatorWinPanel;
    private List<Block> _blocks;

    public void Start()
    {
        StartCoroutine(GettingBlocks());
        AttemptsText.text = "Attempts: " + BallLives.ToString();
        RestartButton.onClick.AddListener(RestartGame);
        NextLevelButton.onClick.AddListener(NextLevelActivate);
        MainMenuButton[0].onClick.AddListener(MainMenuActivate);
        MainMenuButton[1].onClick.AddListener(MainMenuActivate);
        EndGamePanel.SetActive(false);
        WinGamePanel.SetActive(false);
        _animatorEndPanel = EndGamePanel.GetComponent<Animator>();
        _animatorWinPanel = WinGamePanel.GetComponent<Animator>();
    }
    private IEnumerator GettingBlocks()
    {
        yield return new WaitForEndOfFrame();
        _blocks = FindObjectsOfType<Block>().ToList();
        foreach (var block in _blocks)
        {
            block.OnBlockDestroyed += WinGameHandler;
        }
    }
    private void WinGameHandler(Block block)
    {
        block.OnBlockDestroyed -= WinGameHandler;
        _blocks.Remove(block);
        if (_blocks.Count == 0)
        {
            OnBallLivesEnded?.Invoke();
            WinGamePanel.SetActive(true);
            _animatorWinPanel.SetBool("win", true);
        }
    }
    public void RemoveLives(int value)
    {
        BallLives -= value;
    }
    private void Update()
    {
        AttemptsText.text = BallLives.ToString();

        if (BallLives == 0)
        {
            AttemptsTexts.SetActive(false);
            EndGamePanel.SetActive(true);
            _animatorEndPanel.SetBool("end", true);
            OnBallLivesEnded?.Invoke();
        }
    }

    #region MethodsGameManage
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void NextLevelActivate()
    {
        SceneManager.LoadScene(1);
    }
    private void MainMenuActivate()
    {
        SceneManager.LoadScene(2);
    }
    #endregion
}
