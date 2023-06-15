using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PathFinding : MonoBehaviour
{
    public Transform monster;
    public Transform target;
    [SerializeField] GameObject player;
    [SerializeField] float battleDistance = 2f;
    public static GridNya grid;
    public float movementSpeed = 5f; // �Ǫ������ʳt��
    public float rotationspeed = 5;

    private List<Node> path;
    private int currentNodeIndex = 0; // �ثe�`�I������

    bool foundPlayer = false;//���S�����a�b�d��?

    void Awake()
    {
        grid = GetComponent<GridNya>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {

        FindPath(monster.position, target.position);
        CheckPlayerTag();

        if (foundPlayer == true)
        {
            TracePlayer();
            
        }
        if (path != null && foundPlayer == false)
        {
            MoveMonster();
        }


    }

    void TracePlayer()
    {
        monster.position = Vector3.MoveTowards(monster.position, player.transform.position-Vector3.left, movementSpeed * Time.deltaTime);
        monster.rotation = Quaternion.Slerp(monster.rotation, Quaternion.LookRotation(player.transform.position - monster.position), rotationspeed * Time.deltaTime);
    }

    void FindPath(Vector3 startPos, Vector3 endPos)
    {
        Node startNode = grid.TargetNodeOnWorld(startPos);
        Node endNode = grid.TargetNodeOnWorld(endPos);

        List<Node> openList = new List<Node>();
        HashSet<Node> closeList = new HashSet<Node>();
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (currentNode.fCost > openList[i].fCost || currentNode.fCost == openList[i].fCost)
                {
                    if (currentNode.hCost > openList[i].hCost)
                        currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (currentNode == endNode)
            {
                RetracePath(startNode, endNode);
                return;
            }

            foreach (Node neighbor in grid.GetNeighbor(currentNode))
            {
                if (!neighbor.walkable || closeList.Contains(neighbor))
                {
                    continue;
                }

                int newCostToNeighbor = currentNode.gCost + Getdistance(currentNode, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openList.Contains(neighbor))
                {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = Getdistance(neighbor, endNode);
                    neighbor.parent = currentNode;

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
        
    }

    int Getdistance(Node nodeA, Node nodeB)
    {
        int distanceX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distanceY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distanceX > distanceY)
        {
            return 14 * distanceY + 10 * (distanceX - distanceY);
        }
        else
        {
            return 14 * distanceX + 10 * (distanceY - distanceX);
        }
    }

     void CheckPlayerTag()
    {
        // �˴�battleDistance�d�򤺬O�_���аO��"player"������
        Collider[] colliders = Physics.OverlapSphere(monster.position, 5f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                foundPlayer=true;
                return;
            }

        }
        
    }
    void MoveMonster()
    {
        float maxDistance = Vector3.Distance(player.transform.position, monster.position);
        currentNodeIndex = 0; // ���]�ثe�`�I����

        if (maxDistance < battleDistance)//�b�Ǫ������d��
        {
            monster.position += monster.forward * movementSpeed * Time.deltaTime;
            monster.rotation = Quaternion.Slerp(monster.rotation, Quaternion.LookRotation(player.transform.position - monster.position), rotationspeed * Time.deltaTime);
        }
        else if (maxDistance > battleDistance)//�b�Ǫ������d��~
        {
            if (currentNodeIndex >= 0 && currentNodeIndex < path.Count)
            {
                Node targetNode = path[currentNodeIndex];
                Vector3 targetNodePosition = targetNode.worldpos;
                Vector3 currentPosition = monster.position;


                if (foundPlayer)
                {
                    TracePlayer();
                }
                else if (!foundPlayer)
                {
                    monster.position = Vector3.MoveTowards(currentPosition, targetNodePosition, movementSpeed * Time.deltaTime);

                    // �ˬd�O�_�w��F�ؼи`�I
                    if (Vector3.Distance(monster.position, targetNodePosition) < 0.01f)
                    {
                        currentNodeIndex++; // ��s�ثe�`�I����
                    }
                    else if (target.position.x > 0 && target.position.x != targetNodePosition.x)
                    {
                        monster.position = Vector3.MoveTowards(currentPosition, target.position, movementSpeed * Time.deltaTime);
                    }
                }

            }
        }

            
        
    }
}
