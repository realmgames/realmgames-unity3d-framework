using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames.TileSystem
{
    public class TileRenderer : MonoBehaviour
    {
        public Tileset tileset;

        public Tilemap gameBoard;
        public TilemapDefinition m_gameBoardDefinition;

        public void UpdateTexture(int x, int y)
        {
            byte active_color = 2;

            if (m_gameBoardDefinition.GetMapValue(x, y) != active_color)
                return;

            bool right = m_gameBoardDefinition.InBounds(x + 1, y) && m_gameBoardDefinition.GetMapValue(x + 1, y) == active_color;
            bool left = m_gameBoardDefinition.InBounds(x - 1, y) && m_gameBoardDefinition.GetMapValue(x - 1, y) == active_color;
            bool up = m_gameBoardDefinition.InBounds(x, y + 1) && m_gameBoardDefinition.GetMapValue(x, y + 1) == active_color;
            bool down = m_gameBoardDefinition.InBounds(x, y - 1) && m_gameBoardDefinition.GetMapValue(x, y - 1) == active_color;

            if (up && right && down && left)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up-left-right-down") );
            }
            else if (up && left && right)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up-left-right"));
            }
            else if (up && left && down)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up-left-down"));
            }
            else if (up && right && down)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up-right-down"));
            }
            else if (left && right && down)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("left-right-down"));
            }
            else if (up && right)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up-right"));
            }

            else if (up && left)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up-left"));
            }

            else if (down && right)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("down-right"));
            }

            else if (down && left)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("down-left"));
            }
            else if (left && right)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("horizontal"));
            }

            else if (up && down)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("vertical"));
            }

            else if (left)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("left"));
            }

            else if (right)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("right"));
            }

            else if (up)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("up"));
            }

            else if (down)
            {
                gameBoard.SetSlotSprite(x, y, tileset.GetSprite("down"));
            }

        }

        public void UpdateTextures()
        {
            for (int x = 0; x < m_gameBoardDefinition.width; x++)
            {
                for (int y = 0; y < m_gameBoardDefinition.height; y++)
                {
                    UpdateTexture(x, y);
                }
            }
        }
    }
}