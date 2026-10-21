using TMPro;
using UnityEngine;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _ballForceValueText;
    [SerializeField] private TMP_Text _ballForceAnimationText;
    [SerializeField]private TMP_Text _attemptsCountText;
    [SerializeField] private TMP_Text _blocksCountText;
    [SerializeField] private TMP_Text _levelIndexText;

    private RectTransform _ballForceAnimationTextTransform;

    private float _upperAnimationValueTextPos = 50;
    private float _middleAnimationValueTextPos = 0;
    private float _lowerAnimationValueTextPos = -50;
    private float _ballForceAnimationTextDuration = 0.1f;
    private float _ballForceAnimationTextScale = 0.3f;

    private void Awake()
    {
        _ballForceAnimationTextTransform = _ballForceAnimationText.GetComponent<RectTransform>();

        BallBounceRegulator.OnValueChanged += Show;
        BallBounceRegulator.OnButtonPressed += ActivateAnimationText;

        GameManager.OnBlocksCountUpdated += UpdateBlocksCountText;
        GameManager.OnPlayerAttemptsCountUpdated += UpdateAttemptsText;

        GameManager.OnCurrentLevelIndexUpdated += UpdateLevelText;
    }

    private void ActivateAnimationText(bool isRight)
    {
        string increaseSign = "+";
        string decreaseSign = "-";
        _ballForceAnimationText.text = $"{(isRight ? increaseSign : decreaseSign)}1";

        Sequence seq = DOTween.Sequence();
        if (isRight)
        {
            seq.Append(_ballForceAnimationTextTransform.DOAnchorPosY(_upperAnimationValueTextPos, _ballForceAnimationTextDuration))
                .Join(_ballForceAnimationTextTransform.DOScale(Vector3.one * _ballForceAnimationTextScale, _ballForceAnimationTextDuration)).OnComplete(() =>
                {
                    _ballForceAnimationText.text = "";
                    _ballForceAnimationTextTransform.anchoredPosition = new Vector3(_ballForceAnimationTextTransform.anchoredPosition.x, _middleAnimationValueTextPos);
                    _ballForceAnimationTextTransform.localScale = Vector3.one;
                    seq.Kill();
                });
        }
        else
        {
            seq.Append(_ballForceAnimationTextTransform.DOAnchorPosY(_lowerAnimationValueTextPos, _ballForceAnimationTextDuration))
                .Join(_ballForceAnimationTextTransform.DOScale(Vector3.one * _ballForceAnimationTextScale, _ballForceAnimationTextDuration)).OnComplete(() =>
                {
                    _ballForceAnimationText.text = "";
                    _ballForceAnimationTextTransform.anchoredPosition = new Vector3(_ballForceAnimationTextTransform.anchoredPosition.x, _middleAnimationValueTextPos);
                    _ballForceAnimationTextTransform.localScale = Vector3.one;
                    seq.Kill();
                });
        }
    }

    private void Show(float value)
    {
        _ballForceValueText.text = $"{value}";
    }

    private void UpdateAttemptsText(int value)
    {
        _attemptsCountText.text = $"{value}";
    }

    private void UpdateLevelText(int value)
    {
        _levelIndexText.text = $"{value}";
    }

    private void UpdateBlocksCountText(int value)
    {
        _blocksCountText.text = $"{value}";
    }

    private void OnDestroy()
    {
        BallBounceRegulator.OnValueChanged -= Show;
        BallBounceRegulator.OnButtonPressed -= ActivateAnimationText;
        GameManager.OnBlocksCountUpdated -= UpdateBlocksCountText;
        GameManager.OnBlocksCountUpdated -= UpdateAttemptsText;
        GameManager.OnCurrentLevelIndexUpdated -= UpdateLevelText;
    }
}
