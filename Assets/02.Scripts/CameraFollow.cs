using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;


    private void Start()
    {
        StarCatchUI.Instance.OnStarCatchCompleted += Set;
    }
    void Set()
    {
        var follow = cinemachineCamera.GetComponent<CinemachineFollow>();
        if (follow == null)
        {
            follow = cinemachineCamera.gameObject.AddComponent<CinemachineFollow>();
        }
        follow.TrackerSettings.PositionDamping = new Vector3(0.1f, 0.1f);

    }
}
