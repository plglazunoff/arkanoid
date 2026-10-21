using System;
using UnityEngine;

public class BallBounceRegulator : MonoBehaviour
{
    public static event Action<float> OnValueChanged;
    public static event Action<bool> OnButtonPressed;

    private Ball _ball;

    private void Awake()
    {
        _ball = GetComponent<Ball>();

        OnValueChanged?.Invoke(_ball.JumpForce);

        PauseManager.OnIncreaseButtonPressed += Increase;
        PauseManager.OnDecreaseButtonPressed += Decrease;
    }

    private void Increase()
    {
        float upLimit = 20;
        if (_ball.JumpForce < upLimit)
        {
            _ball.JumpForce += 1;
            OnValueChanged?.Invoke(_ball.JumpForce);
            OnButtonPressed?.Invoke(true);
        }
    }

    private void Decrease()
    {
        float downLimit = 5;
        if (_ball.JumpForce > downLimit)
        {
            _ball.JumpForce -= 1;
            OnValueChanged?.Invoke(_ball.JumpForce);
            OnButtonPressed?.Invoke(false);
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnIncreaseButtonPressed -= Increase;
        PauseManager.OnDecreaseButtonPressed -= Decrease;
    }
}
