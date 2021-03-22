using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames.TileSystem
{
    [Serializable]
    public class TilemapData
    {
        public int width;
        public int height;
        public byte[] map;

        public Vector2Int Size
        {
            get
            {
                return new Vector2Int(width, height);
            }
        }

        public bool HasBlock(int x, int y)
        {
            return map[x + (y * width)] > 0;
        }

        public bool HasBlock(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= width)
                return false;

            if (pos.y < 0 || pos.y >= height)
                return false;

            return map[pos.x + (pos.y * width)] > 0;
        }

        public int SpriteIndex(int x, int y)
        {
            return (int)map[x + (y * width)] - 1;
        }

        public int SpriteIndex(Vector2Int pos)
        {
            return (int)map[pos.x + (pos.y * width)] - 1;
        }

        public byte GetMapValue(Vector2Int pos)
        {
            return map[pos.x + (pos.y * width)];
        }

        public byte GetMapValue(int x, int y)
        {
            return map[x + (y * width)];
        }

        public bool InBounds(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= width)
                return false;

            if (pos.y < 0 || pos.y >= height)
                return false;

            return true;
        }

        public bool InBounds(int x, int y)
        {
            if (x < 0 || x >= width)
                return false;

            if (y < 0 || y >= height)
                return false;

            return true;
        }
    }
}