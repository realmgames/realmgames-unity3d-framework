using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealmGames.TileSystem
{
    [CreateAssetMenu(fileName = "new game board definition", menuName = "GameBoardDefinition")]
    public class GameBoardDefinition : StageDefinition
    {
        public int width;
        public int height;
        public int blockCount;
        public Vector2Int start;
        public List<Vector2Int> hint;
        public byte[] map;

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

#if UNITY_EDITOR
        static bool TrySolve(GameBoardDefinition gameboard, Vector2Int startPos)
        {
            List<Vector2Int> selected = new List<Vector2Int>();
            selected.Add(startPos);
            Vector2Int pos = startPos;

            while (true)
            {
                List<Vector2Int> moves = new List<Vector2Int>();

                Vector2Int left = Vector2Int.left + pos;
                Vector2Int right = Vector2Int.right + pos;
                Vector2Int up = Vector2Int.up + pos;
                Vector2Int down = Vector2Int.down + pos;

                if (!selected.Contains(left) && gameboard.HasBlock(left))
                    moves.Add(left);

                if (!selected.Contains(right) && gameboard.HasBlock(right))
                    moves.Add(right);

                if (!selected.Contains(up) && gameboard.HasBlock(up))
                    moves.Add(up);

                if (!selected.Contains(down) && gameboard.HasBlock(down))
                    moves.Add(down);

                int movesCount = moves.Count;

                if (movesCount == 0)
                    return false;

                if (movesCount == 1)
                    pos = moves[0];
                else
                    pos = moves[Random.Range(0, movesCount)];

                selected.Add(pos);

                if (selected.Count == gameboard.blockCount)
                {
                    /*
                    foreach (Vector2Int m in selected)
                    {
                        Debug.Log("move: " + m);
                    }*/

                    return true;
                }
            }
        }

        public static bool Solver(GameBoardDefinition gameboard, Vector2Int start)
        {
            int attempts = 0;
            while (attempts < 10000)
            {
                if (TrySolve(gameboard, start))
                {
                    return true;
                }

                attempts++;
            }
            return false;
        }

        public static void Solver(GameBoardDefinition gameboard)
        {
            int attempts = 0;

            while (attempts < 25000)
            {
                if (TrySolve(gameboard, gameboard.start))
                {
                    Debug.Log("<color=lime>Solution found</color>, attempt: " + attempts);
                    return;
                }

                attempts++;
            }

            List<Vector2Int> startPositions = new List<Vector2Int>();

            for (int x = 0; x < gameboard.width; x++)
            {
                for (int y = 0; y < gameboard.height; y++)
                {
                    if (gameboard.HasBlock(x, y))
                        startPositions.Add(new Vector2Int(x, y));
                }
            }

            for (int i = 0; i < startPositions.Count; i++)
            {
                Vector2Int start = startPositions[i];

                if (Solver(gameboard, start))
                {
                    Debug.Log("Solution found at start(" + start + ")");
                    return;
                }
            }

            Debug.Log("<color=red>No Solutions found.</color>");
        }
#endif
    }
}