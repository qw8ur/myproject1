using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //test
    [SerializeField] float maxDistancetolerate;
    //怪物預設隨機走動
    private NavMeshAgent m_Agent;
    [SerializeField] float radius;
    
    //怪物--玩家追蹤
    [SerializeField] Transform monster;
    [SerializeField] float rotationspeed = 5;
    [SerializeField] GameObject player;
    [SerializeField] Transform target;
    //[SerializeField] float speed;
    [SerializeField] float battleDistance = 5f;

    bool IsinZone=false;

    
    void Start()
    {
        m_Agent=GetComponent<NavMeshAgent>();//怪物預設隨機走動

        player = GameObject.FindGameObjectWithTag("Player");//怪物--玩家追蹤
    }


    // Update is called once per frame
    void Update()
    {
        monster.position = new Vector3(monster.position.x, 0, monster.position.z);
        var maxDistance2 = Vector3.Distance(target.position, monster.position);
        if (!m_Agent.hasPath) //怪物預設隨機走動
        {
            m_Agent.SetDestination(Getpoint.instance.GetRandomPoint(transform,radius));
            monster.position = new Vector3(monster.position.x, 0, monster.position.z);
            var maxDistance = Vector3.Distance(target.position-Vector3.left, monster.position);

            
        }
        else if (!m_Agent.hasPath|| maxDistance2 < battleDistance||IsinZone==true) 
        {
            
            m_Agent.SetDestination(target.position);
            monster.position += monster.forward * m_Agent.speed * Time.deltaTime;
            monster.rotation = Quaternion.Slerp(monster.rotation, Quaternion.LookRotation(target.position - monster.position), rotationspeed * Time.deltaTime);
            if (maxDistance2<=maxDistancetolerate)
            {
                Debug.Log("Stop");
                m_Agent.speed = 0;
                IsinZone = true;
              //monster.position -= Vector3.left * 4;
            }
            else if(maxDistance2>maxDistancetolerate)
            {
                Debug.Log("Move");
                IsinZone= false;
                m_Agent.speed = 3;
            }


        }

        UpdateAnimator();
    }


    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        //float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("Blend", m_Agent.speed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, battleDistance);

    }

}
