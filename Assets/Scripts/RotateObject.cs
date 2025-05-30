using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector;

    [SerializeField] private float rotationSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newRotationSpeed = rotationSpeed * 100;
        transform.Rotate(rotationVector*newRotationSpeed*Time.deltaTime);
    }
}
