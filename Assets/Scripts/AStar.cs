using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DataStructures.Queue;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;

public class AStar 
{
    private Node[,] nodes;

    public AStar(Node[,] nodes)
    {
        this.nodes = nodes;
    }

    public async Task<List<Node>> Search()
    {
        GetSpecials(out Node start, out Node target);

        Dictionary<Node, int> costAtNode = new Dictionary<Node, int>();
        costAtNode.Add(start, 0);

        Dictionary<Vector2Int, Node> visitedNodes = new Dictionary<Vector2Int, Node>();
        visitedNodes.Add(start.Index, start);

        PriorityQueue<int, Node> queue = new PriorityQueue<int, Node>();
        queue.Enqueue(0, start);

        bool foundPath = false;

        while (queue.Count > 0)
        {
            await Task.Delay(25);

            var currentNode = queue.Dequeue();
            currentNode.SetWalked();

            if (currentNode.Index == target.Index)
            {
                foundPath = true;
                break;
            }

            List<Node> neighbours = GetNeighbours(currentNode.Index.x, currentNode.Index.y);
            for (int i = 0; i < neighbours.Count; i++)
            {
                int cost = costAtNode[currentNode] + 1; // It has to be +1

                if (!visitedNodes.ContainsKey(neighbours[i].Index))
                {
                    int prio = cost + GetHeuristic(neighbours[i], target);
                    queue.Enqueue(prio, neighbours[i]);

                    costAtNode.Add(neighbours[i], cost);
                    visitedNodes.Add(neighbours[i].Index, currentNode); // Came from this currentNode
                }
                else if (cost < costAtNode[neighbours[i]])
                {
                    int prio = cost + GetHeuristic(neighbours[i], target);
                    queue.Enqueue(prio, neighbours[i]);

                    costAtNode[neighbours[i]] = cost;
                    visitedNodes[neighbours[i].Index] = currentNode; // Replace instead
                }
            }

        }

        if (!foundPath)
        {
            return new List<Node>();
        }

        return GetPath(visitedNodes, target, start);
    }

    private List<Node> GetPath(Dictionary<Vector2Int, Node> visitedNodes, Node target, Node start)
    {
        List<Node> path = new List<Node>();
        Node current = target;
        while (current.Index != start.Index)
        {
            path.Add(current);

            current = visitedNodes[current.Index];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }

    private int GetHeuristic(Node current, Node target)
    {
        return Mathf.Abs(current.Index.x - target.Index.x) + Mathf.Abs(current.Index.y - target.Index.y);
    }

    private void GetSpecials(out Node start, out Node target)
    {
        start = null;
        target = null;
        foreach (Node node in nodes)
        {
            if (start == null)
            {
                if (node.Special)
                {
                    start = node;
                }
            }
            else if (target == null)
            {
                if (node.Special)
                {
                    target = node;
                }
            }
            else
            {
                return;
            }
        }
    }

    private List<Node> GetNeighbours(int x, int y)
    {
        var neighs = new List<Node>();

        if (x + 1 < nodes.GetLength(0))
        {
            neighs.Add(nodes[x + 1, y]);
        }

        if (x - 1 >= 0)
        {
            neighs.Add(nodes[x - 1, y]);
        }

        if (y + 1 < nodes.GetLength(1))
        {
            neighs.Add(nodes[x, y + 1]);
        }

        if (y - 1 >= 0)
        {
            neighs.Add(nodes[x, y - 1]);
        }

        for (int i = 0; i < neighs.Count; i++)
        {
            if (!neighs[i].Walkable)
            {
                neighs.RemoveAt(i--);
            }
        }

        return neighs;
    }
}

namespace DataStructures.Queue
{
    public class PriorityQueueEntry<TPrio, TItem>
    {
        public TPrio p { get; }
        public TItem data { get; }
        public PriorityQueueEntry(TPrio p, TItem data)
        {
            this.p = p;
            this.data = data;
        }
    }

    public class PriorityQueue<TPrio, TItem> where TPrio : IComparable
    {
        private LinkedList<PriorityQueueEntry<TPrio, TItem>> q;

        public PriorityQueue()
        {
            q = new LinkedList<PriorityQueueEntry<TPrio, TItem>>();
        }

        public int Count { get { return q.Count(); } }

        public void Enqueue(TPrio p, TItem data)
        {
            if (q.Count == 0)
            {
                q.AddFirst(new PriorityQueueEntry<TPrio, TItem>(p, data));
                return;
            }
            // This is a bit classical C but whatever
            LinkedListNode<PriorityQueueEntry<TPrio, TItem>> current = q.First;
            while (current != null)
            {
                if (current.Value.p.CompareTo(p) >= 0)
                {
                    q.AddBefore(current, new PriorityQueueEntry<TPrio, TItem>(p, data));
                    return;
                }
                current = current.Next;
            }
            q.AddLast(new PriorityQueueEntry<TPrio, TItem>(p, data));
        }

        public TItem Dequeue()
        {
            // LinkedList -> LinkedListNode -> PriorityQueueEntry -> data
            var ret = q.First.Value.data;
            q.RemoveFirst();
            return ret;
        }
    }
}
