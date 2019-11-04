using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int width;
    public int height;
    public Cell cellProto;
    public float offset;

    private Cell[,] _grid;

    private void Awake()
    {
        _grid = new Cell[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = Instantiate(cellProto);
                cell.transform.SetParent(transform);
                cell.transform.localPosition = new Vector3(x * offset, -y * offset, 0);
                cell.value = 0;
                _grid[y, x] = cell;
            }
        }
    }

    public void IncCell(Point cell, int value)
    {
        _grid[cell.y, cell.x].value += value;
    }

    public bool IsOccupied(Point cell)
    {
        return _grid[cell.y, cell.x].value != 0;
    }

    public List<Point> GetEmptyCells()
    {
        List<Point> result = new List<Point>();
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if(_grid[y,x].value == 0)
                {
                    result.Add(new Point(x, y));
                }
            }
        }
        return result;
    }

    public int CellValue(Point cell)
    {
        return _grid[cell.y, cell.x].value;
    }

    public bool IsInsideBounts(Point cell)
    {
        return cell.x >= 0 && cell.x < width && cell.y >= 0 && cell.y < height;
    }
}

public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}
