using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    public static event Action OnButtonPressed;

    [Header("-----------Pause Buttons----------")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;

    private Vector3 _originalScale = Vector3.one;
    private Vector3 _targetScale = Vector3.one * 0.9f;
    private float _animationTime = 0.1f;

    private void Awake()
    {
        _restartButton.onClick.AddListener(RestartGame);
        _menuButton.onClick.AddListener(OpenMenuScene);
    }

    private void RestartGame()
    {
        _restartButton.transform.DOScale(_targetScale, _animationTime).OnComplete(() =>
        {
            _restartButton.transform.DOScale(_originalScale, _animationTime).OnComplete(() =>
            {
                OnButtonPressed?.Invoke();
                LoadingPanelsManager.OnPanelsStoppedMove += Restart;
            });
        });

        void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            LoadingPanelsManager.OnPanelsStoppedMove -= Restart;
        }
    }

    private void OpenMenuScene()
    {
        _menuButton.transform.DOScale(_targetScale, _animationTime).OnComplete(() =>
        {
            _menuButton.transform.DOScale(_originalScale, _animationTime).OnComplete(() =>
            {
                OnButtonPressed?.Invoke();
                LoadingPanelsManager.OnPanelsStoppedMove += OpenMenu;
            });
        });
        void OpenMenu()
        {
            SceneManager.LoadScene("MainMenu");
            _menuButton.transform.DOKill();
            LoadingPanelsManager.OnPanelsStoppedMove -= OpenMenu;
        }
    }
}
