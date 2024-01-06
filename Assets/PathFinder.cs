using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private GridManager grid;
    private Node startNode, targetNode;

    // Start is called before the first frame update
    void Start()
    {
        grid = new(20, 10, 10f);
    }
    private void Update()
    // left click is changing the start node
    // right click is changing target node
    // middle click is changing the unwalkable grid spots
    // spacebar is resetting the grid
    {

        //Changing the start node
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("s");
            Node clickedNode = grid.GetNode(GetMouseWorldPosition());
            if (clickedNode != null && clickedNode!= startNode && clickedNode!= targetNode)
             {
                startNode = clickedNode;
            grid.ResetStartNode();
                grid.UpdateStartNode(startNode);
            grid.UpdateWalkable();
             }
         
        }
        //This will be changing the target node
        if (Input.GetMouseButtonDown(1))
            {
            Debug.Log("t");

            Node clickedNode = grid.GetNode(GetMouseWorldPosition());
            if (clickedNode != null && clickedNode != startNode && clickedNode != targetNode)
            {
                targetNode = clickedNode;
                grid.ResetTargetNode();
                grid.UpdateTargetNode(targetNode);
                grid.UpdateWalkable();
            }
                

            }
        // This will be toggling the unwalkable grid spots
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("w");

            Node clickedNode = grid.GetNode(GetMouseWorldPosition());
            if (clickedNode != null && clickedNode != startNode && clickedNode != targetNode)
            {
            grid.SetWalkable(GetMouseWorldPosition());
            grid.UpdateWalkable();
            }
        }
        
        //reset grid
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grid.ResetGrid();
            startNode = null;
            targetNode = null;
            grid.path = null;
        }
        if (startNode != null && targetNode != null && startNode != targetNode)
        {
            FindShortestPath(startNode, targetNode);
            
        }
        
        grid.UpdatePath();
       
    }

    void FindShortestPath(Node startNode, Node targetNode)
    {

        List<Node> openNodes = new();
        HashSet<Node> closedNodes = new();
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];

            for (int i = 0; i < openNodes.Count; i++)
            {
                if (openNodes[i].FCost < currentNode.FCost || openNodes[i].FCost == currentNode.FCost && openNodes[i].hCost < currentNode.hCost)
                {
                    currentNode = openNodes[i];
                }
            }

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in grid.FindNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }
                // distance from start node to neighbour node
                int costToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                // if this distance is shorter than the gCost (distance from start node to that neighbour node) or neighbour isnt in openNodes
                // update the costs of that neigbour node
                if (costToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    neighbour.gCost = costToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parentNode = currentNode;

                    if (!openNodes.Contains(neighbour))
                    {
                        openNodes.Add(neighbour);
                    }
                }
            }
        }
        grid.path = null;
    }
    void RetracePath(Node startNode, Node targetNode)
    {
        List<Node> path = new();
        Node current = targetNode;
        while (current != startNode)
        {
            path.Add(current);
            current = current.parentNode;
        }
        path.Add(current);
        path.Reverse();

        grid.path = path;
    }
    int GetDistance(Node nodeFrom, Node nodeTo)
    // we multiply distances by 10 and round them
    {
        int distX = Mathf.Abs(nodeFrom.x - nodeTo.x);
        int distY = Mathf.Abs(nodeFrom.y - nodeTo.y);

        if (distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);

    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;

    }

   


}

