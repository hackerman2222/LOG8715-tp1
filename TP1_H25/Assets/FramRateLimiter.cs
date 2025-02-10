using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    void Start()
    {
        // VSync 
        QualitySettings.vSyncCount = 1;
        // Fixe le framerate à 30 FPS
        Application.targetFrameRate = 30;
    }
}