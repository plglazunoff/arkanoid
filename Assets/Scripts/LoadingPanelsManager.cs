using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Random = UnityEngine.Random;

public class LoadingPanelsManager : MonoBehaviour
{
    public static event Action OnPanelsStoppedMove;

    [SerializeField] private RectTransform _canvas;
    [SerializeField] private RectTransform _upPanel;
    [SerializeField] private RectTransform _bottomPanel;
    [SerializeField] private RectTransform _leftPanel;
    [SerializeField] private RectTransform _rightPanel;
    [SerializeField] private Camera _targetCamera;

    private float _animationTime = 1f;
    private Vector2 _upPanelCenterPosition;
    private Vector2 _bottomPanelCenterPosition;
    private Vector2 _leftPanelCenterPosition;
    private Vector2 _rightPanelCenterPosition;

    private Vector2 _upPanelOutsidePosition;
    private Vector2 _bottomPanelOutsidePosition;
    private Vector2 _leftPanelOutsidePosition;
    private Vector2 _rightPanelOutsidePosition;

    private AnimateType _animateType;

    private enum AnimateType
    {
        Vertical,
        Horizontal
    }

    private void Awake()
    {
        ButtonsManager.OnButtonPressed += ChooseType;
    }

    private void ChooseType()
    {
        AnimateType[] type = ((AnimateType[])Enum.GetValues(typeof(AnimateType)));
        _animateType = type[Random.Range(0, type.Length)];
        SetType();
    }

    private void SetType()
    {
        switch (_animateType)
        {
            case AnimateType.Horizontal:
                AnimateHorizontalPanels();
                break;
            case AnimateType.Vertical:
                AnimateVerticalPanels();
                break;
        }
    }

    private void AnimateHorizontalPanels()
    {
        _leftPanel.DOKill();
        _rightPanel.DOKill();

        Vector3 cameraWorldCenter = _targetCamera.transform.position;
        cameraWorldCenter.z = 0;

        Vector2 screenPoint = _targetCamera.WorldToScreenPoint(cameraWorldCenter);
        Vector2 localCenterPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, screenPoint, _targetCamera, out localCenterPoint);

        float leftHalfWidth = _leftPanel.rect.width / 2;
        float rightHalfWidth = _rightPanel.rect.width / 2;

        _leftPanelCenterPosition = new Vector2(localCenterPoint.x - leftHalfWidth, localCenterPoint.y);
        _rightPanelCenterPosition = new Vector2(localCenterPoint.x + rightHalfWidth, localCenterPoint.y);

        float canvasWidth = _canvas.rect.width;

        _leftPanelOutsidePosition = new Vector2(localCenterPoint.x - canvasWidth, localCenterPoint.y);
        _rightPanelOutsidePosition = new Vector2(localCenterPoint.x + canvasWidth, localCenterPoint.y);

        Vector2 centerAnchor = new Vector2(0.5f, 0.5f);

        _leftPanel.anchorMin = centerAnchor;
        _leftPanel.anchorMax = centerAnchor;

        _rightPanel.anchorMin = centerAnchor;
        _rightPanel.anchorMax = centerAnchor;

        _leftPanel.pivot = centerAnchor;
        _rightPanel.pivot = centerAnchor;

        _leftPanel.anchoredPosition = _leftPanelOutsidePosition;
        _rightPanel.anchoredPosition = _rightPanelOutsidePosition;

        _leftPanel.DOKill();
        _rightPanel.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(_leftPanel.DOAnchorPos(_leftPanelCenterPosition, _animationTime))
            .Join(_rightPanel.DOAnchorPos(_rightPanelCenterPosition, _animationTime)).OnComplete(() =>
            {
                OnPanelsStoppedMove?.Invoke();
            });
    }

    private void AnimateVerticalPanels()
    {
        Vector3 cameraWorldCenter = _targetCamera.transform.position;
        cameraWorldCenter.z = 0;

        Vector2 screenPoint = _targetCamera.WorldToScreenPoint(cameraWorldCenter);
        Vector2 localCenterPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, screenPoint, _targetCamera, out localCenterPoint);

        float upHalfHeight = _upPanel.rect.height / 2;
        float bottomHalfHeight = _bottomPanel.rect.height / 2;

        _upPanelCenterPosition = new Vector2(localCenterPoint.x, localCenterPoint.y + upHalfHeight);
        _bottomPanelCenterPosition = new Vector2(localCenterPoint.x, localCenterPoint.y - bottomHalfHeight);

        float canvasHeight = _canvas.rect.height;

        _upPanelOutsidePosition = new Vector2(localCenterPoint.x, localCenterPoint.y + canvasHeight);
        _bottomPanelOutsidePosition = new Vector2(localCenterPoint.x, localCenterPoint.y - canvasHeight);

        Vector2 centerAnchor = new Vector2(0.5f, 0.5f);

        _upPanel.anchorMin = centerAnchor;
        _upPanel.anchorMax = centerAnchor;

        _bottomPanel.anchorMin = centerAnchor;
        _bottomPanel.anchorMax = centerAnchor;

        _upPanel.pivot = centerAnchor;
        _bottomPanel.pivot = centerAnchor;

        _upPanel.anchoredPosition = _upPanelOutsidePosition;
        _bottomPanel.anchoredPosition = _bottomPanelOutsidePosition;

        _upPanel.DOKill();
        _bottomPanel.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(_upPanel.DOAnchorPos(_upPanelCenterPosition, _animationTime))
            .Join(_bottomPanel.DOAnchorPos(_bottomPanelCenterPosition, _animationTime)).OnComplete(() =>
            {
                OnPanelsStoppedMove?.Invoke();
            });
    }

    private void OnDestroy()
    {
        ButtonsManager.OnButtonPressed -= ChooseType;
    }
}
