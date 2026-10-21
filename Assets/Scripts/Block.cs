using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    public event Action<Block> OnBlockDestroyed;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;
    private float _borderPositionY = -6f;
    private Vector3 _savedDirection;
    private Tweener _fadeTween;
    private Tweener _reduceTween;

    private bool _isFallingDown = false;
    private bool _isFading = false;

    private void Awake()
    {
        PauseManager.OnGamePaused += PauseBlockBehaviour;
        PauseManager.OnGameResumed += ResumeBlockBehaviour;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = BlockSpriteManager.SelectedSprite;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>())
        {
            ReduceBlock();
        }
    }

    public void BlockFall()
    {
        _isFallingDown = true;
        _collider.enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
        _spriteRenderer.color = new Color(Random.value, Random.value, Random.value);

        StartCoroutine(DestroyBlock());

        IEnumerator DestroyBlock()
        {
            float destroyYPosition = -12;
            while (transform.position.y > destroyYPosition)
            {
                yield return null;
            }
            Destroy(gameObject);
        }
    }

    public void BlockFade()
    {
        _collider.isTrigger = true;
        _fadeTween = _spriteRenderer.DOFade(0, 0.5f).OnComplete(() =>
        {
            _fadeTween?.Kill();
            Destroy(gameObject);
        });
    }

    public void ReduceBlock()
    {
        _collider.isTrigger = true;
        _reduceTween = transform.DOScale(0.1f, 1.5f).OnComplete(() =>
        {
            Destroy(gameObject);
            transform.DOKill();
        });
    }

    private void PauseBlockBehaviour()
    {
        if (_isFallingDown)
        {
            _savedDirection = _rigidbody.linearVelocity;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (_fadeTween.IsActive() && _fadeTween != null)
        {
            _fadeTween.Pause();
        }

        if(_reduceTween.IsActive() && _reduceTween != null)
        {
            _reduceTween.Pause();
        }
    }

    private void ResumeBlockBehaviour()
    {
        if (_isFallingDown)
        {
            _rigidbody.constraints = RigidbodyConstraints2D.None;
            _rigidbody.linearVelocity = _savedDirection;
        }

        if (_fadeTween.IsActive() && _fadeTween != null)
        {
            _fadeTween.Play();
        }

        if (_reduceTween.IsActive() && _reduceTween != null)
        {
            _reduceTween.Play();
        }
    }

    private void OnDestroy()
    {
        PauseManager.OnGamePaused -= PauseBlockBehaviour;
        PauseManager.OnGameResumed -= ResumeBlockBehaviour;
    }
}
