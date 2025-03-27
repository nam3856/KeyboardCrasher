using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         // ���� ���
    public Vector3 offset;           // ī�޶� ������ ���� ��ġ
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

        // ���� ������ �����̶�� LookAt�� ����
    }
}
