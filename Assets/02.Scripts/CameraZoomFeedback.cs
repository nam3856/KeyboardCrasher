using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.Cinemachine;

public class CameraZoomFeedback : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vcam;
    private float defaultFOV = 60f;

    private void Start()
    {
        defaultFOV = vcam.Lens.FieldOfView;
    }

    public async UniTaskVoid SmoothZoomEffect(float targetFOV, float duration)
    {
        float elapsed = 0f;
        float startFOV = vcam.Lens.FieldOfView;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            vcam.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            await UniTask.Yield();
        }

        // 되돌리기
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            vcam.Lens.FieldOfView = Mathf.Lerp(targetFOV, defaultFOV, t);
            await UniTask.Yield();
        }

        vcam.Lens.FieldOfView = defaultFOV;
    }

    public async UniTaskVoid SmoothZoom(float targetFOV, float duration)
    {
        float elapsed = 0f;
        float startFOV = vcam.Lens.FieldOfView;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            vcam.Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            await UniTask.Yield();
        }
    }
}
