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

    private void Start()
    {
        SpawnGridBlocks();
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


}
