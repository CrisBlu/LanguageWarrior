using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TranslationManager : MonoBehaviour
{
    private AudioClip audioClip;
    private string speechToTextUrl = "http://127.0.0.1:5002/capture_speech"; // Flask Speech-to-Text API
    private string translationUrl = "http://127.0.0.1:5001/translate"; // Flask Translation API
    private TextMeshPro textMeshPro3D;

    void Start()
    {
        Create3DText();
    }

    public void StartRecording()
    {
        Debug.Log("🎤 Recording started...");
        audioClip = Microphone.Start(null, false, 5, 44100); // Record for 5 seconds
    }

    public void StopRecording()
    {
        Debug.Log("🛑 Recording stopped.");
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
                StartCoroutine(SendTextToTranslate(response.text)); // Send transcribed text to translation API
            }
            else
            {
                Debug.LogError("❌ Speech-to-Text Failed: " + request.error);
            }
        }
    }

    IEnumerator SendTextToTranslate(string text)
    {
        string jsonData = $"{{\"from_language\":\"es\", \"to_language\":\"en\", \"user_input\":\"{text}\"}}";

        using (UnityWebRequest request = new UnityWebRequest(translationUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseJson = request.downloadHandler.text;
                Debug.Log("✅ Translation Response: " + responseJson);

                TranslationResponse response = JsonUtility.FromJson<TranslationResponse>(responseJson);
                textMeshPro3D.text = response.translated_text;  // ✅ Display translated text
            }
            else
            {
                Debug.LogError("❌ Translation Failed: " + request.error);
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

        GameObject textObject = new GameObject("XRTranslatedText");
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

    [System.Serializable]
    public class SpeechResponse
    {
        public bool success;
        public string text;
    }

    [System.Serializable]
    public class TranslationResponse
    {
        public string from_language;
        public string to_language;
        public string user_input;
        public string translated_text;
    }
}
