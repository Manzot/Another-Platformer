using UnityEngine;

public class CameraLerpToTransform : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float cameraDepth = -10f;
    public float minX, minY, maxX, maxY;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = Vector2.Lerp(transform.position, target.position, Time.deltaTime * speed);
        Vector3 camPosition = new Vector3(newPosition.x, newPosition.y, cameraDepth);
        Vector3 v3 = camPosition;
        float newX = Mathf.Clamp(v3.x, minX, maxX);
        float newY = Mathf.Clamp(v3.y, minY, maxY);
        transform.position = new Vector3(newX, newY, cameraDepth);
    }
}
