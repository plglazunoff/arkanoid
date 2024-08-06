using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] protected Vector3 _startSpawnPosition;
    [SerializeField] protected float _offsetX;
    [SerializeField] protected float _offsetY;
    [SerializeField] protected GameObject _blockPrefab;
    [SerializeField] protected int _rows;
    [SerializeField] protected int _columns;

    private void Start()
    {
        SpawnBlocksParent();
    }

    protected virtual void SpawnBlocksParent()
    {
        for(int row = 0; row < _rows; row++)
        {
            for(int column = 0; column < _columns; column++)
            {
                Vector3 blockPosition = _startSpawnPosition + new Vector3(column * _offsetX,  row * _offsetY);
                GameObject block = Instantiate(_blockPrefab, blockPosition, Quaternion.identity);
                SpriteRenderer render = block.GetComponent<SpriteRenderer>();
                Color blockColor = new Color(Random.value, Random.value, Random.value);
                render.color = blockColor;
            }
        }
    }
}
