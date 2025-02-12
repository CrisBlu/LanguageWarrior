using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class MacMicrophoneRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private string speechToTextUrl = "http://127.0.0.1:5002/capture_speech"; // Flask Speech-to-Text API
    private bool isRecording = false;
    private TextMeshPro textMeshPro3D;
    private Coroutine captionCoroutine;

    void Start()
    {
        RequestMicrophonePermission();
        Create3DText();
        StartCoroutine(ContinuousRecording());
    }

    void RequestMicrophonePermission()
    {
#if UNITY_STANDALONE_OSX
        Application.RequestUserAuthorization(UserAuthorization.Microphone);
#endif
    }

    IEnumerator ContinuousRecording()
    {
        while (true)
        {
            if (!isRecording)
            {
                StartRecording();
                yield return new WaitForSeconds(5); // Record for 5 seconds
                StopRecording();
            }
            yield return new WaitForSeconds(0.5f); // Small delay before next recording
        }
    }

    void StartRecording()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("❌ No microphone detected!");
            return;
        }

        Debug.Log("🎤 Recording started...");
        isRecording = true;
        audioClip = Microphone.Start(null, false, 5, 44100); // 5-sec recording
    }

    void StopRecording()
    {
        if (!isRecording) return;

        Debug.Log("🛑 Recording stopped.");
        isRecording = false;
        Microphone.End(null);
        StartCoroutine(SendAudioToFlask());
    }

    IEnumerator SendAudioToFlask()
    {
        byte[] audioData = WavUtility.FromAudioClip(audioClip);
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", audioData, "speech.wav", "audio/wav");

        using (UnityWebRequest request = UnityWebRequest.Post(speechToTextUrl, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                Debug.Log("✅ Speech-to-Text Response: " + responseJson);

                SpeechResponse response = JsonUtility.FromJson<SpeechResponse>(responseJson);

                if (!string.IsNullOrEmpty(response.translated_text))
                {
                    ShowCaption(response.translated_text);
                }
            }
            else
            {
                Debug.LogError("❌ Speech-to-Text Failed: " + request.error);
            }
        }
    }

    void Create3DText()
    {
        Debug.Log("🟡 Creating 3D Text...");

        Camera xrCamera = Camera.main;
        if (xrCamera == null)
        {
            Debug.LogError("❌ No main camera found! Ensure your XR camera is tagged as 'MainCamera'.");
            return;
        }

        GameObject textObject = new GameObject("XRTranscribedText");
        textMeshPro3D = textObject.AddComponent<TextMeshPro>();
        textMeshPro3D.text = "";
        textMeshPro3D.fontSize = 10;
        textMeshPro3D.alignment = TextAlignmentOptions.Center;
        textMeshPro3D.color = Color.green;

        textObject.transform.position = xrCamera.transform.position + xrCamera.transform.forward * 3;
        textObject.transform.LookAt(xrCamera.transform);
        textObject.transform.Rotate(0, 180, 0);

        textObject.transform.localScale = Vector3.one * 0.1f;
        Debug.Log("✅ 3D Text Created Successfully");
    }

    void ShowCaption(string text)
    {
        if (captionCoroutine != null)
        {
            StopCoroutine(captionCoroutine);
        }

        textMeshPro3D.text = text;  // ✅ Display new caption
        captionCoroutine = StartCoroutine(HideCaptionAfterDelay(5)); // Hide after 5 sec
    }

    IEnumerator HideCaptionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        textMeshPro3D.text = ""; // Clear text
    }

    [System.Serializable]
    public class SpeechResponse
    {
        public bool success;
        public string transcribed_text;
        public string translated_text;
    }
}
