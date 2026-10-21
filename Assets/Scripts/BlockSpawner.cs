using UnityEngine;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private Sprite[] _sprites = new Sprite[] { };

    [Header("----------Grid Spawner----------")]
    [SerializeField] private Vector3 _startSpawnPosition;
    [SerializeField] private float _offsetX;
    [SerializeField] private float _offsetY;
    [SerializeField] private int _rows;
    [SerializeField] private int _columns;

    [Header("----------Cirlce Spawner----------")]
    [SerializeField] private Vector3 _startSpawnCirclePosition;
    [SerializeField] private int _numberOfCircleBlocks;
    [SerializeField] private float _radius;

    [Header("----------W Spawner----------")]
    [SerializeField] private float _wOffsetX;
    [SerializeField] private float _wOffsetY;
    [SerializeField] private Vector3 _startSpawnWPosition;
    private int[,] _letterW =
    {
        {1, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1},
        {1, 0, 0, 0, 0, 1},
        {1, 0, 1, 1, 0, 1},
        {1, 1, 0, 0, 1, 1},
        {1, 0, 0, 0, 0, 1},
    };

    private List<Block> _spawnedBlocks = new List<Block>();
    private GameManager _gameManager;

    private void Awake()
    {
        BlockSpriteManager.Initialize(_sprites);
        _gameManager = FindAnyObjectByType<GameManager>();
    }


    private void Start()
    {
        SpawnWBlocks();
    }

    public void SpawnGridBlocks()
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                Vector3 blockPosition = _startSpawnPosition + new Vector3(column * _offsetX, row * _offsetY);
                var block = Instantiate(_blockPrefab, blockPosition, Quaternion.identity);
                _spawnedBlocks.Add(block);
                _gameManager.SetBlocks(_spawnedBlocks);
            }
        }
    }

    private void SpawnCircleBlocks()
    {
        float angle = 360 / _numberOfCircleBlocks;
        for (int i = 0; i < _numberOfCircleBlocks; i++)
        {
            float angleRad = i * angle * Mathf.Deg2Rad;
            float x = _radius * Mathf.Cos(angleRad);
            float y = _radius * Mathf.Sin(angleRad);
            Vector3 spawnPosition = _startSpawnCirclePosition + new Vector3(x, y);
            var block = Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
            _spawnedBlocks.Add(block);
            _gameManager.SetBlocks(_spawnedBlocks);
        }
    }

    private void SpawnWBlocks()
    {
        for (int y = 0; y < _letterW.GetLength(0); y++)
        {
            for (int x = 0; x < _letterW.GetLength(1); x++)
            {
                if (_letterW[y, x] == 1)
                {
                    Vector3 spawnPosition = _startSpawnWPosition + new Vector3(x * _wOffsetX, -y * _wOffsetY);
                    var blockInstance = Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
                    _spawnedBlocks.Add(blockInstance);
                    _gameManager.SetBlocks(_spawnedBlocks);
                    
                        blockInstance.GetComponent<SpriteRenderer>().sortingOrder -= 1;
                }
            }
        }
    }

}
