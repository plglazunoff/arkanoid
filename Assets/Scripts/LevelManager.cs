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
        RestartButton.onClick.AddListener(RestartGame);
        NextLevelButton.onClick.AddListener(NextLevelActivate);
        MainMenuButton[0].onClick.AddListener(MainMenuActivate);
        MainMenuButton[1].onClick.AddListener(MainMenuActivate);
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
            _animatorWinPanel.SetBool("win", true);
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
