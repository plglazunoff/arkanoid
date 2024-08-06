using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Transform _transform;
    private Camera _camera;
    private LevelManager _gameManager;
    private bool _canNotMove = false;

    public float Speed;
    public float BorderPositionX;
    private void Awake()
    {
        _transform = GetComponent<Transform>();
        _camera = Camera.main;
        Cursor.visible = false;
        _gameManager = FindObjectOfType<LevelManager>();
        _gameManager.OnBallLivesEnded += StopPlatform;
    }
    private void StopPlatform()
    {
        _gameManager.OnBallLivesEnded -= StopPlatform;
        _canNotMove = true;
        Cursor.visible = true;
    }

    private void Update()
    {
        if (_canNotMove)
        {
            return;
        }

        float mousePosition = Input.mousePosition.x;
        Vector3 mousePositionOnScreen = _camera.ScreenToWorldPoint(new Vector3(mousePosition,0,0));
        _transform.position = new Vector3(mousePositionOnScreen.x, _transform.position.y, _transform.position.z);
        //if (_transform.position.x > BorderPositionX)
        //{
        //    _transform.position = new Vector2(BorderPositionX, _transform.position.y);
        //}

        //if (_transform.position.x < -BorderPositionX)
        //{
        //    _transform.position = new Vector2(-BorderPositionX, _transform.position.y);
        //}
        float clampX = Mathf.Clamp(_transform.position.x, -BorderPositionX, BorderPositionX);
        _transform.position = new Vector2(clampX, _transform.position.y);
    }
}
