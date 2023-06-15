using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 CamPlayerDistance;
    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        CamPlayerDistance = Player.transform.position - transform.position;
        
    }

  
    void LateUpdate()
    {
        transform.position = Player.transform.position - CamPlayerDistance;
    }
}
