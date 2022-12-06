using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color wallColor;

    [SerializeField]
    private Color specialColor;

    [SerializeField]
    private Color walkedColor;

    [SerializeField]
    private Color pathColor;

    private SpriteRenderer rend;

    public bool Walkable { get; private set; } = true;
    public bool Special { get; private set; } = false;
    public Vector2Int Index { get; set; }

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(1))
        {
            ToggleWall();
        }

        if (Input.GetMouseButton(0))
        {
            ToggleWall();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ToggleSpecial();
        }

        if (Input.GetMouseButtonDown(0))
        {
            ToggleWall();
        }
    }

    private void ToggleSpecial()
    {
        Walkable = true;
        Special = true;

        rend.color = specialColor;
    }

    public void ToggleWall()
    {
        Special = false;

        Walkable = !Walkable;

        rend.color = Walkable ? normalColor : wallColor;
    }

    public void SetPath()
    {
        rend.color = pathColor;
    }

    public void SetWalked()
    {
        rend.color = walkedColor;
    }
}