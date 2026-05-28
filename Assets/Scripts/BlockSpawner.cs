using Unity.VisualScripting;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;

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


    private void Start()
    {
        SpawnCircleBlocks();
    }

    public void SpawnGridBlocks()
    {
        for(int row = 0; row < _rows; row++)
        {
            for(int column = 0; column < _columns; column++)
            {
                Vector3 blockPosition = _startSpawnPosition + new Vector3(column * _offsetX,  row * _offsetY);
                GameObject block = Instantiate(_blockPrefab, blockPosition, Quaternion.identity);
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
            Instantiate(_blockPrefab, spawnPosition, Quaternion.identity);
        }
    }

}
