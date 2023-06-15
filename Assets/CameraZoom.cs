using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    
    
    public GameObject player;
    [SerializeField]
    private float zoomStep,minCamView, maxCamView;


    void Start()
    {
        Test();
    }
    public void Test()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 targetPosition = player.transform.position;
        Vector3 direction = (targetPosition - origin).normalized;
        Ray ray = new Ray(origin, direction);
        bool raycast = Physics.Raycast(ray, out RaycastHit hitInfo);
        Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
    }

    public void TestClickPos()
    {
        Vector3 origin = Camera.main.transform.position;
        Vector3 click = Input.mousePosition;
        Vector3 camClickDirection = (click - origin).normalized;
        Ray ray = new Ray(origin, camClickDirection);
        bool raycast = Physics.Raycast(ray, out RaycastHit hitInfo);
        Debug.DrawLine(ray.origin, hitInfo.point, Color.blue);
    }


    void Update()
    {
       
        Test();
        TestClickPos();

        // -------------------Code for Zooming Out------------
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOut();
        }
        // ---------------Code for Zooming In------------------------
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomIn();
        }

        // ---------------Code for Rotation------------------------

        if (Input.GetAxis("Fire3") > 0)
        {

            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(0, mouseX, 0, Space.World);
            


        }

    }

    
        public void ZoomIn()
    {
        float newView = Camera.main.fieldOfView - zoomStep;
        Camera.main.fieldOfView = Mathf.Clamp(newView, minCamView, maxCamView);
    }

    public void ZoomOut()
    {
        float newView = Camera.main.fieldOfView + zoomStep;
        Camera.main.fieldOfView = Mathf.Clamp(newView, minCamView, maxCamView);
    }



}
