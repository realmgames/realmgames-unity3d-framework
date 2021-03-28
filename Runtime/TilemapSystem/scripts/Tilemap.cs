using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RealmGames.TileSystem
{
    [Serializable]
    public class GameBoardGeneratedEvent : UnityEvent<Tilemap> { }

    public class Tilemap : MonoBehaviour
    {
        public Tileset tileset;
        public GameObject boardTemplate;
        public Vector2 tileSize = Vector2.one;
        //public CameraController cameraController;
        public Vector2Int margin = Vector2Int.zero;
        public Tile[] m_tiles;
        public GameBoardGeneratedEvent gameBoardGenerated;

        private Vector2Int m_invalidPosition = new Vector2Int(-1, -1);
        //private int m_boardLayerMask;
        private int m_width, m_height;
        private ITilemapData m_tileMapData;

        public int Width
        {
            get
            {
                return (m_tileMapData != null) ? m_tileMapData.Width : 0;
            }
        }

        public int Height
        {
            get
            {
                return (m_tileMapData != null) ? m_tileMapData.Height : 0;
            }
        }

        public Tile[] Slots
        {
            get
            {
                return m_tiles;
            }
        }

        public Tile GetSlot(int x, int y)
        {
            return m_tiles[x + (y * m_width)];
        }

        public Vector2 CursorPosition
        {
            get
            {
#if UNITY_STANDALONE_OSX || UNITY_WEBGL
                Vector2 offset = new Vector2(0, 0);
#else
                Vector2 offset = new Vector2(0, 0);
#endif
                return MouseUtility.MousePositionOnPlaneVec2(transform, Camera.main) + offset;
            }
        }

        public Vector2 CursorPositionNoOffset
        {
            get
            {
                return MouseUtility.MousePositionOnPlaneVec2(transform, Camera.main);
            }
        }

        // Use this for initialization
        //void Awake()
        //{
        //    m_boardLayerMask = LayerMask.GetMask("board");
        //}
        /*
        public void FitToSizeCenter(int width, int height, Vector2Int margin, Vector3 offset, Vector3 padding)
        {
            float w = width + margin.x + padding.x;
            float h = height + margin.y + padding.y;

            cameraController.FitToSize(w, h);

            float x = (cameraController.ScreenWidthInWorldUnits - (float)(width + padding.x)) / 2f;
            float y = ((cameraController.ScreenHeightInWorldUnits) - (float)(height + padding.y)) / 2f;

            transform.localPosition = new Vector3(x, y, 0f) + offset;
        }*/

        public void Cleanup()
        {
            m_tileMapData = null;

            m_width = 0;
            m_height = 0;

            foreach (Tile slot in m_tiles)
            {
                if (slot == null)
                    continue;

                Destroy(slot.gameObject);
            }

            m_tiles = new Tile[] { };
        }

        public void Reset()
        {
            foreach (Tile slot in m_tiles)
            {
                if (slot == null)
                    continue;

                //slot.SetState(0);
            }
        }

        public void Generate(ITilemapData definition)
        {
            Cleanup();

            m_tileMapData = definition;

            m_width = m_tileMapData.Width;
            m_height = m_tileMapData.Height;

            m_tiles = new Tile[m_width * m_height];

            for (int x = 0; x < m_width; x++)
            {
                for (int y = 0; y < m_height; y++)
                {
                    if (definition.HasBlock(x, y) == false)
                        continue;

                    GameObject block = Instantiate(boardTemplate);

                    block.tag = "board";
                    block.name = "b" + x + "x" + y;
                    block.layer = LayerMask.NameToLayer("board");

                    block.transform.SetParent(transform);

                    block.transform.localPosition = new Vector3((float)x, (float)y, 0);

                    Tile slot = block.GetComponent<Tile>();

                    //slot.SetState(0);
                    slot.position = new Vector2Int(x,y);

                    m_tiles[x + (m_width * y)] = slot;
                }
            }

            if (gameBoardGenerated != null)
                gameBoardGenerated.Invoke(this);
        }

        public int GetTileIndex(Vector2Int tilePosition)
        {
            return tilePosition.x + (m_width * tilePosition.y);
        }

        public Tile GetTile(int index)
        {
            if (index < 0 || index > m_tiles.Length - 1)
                return null;

            return m_tiles[index];
        }

        public Tile GetTile(int x, int y)
        {
            return m_tiles[x + (y * m_width)];
        }

        public Tile GetTile(Vector2Int pos)
        {
            if (!IsInBounds(pos))
                return null;

            return m_tiles[pos.x + (pos.y * m_width)];
        }

        public bool IsFree(int x, int y)
        {
            Tile slot = m_tiles[x + (y * m_width)];

            return slot != null && slot.free;
        }

        public bool IsFree(Vector2Int pos)
        {
            Tile slot = m_tiles[pos.x + (pos.y * m_width)];

            return slot != null && slot.free;
        }

        public bool IsInBounds(Vector2Int pos)
        {
            if (pos.x < 0 || pos.x >= m_width)
                return false;

            if (pos.y < 0 || pos.y >= m_height)
                return false;

            return true;
        }

        public Vector2Int WorldToTilePosition(Vector2 position)
        {
            //Debug.Log("World: " + position);
            //Debug.Log("MAP: " + transform.position);

            Vector2 local = new Vector2(transform.position.x, transform.position.y);

            Vector2 pointOnTilemap = position - local;

            pointOnTilemap += tileSize / 2;

            //Debug.Log("POINT ON TILEMAP: " + pointOnTilemap);

            Vector2Int tilePos = new Vector2Int();

            tilePos.x = (int)(pointOnTilemap.x / tileSize.x);
            tilePos.y = (int)(pointOnTilemap.y / tileSize.y);

            //Debug.Log("TILE: " + tilePos);

            return tilePos;
        }

        public Vector2Int GetBoardPosition()
        {
            //Collider2D hit2D = Physics2D.OverlapPoint(CursorPosition, m_boardLayerMask);

            return WorldToTilePosition(CursorPosition);
            /*
            if (hit2D != null)
            {
                Tile slot = hit2D.transform.GetComponent<Tile>();

                return slot.position;
            }

            return m_invalidPosition;*/
        }

        public bool GetBoardPosition(out Vector2Int pos)
        {
            Vector2Int p = WorldToTilePosition(CursorPosition);

            Tile tile = GetTile(p);

            if (tile == null)
            {
                pos = m_invalidPosition;
                return false;
            }
            else
            {
                pos = p;
                return true;
            }
        }

        public bool GetBoardPosition(Vector2 cursorPos, out Vector2Int pos)
        {
            Vector2Int p = WorldToTilePosition(cursorPos);

            Tile tile = GetTile(p);

            if (tile == null)
            {
                pos = m_invalidPosition;
                return false;
            }
            else
            {
                pos = p;
                return true;
            }
        }

        public void SetSlotSprite(int x, int y, Sprite sprite)
        {
            m_tiles[x + (y * m_width)].foreground.color = Color.white;

            m_tiles[x + (y * m_width)].foreground.sprite = sprite;
        }

        public void SetBackgroundSprite(int x, int y, Sprite sprite)
        {
            m_tiles[x + (y * m_width)].background.color = Color.white;

            m_tiles[x + (y * m_width)].background.sprite = sprite;
        }

        public void SetSlotSprite(Vector2Int pos, string name, Color color)
        {
            int index = pos.x + (pos.y * m_width);

            m_tiles[index].foreground.sprite = tileset.GetSprite(name);

            m_tiles[index].foreground.color = color;
        }

        public void PlaceBlock(GameObject gameBlock, Vector2Int pos)
        {
            Tile slot = GetTile(pos);

            slot.SetBlock(gameBlock);
        }
    }
}