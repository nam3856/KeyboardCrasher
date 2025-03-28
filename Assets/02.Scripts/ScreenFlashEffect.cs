using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenFlashEffect : MonoBehaviour
{
    [SerializeField] private Volume volume;


    public async UniTaskVoid FlashBloom(float maxIntensity = 10f, float duration = 0.2f)
    {
        if (!volume.profile.TryGet(out Bloom bloom)) return;

        float halfDuration = duration / 2f;

        // π‡±‚ ¡ı∞°
        for (float t = 0; t < halfDuration; t += Time.unscaledDeltaTime)
        {
            float lerp = t / halfDuration;
            bloom.intensity.value = Mathf.Lerp(0f, maxIntensity, lerp);
            await UniTask.Yield();
        }

        // π‡±‚ ∞®º“
        for (float t = 0; t < halfDuration; t += Time.unscaledDeltaTime)
        {
            float lerp = t / halfDuration;
            bloom.intensity.value = Mathf.Lerp(maxIntensity, 0f, lerp);
            await UniTask.Yield();
        }

        bloom.intensity.value = 0f;
    }

    public async UniTaskVoid VignetteStart(float maxIntensity = 0.34f, float duration = 1f)
    {
        if (!volume.profile.TryGet(out Vignette vignette)) return;


        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            float lerp = t / duration;
            vignette.intensity.value = Mathf.Lerp(0f, maxIntensity, lerp);
            await UniTask.Yield();
        }

    }

    public async UniTaskVoid ChromaticStart(float maxIntensity = 0.34f, float duration = 1f)
    {
        if (!volume.profile.TryGet(out ChromaticAberration chrome)) return;


        for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
        {
            float lerp = t / duration;
            chrome.intensity.value = Mathf.Lerp(0f, maxIntensity, lerp);
            await UniTask.Yield();
        }

    }
}
