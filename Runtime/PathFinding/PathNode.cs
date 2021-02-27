using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RealmGames
{
    public class PathNode
    {
        public Vector2Int position;
        public PathNode parent;
        public int heuristic;
        public int step;

        public PathNode(PathNode parent, Vector2Int pos, int step, Vector2Int goal)
        {
            this.parent = parent;
            this.position = pos;
            this.step = step;
            this.heuristic = Mathf.Abs(position.x - goal.x) + Mathf.Abs(position.y - goal.y) + step;
        }

        public int ComputeHeuristic(int x, int y, int z)
        {
            return Mathf.Abs(position.x - x) + Mathf.Abs(position.y - y);
        }

        public Vector2Int[] GetPathPoints()
        {
            List<Vector2Int> points = new List<Vector2Int>();

            PathNode current = this;

            while (current != null)
            {
                Vector2Int p = current.position;

                current = current.parent;

                points.Insert(0, p);
            }

            return points.ToArray();
        }

        public Vector3[] GetPathPointsVec3()
        {
            List<Vector3> points = new List<Vector3>();

            PathNode current = this;

            while (current != null)
            {
                Vector2Int p = current.position;

                current = current.parent;

                points.Insert(0, new Vector3(p.x, p.y, 0f));
            }

            return points.ToArray();
        }

        public override string ToString()
        {
            return string.Format("[PathNode] {0}", position);
        }
    }
}