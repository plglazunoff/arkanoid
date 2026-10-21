using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static event Action OnBlocksCountEnded;
    public static event Action OnPlayerAttemptsCountEnded;
    public static event Action<int> OnPlayerAttemptsCountUpdated;
    public static event Action<int> OnBlocksCountUpdated;
    public static event Action<int> OnCurrentLevelIndexUpdated;

    private List<Block> _spawnedBlocks = new List<Block>();
    private int _playerAttemptsPerLevel = 10;
    private int _currentLevelIndex;

    private void Awake()
    {
        Ball.OnBallLivesUpdated += HandlePlayerAttemptsCount;

        OnCurrentLevelIndexUpdated?.Invoke(_currentLevelIndex = SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        StartCoroutine(ShowBlocksCount());

        OnPlayerAttemptsCountUpdated?.Invoke(_playerAttemptsPerLevel);
    }

    public void SetBlocks(List<Block> blocks)
    {
        _spawnedBlocks = blocks;
    }

    private void HandlePlayerAttemptsCount()
    {
        if (_playerAttemptsPerLevel > 0)
        {
            _playerAttemptsPerLevel--;
            OnPlayerAttemptsCountUpdated?.Invoke(_playerAttemptsPerLevel);
        }

        if (_playerAttemptsPerLevel == 0)
        {
            Debug.Log("Попытки закончились");
        }
    }

    private IEnumerator ShowBlocksCount()
    {
        OnBlocksCountUpdated?.Invoke(_spawnedBlocks.Count);

        yield return new WaitForEndOfFrame();

        while (_spawnedBlocks.Count > 0)
        {
            for (int i = _spawnedBlocks.Count - 1; i >= 0; i--)
            {
                if (_spawnedBlocks[i] == null)
                {
                    _spawnedBlocks.RemoveAt(i);
                    OnBlocksCountUpdated?.Invoke(_spawnedBlocks.Count);
                }
            }
            yield return null;
        }

        if (_spawnedBlocks.Count == 0)
        {
            OnBlocksCountEnded?.Invoke();
        }
    }

    private void OnDestroy()
    {
        Ball.OnBallLivesUpdated -= HandlePlayerAttemptsCount;
    }
}
