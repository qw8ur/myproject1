using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridNya : MonoBehaviour
{
    public GameObject monster;
    public Vector2 gridWorldSize;
    public LayerMask unWalkable;
    public float nodeRadius;
    float nodeDiameter;
    public static int gridX, gridY;//how many node on x and y
    Node[,] node;
    

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateNode();
    }
    void CreateNode()
    {
        node = new Node[gridX, gridY];
        Vector3 pointBtnLeft= transform.position-Vector3.right*gridWorldSize.x/2-Vector3.forward* gridWorldSize.y / 2;
        for (int x = 0;x< gridX;x++)
        { 
           for (int y = 0;y< gridY;y++)
          {
                Vector3 pointCenter = pointBtnLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !Physics.CheckSphere(pointCenter, nodeRadius);
                node[x, y] = new Node(walkable, pointCenter, x, y);
          }

        }
    }

    public List<Node> GetNeighbor(Node nodeCenter)
    {
        List<Node> neighborNodes = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int neighborNodeX = nodeCenter.gridX + x;
                int neighborNodeY = nodeCenter.gridY + y;
                if (neighborNodeX >= 0 && neighborNodeX < gridX && neighborNodeY >= 0 && neighborNodeY < gridY)
                {
                    neighborNodes.Add(node[neighborNodeX, neighborNodeY]);
                }
            }
        }
        return neighborNodes;
    }




    public Node TargetNodeOnWorld(Vector3 targetPos)
    {
        float percentX=(targetPos.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (targetPos.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x= Mathf.RoundToInt((gridX - 1) * percentX);
        int y = Mathf.RoundToInt((gridY - 1) * percentY);
        return node[x, y];

    }

    public List<Node> path;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x,1, gridWorldSize.y));

        if(node != null)
        {
            Node targetNode = TargetNodeOnWorld(monster.transform.position);
        foreach (Node n in node) 
        {
                if (n.walkable)
                {
                    Gizmos.color = Color.clear;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                if (path != null)
                {
                    if (path.Contains(n))
                    {
                        Gizmos.color = Color.magenta;
                    }
                }
                Gizmos.DrawCube(n.worldpos, Vector3.one * (nodeDiameter - 0.1f));
        }
        }
    }
}
