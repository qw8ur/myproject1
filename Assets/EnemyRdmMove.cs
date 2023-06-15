using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRdmMove : MonoBehaviour
{
    private NavMeshAgent m_Agent;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Agent.hasPath)
        {
            m_Agent.SetDestination(Getpoint.instance.GetRandomPoint(transform, radius));
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);

    }
}
