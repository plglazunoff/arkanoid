using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _jumpForce;
    private Vector3 _reflectedDirection;
    [SerializeField] private Platform _platform;
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
    }
    private void StopBall()
    {
        _gameManager.OnBallLivesEnded -= StopBall;
        _jumpForce = 0;
    }
    private void Start()
    {
        _startBallPosition = transform.position;
        _startPlatformPosition = _platform.transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var point = collision.contacts[0].normal;
        _rigidbody.velocity = Vector3.Reflect(_reflectedDirection, point).normalized * _jumpForce;
        if (collision.gameObject.TryGetComponent<Block>(out var block))
        {
            block.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
            block.SetDamage();
        }
    }
    private void FixedUpdate()
    {
        _reflectedDirection = _rigidbody.velocity * _jumpForce;
    }
    private void Update()
    {
        if (_isBallOnPlatform)
        {
            transform.position = new Vector3(_platform.transform.position.x, _startBallPosition.y, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && _isBallOnPlatform)
        {
            _rigidbody.velocity = (Vector3.up + Vector3.right * Random.Range(-1f, 1f)).normalized * _jumpForce;
            _isBallOnPlatform = false;
        }
        transform.rotation = _startRotation;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int livesCountToRemove = 1;
        _gameManager.RemoveLives(livesCountToRemove);
        Respawn();
    }
    private void Respawn()
    {
        transform.position = _startBallPosition;
        _platform.transform.position = _startPlatformPosition;
        _rigidbody.velocity = Vector3.zero;
        _isBallOnPlatform = true;

    }
}
