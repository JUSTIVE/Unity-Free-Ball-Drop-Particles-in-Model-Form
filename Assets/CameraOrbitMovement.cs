using UnityEngine;
using System.Collections;

public class CameraOrbitMovement : MonoBehaviour
{
    private Vector3 Speed = new Vector3(120.0f, 120.0f);
    private float yMinLimit = -60f;
    private float yMaxLimit = 80f;
    private Vector3 mouseDownCoordinate;
    private float camDist;
    private Vector3 mouseCurrentPosition;
    private float x = 0.0f;
    private float y = 0.0f;
    public Camera cam;
    public GameObject target;
    // Use this for initialization
    void Start()
    {
        InitCamera();
    }
    void InitCamera()
    {
        //cam.transform.position = new Vector3(cam, 0.5f, -1);
        cam.transform.LookAt(target.transform.position, new Vector3(0, 1, 0));
        camDist = Vector3.Distance(cam.transform.position, Vector3.zero);
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }
    void UpdateCamera()
    {
        if (Input.GetMouseButton(0) == true)
        {
            x += Input.GetAxis("Mouse X") * Speed.x * camDist * 0.02f;
            y -= Input.GetAxis("Mouse Y") * Speed.y * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            Quaternion rotation = Quaternion.Euler(y, x, 0);

            Vector3 negDist = new Vector3(0.0f, 0.0f, -camDist);
            Vector3 position = rotation * negDist;

            transform.rotation = rotation;
            transform.position = position;
        }
        cam.transform.LookAt(target.transform.position);
    }
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
