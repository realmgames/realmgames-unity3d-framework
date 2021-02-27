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
        private TilemapDefinition m_tileMapDefinition;

        public int Width
        {
            get
            {
                return (m_tileMapDefinition != null) ? m_tileMapDefinition.width : 0;
            }
        }

        public int Height
        {
            get
            {
                return (m_tileMapDefinition != null) ? m_tileMapDefinition.height : 0;
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
            m_tileMapDefinition = null;

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

                slot.SetState(0);
            }
        }

        public void Generate(TilemapDefinition definition, bool centerGameBoard = true)
        {
        //    if (centerGameBoard)
        //        FitToSizeCenter(definition.width, definition.height, margin, new Vector3(0.5f, 0.5f, 0f), new Vector3(0, 1f, 0));

            Cleanup();

            m_tileMapDefinition = definition;

            m_width = m_tileMapDefinition.width;
            m_height = m_tileMapDefinition.height;

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

                    slot.SetState(0);
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

        public Tile GetTile(Vector2Int tileMapPosition)
        {
            return GetTile(GetTileIndex(tileMapPosition));
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

            /*
            Collider2D hit2D = Physics2D.OverlapPoint(CursorPosition, m_boardLayerMask);

            if (hit2D != null)
            {
                Tile slot = hit2D.transform.GetComponent<Tile>();

                pos = slot.position;

                return true;
            }

            pos = m_invalidPosition;

            return false;
            */
        }

        public void SetSlotSpriteState(int x, int y, int index)
        {
            m_tiles[x + (y * m_width)].SetState(index);
        }

        public void SetSlotSpriteState(Vector2Int pos, int index)
        {
            m_tiles[pos.x + (pos.y * m_width)].SetState(index);
        }

        public void SetSlotSpriteState(Vector2Int pos, string name)
        {
            m_tiles[pos.x + (pos.y * m_width)].SetState(name);
        }

        public void SetSlotSpriteState(Vector2Int pos, string name, Color color)
        {
            m_tiles[pos.x + (pos.y * m_width)].SetState(name, color);
        }

        public string GetSlotSpriteStateName(Vector2Int pos)
        {
            return m_tiles[pos.x + (pos.y * m_width)].GetStateName();
        }

        public void SetSlotSprite(int x, int y, Sprite sprite)
        {
            m_tiles[x + (y * m_width)].spriteRenderer.sprite = sprite;
        }

        public void SetSlotSprite(Vector2Int pos, string name, Color color)
        {
            int index = pos.x + (pos.y * m_width);

            m_tiles[index].spriteRenderer.sprite = tileset.GetSprite(name);

            m_tiles[index].spriteRenderer.color = color;
        }
    }
}