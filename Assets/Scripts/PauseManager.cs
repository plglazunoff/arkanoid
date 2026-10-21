using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using Mono.Cecil;

public class PauseManager : MonoBehaviour
{
    public static event Action OnIncreaseButtonPressed;
    public static event Action OnDecreaseButtonPressed;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _pauseButton;
    
    [SerializeField] private Button _increaseButton;
    [SerializeField] private Button _decreaseButton;
    [SerializeField] private RectTransform _pausePanel;

    private Vector3 _originalScale = Vector3.one;
    private Vector3 _targetScale = Vector3.one * 0.9f;
    private Vector3 _pausePanelStartPosition = new Vector3(280, 1000);
    private Vector3 _pausePanelEndPosition = new Vector3(280, -1000);
    private Vector3 _pausePanelMiddlePosition = new Vector3(280, 0);

    private float _animationTime = 0.1f;
    private float _slidingAnimationTime = 0.5f;

    private void Awake()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
        _pauseButton.onClick.AddListener(PauseGame);
        _increaseButton.onClick.AddListener(IncreaseBallValueForce);
        _decreaseButton.onClick.AddListener(DecreaseBallValueForce);
    }

    private void ResumeGame()
    {
        _resumeButton.transform.DOScale(_targetScale, _animationTime).OnComplete(() =>
        {
            _resumeButton.transform.DOScale(_originalScale, _animationTime).OnComplete(() =>
            {
                _resumeButton.transform.DOKill();
                _pausePanel.DOAnchorPos(_pausePanelEndPosition, _slidingAnimationTime).OnComplete(() =>
                {
                    _pausePanel.DOKill();
                    _pausePanel.anchoredPosition = _pausePanelStartPosition;
                    _pausePanel.gameObject.SetActive(false);
                    OnGameResumed?.Invoke();
                    _pauseButton.interactable = true;
                });
            });
        });
    }

    private void PauseGame()
    {
        OnGamePaused?.Invoke();
        _pauseButton.interactable = false;
        _pauseButton.transform.DOScale(_targetScale, _animationTime).OnComplete(() =>
        {
            _pauseButton.transform.DOScale(_originalScale, _animationTime).OnComplete(() =>
            {
                _pausePanel.anchoredPosition = _pausePanelStartPosition;
                _pausePanel.gameObject.SetActive(true);
                _pausePanel.DOAnchorPos(_pausePanelMiddlePosition, _slidingAnimationTime).OnComplete(() =>
                {
                    _pausePanel.DOKill();
                });

                _pauseButton.transform.DOKill();
            });
        });
    }

    private void IncreaseBallValueForce()
    {
        _increaseButton.transform.DOScale(_targetScale, _animationTime).OnComplete(() =>
        {
            _increaseButton.transform.DOScale(_originalScale, _animationTime).OnComplete(() =>
            {
                OnIncreaseButtonPressed?.Invoke();
                _increaseButton.transform.DOKill();
            });
        });
    }

    private void DecreaseBallValueForce()
    {
        _decreaseButton.transform.DOScale(_targetScale, _animationTime).OnComplete(() =>
        {
            _decreaseButton.transform.DOScale(_originalScale, _animationTime).OnComplete(() =>
            {
                OnDecreaseButtonPressed?.Invoke();
                _decreaseButton.transform.DOKill();
            });
        });
    }
}
