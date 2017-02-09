using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject FollowTarget;             //Target que la caméra va suivre
    public float CameraMoveSpeed;               //Vitesse de la caméra qui suit la target
    private Vector3 _targetPosition;

    void Start()
    {

    }

    void FixedUpdate()
    {

        _targetPosition = new Vector3(FollowTarget.transform.position.x, FollowTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, _targetPosition, CameraMoveSpeed * Time.fixedDeltaTime);

    }
}
