using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    public static event Action OnBallLivesUpdated;
    public float JumpForce
    {
        get { return _jumpForce; }
        set { _jumpForce = value; }
    }

    [SerializeField] private float _jumpForce;
    [SerializeField] private Platform _platform;

    private Rigidbody2D _rigidbody;
    private Vector3 _reflectedDirection;
    private Vector3 _savedDirection;
    private bool _isBallOnPlatform = true;
    private Vector3 _startBallPosition;
    private Vector3 _startPlatformPosition;
    private Quaternion _startRotation;
    private LevelManager _gameManager;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _startRotation = transform.rotation;
        _gameManager = FindObjectOfType<LevelManager>();
        _gameManager.OnBallLivesEnded += StopBall;

        PauseManager.OnGamePaused += StopBallFlight;
        PauseManager.OnGameResumed += ResumeBallFlight;
    }

    private void StopBall()
    {
        _gameManager.OnBallLivesEnded -= StopBall;
        _jumpForce = 0;
    }

    private void StopBallFlight()
    {
        _savedDirection = _rigidbody.linearVelocity;
        _rigidbody.linearVelocity = Vector3.zero;
    }

    private void ResumeBallFlight()
    {
        _rigidbody.linearVelocity = _savedDirection.normalized * _jumpForce;
    }

    private void Start()
    {
        _startBallPosition = transform.position;
        _startPlatformPosition = _platform.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var point = collision.contacts[0].normal;
        _rigidbody.linearVelocity = Vector3.Reflect(_reflectedDirection, point).normalized * _jumpForce;
    }

    private void FixedUpdate()
    {
        _reflectedDirection = _rigidbody.linearVelocity * _jumpForce;
    }

    private void Update()
    {
        if (_isBallOnPlatform)
        {
            transform.position = new Vector3(_platform.transform.position.x, _startBallPosition.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && _isBallOnPlatform) // переписать запуск мяча по нажатию на экран в специальную зону 
        {
            _rigidbody.linearVelocity = (Vector3.up + Vector3.right * Random.Range(-1f, 1f)).normalized * _jumpForce;
            _isBallOnPlatform = false;
        }
        transform.rotation = _startRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RespawnBorder"))
        {
            Respawn();
            OnBallLivesUpdated?.Invoke();
        }
    }

    private void Respawn()
    {
        transform.position = _startBallPosition;
        _platform.transform.position = _startPlatformPosition;
        _rigidbody.linearVelocity = Vector3.zero;
        _isBallOnPlatform = true;
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= StopBallFlight;
        PauseManager.OnGameResumed -= ResumeBallFlight;
    }
}
