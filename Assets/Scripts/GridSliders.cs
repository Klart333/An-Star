using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSliders : MonoBehaviour
{
    [SerializeField]
    private Slider widthSlider;
    
    [SerializeField]
    private Slider heightSlider;

    private Grid grid;

    private int gridWidth = 3;
    private int gridHeight = 3;

    private void Start()
    {
        grid = FindObjectOfType<Grid>();

        widthSlider.onValueChanged.AddListener(OnWidthChanged);
        heightSlider.onValueChanged.AddListener(OnHeightChanged);
    }

    private void OnWidthChanged(float width)
    {
        gridWidth = Mathf.RoundToInt(width);
        RebuildGrid();
    }

    private void OnHeightChanged(float height)
    {
        gridHeight = Mathf.RoundToInt(height);
        RebuildGrid();
    }

    public void RebuildGrid()
    {
        grid.GenerateGrid(gridWidth, gridHeight);
    }
}
