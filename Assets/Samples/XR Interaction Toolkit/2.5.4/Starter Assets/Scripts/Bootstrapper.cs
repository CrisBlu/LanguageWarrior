using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    void Awake()
    {
        GameObject micRecorder = new GameObject("MicRecorder");
        micRecorder.AddComponent<MacMicrophoneRecorder>(); // Attach script automatically
        DontDestroyOnLoad(micRecorder); // Ensure it stays across scene loads
    }
}
