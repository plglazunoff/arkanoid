using UnityEngine;

public static class BlockSpriteManager
{
    public static Sprite SelectedSprite;

    public static void Initialize(Sprite[] sprites)
    {
        SelectedSprite = sprites[Random.Range(0, sprites.Length)];
    }
}
