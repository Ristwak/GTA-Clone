using UnityEngine;

public class CarCameraController : MonoBehaviour
{
    public Transform carTransform;
    public float distanceBehind = 5f;
    public float height = 2f;
    public float smoothSpeed = 0.1f;

    void LateUpdate()
    {
        if (carTransform == null)
            return;
        Vector3 desiredPosition = carTransform.position - carTransform.forward * distanceBehind + Vector3.up * height;
        float smoothFactor = 1f - Mathf.Pow(1f - Mathf.Clamp01(smoothSpeed), Time.deltaTime * 60f);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFactor);
        transform.position = smoothPosition;
        transform.LookAt(carTransform.position);
    }
}