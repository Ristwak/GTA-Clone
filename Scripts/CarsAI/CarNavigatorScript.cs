using UnityEngine;

public class CarNavigatorScript : MonoBehaviour
{
    [Header("Charcater Info")]
    public float movingSpeed;
    public float turningSpeed = 300f;
    public float stopSpeed = 1f;
    public Transform[] wheels;
    [Header("Destination Variable")]
    public Vector3 destination;
    public bool destinationReached;

    void Update()
    {
        Walk();
    }

    public void Walk()
    {
        Vector3 destinationDir = destination - transform.position;
        destinationDir.y = 0f;
        float destinationDistance = destinationDir.magnitude;
        if (destinationDistance >= stopSpeed)
        {
            destinationReached = false;
            Quaternion targetRotation = Quaternion.LookRotation(destinationDir);
            float angleToDestination = Quaternion.Angle(transform.rotation, targetRotation);
            if (angleToDestination > 1f)
            {
                float adjustedTurningSpeed = turningSpeed * Mathf.Clamp01(destinationDistance / 10f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, adjustedTurningSpeed * Time.deltaTime);
            }
            transform.Translate(Vector3.forward * movingSpeed * Time.deltaTime);
            RotateWheels();
        }
        else
        {
            destinationReached = true;
        }
    }

    public void LocateDestination(Vector3 destination)
    {
        this.destination = destination;
        destinationReached = false;
    }

    void RotateWheels()
    {
        if (wheels == null)
            return;
        foreach (Transform wheel in wheels)
        {
            if (wheel != null)
                wheel.Rotate(Vector3.right, movingSpeed * Time.deltaTime * 200f);
        }
    }
}