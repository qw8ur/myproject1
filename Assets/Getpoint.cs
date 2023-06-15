using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Getpoint : MonoBehaviour
{
    public static Getpoint instance;
    public float range;
  
    private void Awake()
    {
        instance = this;
    }
   
    bool RandomPoint(Vector3 center, float range, out Vector3 result)//如果忘記了就參考官方文件
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;

    }
    public Vector3 GetRandomPoint(Transform point = null, float radius = 0)
    {
        Vector3 _point;
        if (RandomPoint(point == null ? transform.position : point.position, radius == 0 ? range : radius, out _point))
        {
            Debug.DrawRay(_point, Vector3.up, Color.black, 1);
            return _point;
        }
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
