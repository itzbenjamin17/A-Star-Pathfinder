using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class GridManager
{
    private readonly int width;
    private readonly int height;
    private readonly Node[,] gridArray;
    private readonly TextMesh[,] displayArray;
    public readonly float cellSize;
    public List<Node> path;

    public GridManager(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        gridArray = new Node[width, height];
        displayArray = new TextMesh[width, height];
        this.cellSize = cellSize;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x,y] = new Node(x,y);
                displayArray[x,y] = CreateWorldText(gridArray[x, y].walkable.ToString(),GetWorldPosition(x,y) + new Vector3(cellSize,cellSize) * 0.5f);
                
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white);
                //Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white);

            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white);

    }
    public void ResetGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                gridArray[x, y] = new Node(x,y);
                displayArray[x, y].text = gridArray[x, y].walkable.ToString(); // Update the text mesh display
                displayArray[x, y].color = Color.white;

            }
        }
    }
    public void ResetStartNode()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (displayArray[x, y].text == "Start")
                {
                    displayArray[x, y].text = gridArray[x,y].walkable.ToString();
                }

            }
        }
    }
    public void ResetTargetNode()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (displayArray[x, y].text == "Target")
                {
                    displayArray[x, y].text = gridArray[x, y].walkable.ToString();
                }

            }
        }
    }
    public void UpdateWalkable()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                displayArray[x, y].color = gridArray[x, y].walkable? Color.white: Color.red; // Update the text mesh display

            }
        }
    }
    public void UpdatePath()
    {
        if (path != null)
        {
        for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if (path.Contains(gridArray[x, y]))
                        {
                        if (gridArray[x, y].hCost == 0)
                        {
                            displayArray[x, y].color = Color.green;

                        }
                        else
                            displayArray[x, y].color = Color.blue;
                        }

                    }
                }
        }
        
    }

    public static TextMesh CreateWorldText(string text,Vector3 localPosition)
    {
        GameObject gameObject = new("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(null, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Left;
        textMesh.text = text;
        textMesh.fontSize = 20;
        textMesh.color = Color.white;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = 5000;
        return textMesh;
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
    private Vector2 GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / cellSize);
        int y = Mathf.FloorToInt(worldPosition.y / cellSize);
        return new Vector2(x, y);
    }
    public void SetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && y < height && x < width) {
            gridArray[x,y].walkable = !gridArray[x, y].walkable;
            displayArray[x,y].text = gridArray[x, y].walkable.ToString();
        }
        
    }
    public void SetWalkable(Vector3 worldPosition)
    {
        Vector3 vec = GetXY(worldPosition);
        SetValue((int)vec.x, (int)vec.y);
    }
    public Node GetNode(Vector3 worldPosition)
    {
        Vector3 vec = GetXY(worldPosition);
        if (vec.x >= 0 && vec.y >= 0 && vec.y < height && vec.x < width)
        {
        Node node = gridArray[(int)vec.x, (int)vec.y];

        return node;

        }
        return null;
    }
    public void UpdateStartNode(Node node)
    {
        displayArray[node.x, node.y].text = "Start";

    }
    public void UpdateTargetNode(Node node)
    {
        displayArray[node.x, node.y].text = "Target";

    }

    public List<Node> FindNeighbours(Node node) {
        List<Node> neighbours = new();
        for(int x = node.x - 1; x <= node.x + 1; x++) {
            for(int y = node.y - 1; y <= node.y + 1; y++)
            {
                try
                {
                    if (gridArray[x, y] != node)
                    {
                        neighbours.Add(gridArray[x,y]);
                    }
                }
                catch (System.Exception)
                {
                }
            }
        }
        return neighbours;
    }
}
