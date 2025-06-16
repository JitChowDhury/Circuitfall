using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 120;
    [Header(("Rotation Details"))]
    [SerializeField] private Transform focusPoint;
    [SerializeField] private float rotationSpeed = 200f;
    [Space] 
    private float pitch;
    [SerializeField]private float minPitch = 5f;
    [SerializeField]private float maxPitch = 85f;
    [SerializeField] private float maxFocusPointDistance = 15;
    
    [Header("Zoom details")]
    [SerializeField] private float zoomSpeed=10;
    [SerializeField] private float minZoom = 3;
    [SerializeField] private float maxZoom = 15;
    
    
    
    private float smoothTime = .1f;
    private Vector3 movementVelocity = Vector3.zero;
    private Vector3 zoomVelocity = Vector3.zero;

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();

        focusPoint.position = transform.position + (transform.forward * getFocusPointDistance());
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoomDirection = transform.forward * scroll * zoomSpeed;
        Vector3 targetPosition = transform.position + zoomDirection;

        if (transform.position.y < minZoom && scroll > 0)
       return;
        if(transform.position.y>maxZoom &&scroll<0)return;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref zoomVelocity, smoothTime);

    }

    private float getFocusPointDistance()
    {
        if (Physics.Raycast(transform.position, transform.forward,out RaycastHit hit, maxFocusPointDistance))
        {
            return hit.distance;
        }

        return maxFocusPointDistance;
    }
    private void HandleRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;


            pitch = Mathf.Clamp(pitch - verticalRotation, minPitch, maxPitch);
            transform.RotateAround(focusPoint.position,Vector3.up,horizontalRotation);
            transform.RotateAround(focusPoint.position,transform.right,pitch-transform.eulerAngles.x);
            transform.LookAt(focusPoint);
        }
    }

private void HandleMovement()
    {
        Vector3 targetPosition = transform.position;

        float vInput = Input.GetAxisRaw("Vertical");
        float hInput = Input.GetAxisRaw("Horizontal");

        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
;
        if (hInput > 0) targetPosition += transform.right * movementSpeed * Time.deltaTime;
        if (hInput < 0) targetPosition -= transform.right * movementSpeed * Time.deltaTime;

        if (vInput > 0) targetPosition += flatForward * movementSpeed * Time.deltaTime;
        if (vInput < 0) targetPosition -= flatForward * movementSpeed * Time.deltaTime;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition,ref movementVelocity, smoothTime);
    }
}
