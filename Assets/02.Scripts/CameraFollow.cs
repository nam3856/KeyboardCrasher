using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // 따라갈 대상
    public Vector3 offset;           // 카메라가 떨어져 있을 위치
    public float smoothSpeed = 0.125f;
    public GameObject Target;

    private void Start()
    {
        StarCatchBarUI.Instance.OnStarCatchCompleted += Set;
    }
    void Set()
    {
        target = Target.transform;
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 만약 시점이 고정이라면 LookAt은 생략
    }
}
