using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public event Action<Block> OnBlockDestroyed;
    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private float _borderPositionY = -6f;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if(transform.position.y <= _borderPositionY)
        {
            OnBlockDestroyed?.Invoke(this);
            DestroyBlock();
        }
    }
    private void DestroyBlock()
    {
        Destroy(gameObject);
    }
    public void SetDamage()
    {
        _collider.enabled = false;
        _rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

}
