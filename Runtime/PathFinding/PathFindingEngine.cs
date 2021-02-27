using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RealmGames.PathFinding
{
    public delegate bool CanTraverse(Vector2Int p);

    public class PathFindingEngine
    {
        private Vector2Int m_size;
        private bool[] m_visited;
        private CanTraverse m_canTraverse;

        public PathFindingEngine(Vector2Int size, CanTraverse canTraverse)
        {
            m_size = size;
            m_canTraverse = canTraverse;
            m_visited = new bool[m_size.x * m_size.y];
        }

        public bool InBounds(Vector2Int p)
        {
            return p.x >= 0 && p.x < m_size.x &&
                    p.y >= 0 && p.y < m_size.y;
        }

        public bool HasVisited(Vector2Int p)
        {
            return m_visited[p.x + (p.y * m_size.x)];
        }

        int Cost(CanTraverse canTraverse, Vector2Int p)
        {
            int cost = 0;

            Vector2Int right = p + Vector2Int.right;
            Vector2Int left = p + Vector2Int.left;
            Vector2Int up = p + Vector2Int.up;
            Vector2Int down = p + Vector2Int.down;

            if (InBounds(right))
                cost += canTraverse(right) ? 0 : 2;

            if (InBounds(left))
                cost += canTraverse(left) ? 0 : 2;

            if (InBounds(up))
                cost += canTraverse(up) ? 0 : 2;

            if (InBounds(down))
                cost += canTraverse(down) ? 0 : 2;
            
            return cost;
        }

        void MarkVisited(Vector2Int p)
        {
            m_visited[p.x + (p.y * m_size.x)] = true;
        }

        void ClearVisited() {
            for (int i = 0; i < m_visited.Length; i++)
                m_visited[i] = false;
        }
 
        void VisitNode(CanTraverse canTraverse, PathNode current, List<PathNode> nodes, Vector2Int goal)
        {
            int step = current.step + 1;

            Vector2Int right = current.position + Vector2Int.right;
            Vector2Int left = current.position + Vector2Int.left;
            Vector2Int up = current.position + Vector2Int.up;
            Vector2Int down = current.position + Vector2Int.down;

            if (InBounds(right) && canTraverse(right) && !HasVisited(right))
            {
                MarkVisited(right);
                int cost = Cost(canTraverse, right);
                nodes.Add(new PathNode(current, right, step + cost, goal));
            }

            if (InBounds(left) && canTraverse(left) && !HasVisited(left))
            {
                MarkVisited(left);
                int cost = Cost(canTraverse, left);
                nodes.Add(new PathNode(current, left, step + cost, goal));
            }

            if (InBounds(up) && canTraverse(up) && !HasVisited(up))
            {
                MarkVisited(up);
                int cost = Cost(canTraverse, up);
                nodes.Add(new PathNode(current, up, step + cost, goal));
            }

            if (InBounds(down) && canTraverse(down) && !HasVisited(down))
            {
                MarkVisited(down);
                int cost = Cost(canTraverse, down);
                nodes.Add(new PathNode(current, down, step + cost, goal));
            }
        }

        void WalkNode(CanTraverse canTraverse, Vector2Int current, List<Vector2Int> nodes)
        {
            Vector2Int right = current + Vector2Int.right;
            Vector2Int left = current + Vector2Int.left;
            Vector2Int up = current + Vector2Int.up;
            Vector2Int down = current + Vector2Int.down;

            if (InBounds(right) && canTraverse(right) && !HasVisited(right))
            {
                MarkVisited(right);
                nodes.Add(right);
            }

            if (InBounds(left) && canTraverse(left) && !HasVisited(left))
            {
                MarkVisited(left);
                nodes.Add(left);
            }

            if (InBounds(up) && canTraverse(up) && !HasVisited(up))
            {
                MarkVisited(up);
                nodes.Add(up);
            }

            if (InBounds(down) && canTraverse(down) && !HasVisited(down))
            {
                MarkVisited(down);
                nodes.Add(down);
            }
        }

        public void WalkAllNodes(Vector2Int start)
        {
            ClearVisited();

            List<Vector2Int> nodes = new List<Vector2Int>();

            nodes.Add(start);

            while (nodes.Count > 0)
            {
                Vector2Int current = nodes[0]; nodes.RemoveAt(0);

                WalkNode(m_canTraverse, current, nodes);
            }
        }

        public PathNode FindPath(Vector2Int start, Vector2Int goal)
        {
            ClearVisited();

            //Debug.Log("FINDING PATH FROM: " + start + "->" + goal);

            List<PathNode> nodes = new List<PathNode>();
            PathNode start_node = new PathNode(null, start, 0, goal);
            PathNode goal_node = null;

            nodes.Add(start_node);

            while (nodes.Count > 0)
            {
                PathNode current = nodes[0]; nodes.Remove(current);

                if (current.position.x == goal.x && current.position.y == goal.y)
                {
                    goal_node = current;
                    //Debug.Log("FOUND GOAL NODE!");
                    break;
                }

                //Debug.Log("VISITING NODE: " + current);

                VisitNode(m_canTraverse, current, nodes, goal);

                nodes.Sort(delegate (PathNode a, PathNode b)
                {
                    if (a.heuristic > b.heuristic) return 1;
                    else if (a.heuristic < b.heuristic) return -1;
                    else return 0;
                });

                //Debug.Log("### SORT RESULT ###");
                //foreach(PathNode n in nodes) Debug.Log("heuristic: " + n.heuristic);
            }

            return goal_node;
        }
    }
}