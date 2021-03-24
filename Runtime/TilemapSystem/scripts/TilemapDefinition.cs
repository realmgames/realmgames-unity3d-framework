using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RealmGames.TileSystem
{
    [CreateAssetMenu(fileName = "tilemap", menuName = "Tilemap Definition")]
    public class TilemapDefinition : ScriptableObject, ITilemapData
    {
        public int width;
        public int height;
        public int blockCount;
        public byte[] map;

        public byte[] Data
        {
            get
            {
                return map;
            }
        }

        public Vector2Int Size
        {
            get
            {
                return new Vector2Int(width, height);
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
        }

        public void ExportJson ()
        {
            TilemapData data = new TilemapData();

            data.width = width;
            data.height = height;
            data.map = map;

            string json_string = JsonUtility.ToJson(data, true);

            File.WriteAllText(Application.dataPath + "/Resources/level.json", json_string);
        }

        public int CountBlocks()
        {
            int m_blockCount = 0;
            foreach (byte m in map)
            {
                if (m > 0)
                    m_blockCount++;
            }
            return m_blockCount;
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