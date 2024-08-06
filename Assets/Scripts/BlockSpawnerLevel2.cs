using UnityEngine;

public class BlockSpawnerLevel2 : BlockSpawner
{
    private void Start()
    {
        SpawnBlocksParent();
    }

    protected override void SpawnBlocksParent()
    {
        base.SpawnBlocksParent();
        for (int row = 0; row < _rows; row++)
        {
            for (int column = 0; column < _columns; column++)
            {
                if (column % 2 == 0)
                {

                    continue;
                }
                if (row % 1 == 0)
                {
                    continue;
                }
                Vector3 blockPosition = _startSpawnPosition + new Vector3(column * _offsetX, row * _offsetY);
                GameObject block = Instantiate(_blockPrefab, blockPosition, Quaternion.identity, transform);
            }
        }
    }
}

/*
 
 
 
 
 
 Паша привет! 
 
 
 
 
 
 
 */
