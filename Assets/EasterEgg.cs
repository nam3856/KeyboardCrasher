using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    public CameraZoomFeedback cam;

    public ScreenFlashEffect eff;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        cam.SmoothZoomEffect(100, 2f).Forget();
        eff.FlashBloom(3f, 2f).Forget();
    }


}
