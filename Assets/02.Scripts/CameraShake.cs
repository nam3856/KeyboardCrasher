using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineImpulseSource impulseSource;

    public void Shake()
    {
        impulseSource.GenerateImpulse();
    }
}