using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTS_Cam : MonoBehaviour
{
    public Camera cam;
    public float zoomSpeed;
    public Vector2 mouseLook;
    public float sensitivity;
    public float moveSpeed;

    private Vector3 startPos;


    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            transform.position = startPos;
        }


        HandleMove();

        HandleRot();

        HandleZoom();

    }

    private void HandleMove()
    {
        //--moves forward/back/left/right relative to tranforms rotation
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(x, 0f, z) * moveSpeed * Time.deltaTime;

        transform.position += transform.TransformDirection(moveDirection);

    }

    private void HandleRot()
    {
        if (Input.GetMouseButton(2))
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            Vector2 look = new Vector2(horizontal, vertical);
            mouseLook += look * sensitivity;

            //mouseLook.y = Mathf.Clamp(mouseLook.y, 30, -70);

            //cam.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
        }
    }

    private void HandleZoom()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        cam.fieldOfView += -zoom;

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 40f, 80f);
    }
}
