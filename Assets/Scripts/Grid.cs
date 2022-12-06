using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private Node nodePrefab;

    [SerializeField]
    private float spacing = 0.1f;

    private Node[,] nodes;

    private AStar star;
    private CameraController cam;

    private void Start()
    {
        cam = FindObjectOfType<CameraController>();
    }

    [ContextMenu("Generate")]
    public void GenerateGrid(int width, int height)
    {
        Reset();
        nodes = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x - (width - 1.0f) / 2.0f, y - (height - 1.0f) / 2.0f);
                pos += new Vector2(x, y) * spacing;
                var node = Instantiate(nodePrefab, pos, Quaternion.identity);
                node.transform.SetParent(transform, true);
                node.name = $"{x}, {y}";

                node.Index = new Vector2Int(x, y);
                nodes[x, y] = node;
            }
        }

        star = new AStar(nodes);
        cam.UpdateSize(width, height);
    }

    public async void FindShortestPath()
    {
        List<Node> path = await star.Search();
        for (int i = 0; i < path.Count; i++)
        {
            path[i].SetPath();
            await Task.Delay(100);
        } 
    }

    private void Reset()
    {
        if (nodes != null)
        {
            foreach (Node node in nodes)
            {
                DestroyImmediate(node.gameObject);
            }
        }
    }
}
