using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldpos;
    public bool walkable;
    public int gCost;
    public int hCost;
    public int gridX;//how many nodes on x
    public int gridY;//how many nodes on y
    public Node parent;


    public Node(bool _walkable, Vector3 _worldpos, int _gridX, int _gridY)
    {
       
        gridX =_gridX;
        gridY =_gridY;
        walkable = _walkable;
        worldpos = _worldpos;
    }
    public int fCost
    {
         get { return gCost + hCost; }
     }
}
