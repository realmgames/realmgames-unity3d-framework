using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames.TileSystem
{
    public interface ITilemapData
    {
        Vector2Int Size
        {
            get;
        }

        byte[] Data
        {
            get;
        }

        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        bool HasBlock(int x, int y);
        bool HasBlock(Vector2Int pos);

        int SpriteIndex(int x, int y);
        int SpriteIndex(Vector2Int pos);

        byte GetMapValue(Vector2Int pos);
        byte GetMapValue(int x, int y);

        bool InBounds(Vector2Int pos);
        bool InBounds(int x, int y);
    }
}