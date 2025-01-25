using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class backend_connection : MonoBehaviour
{


    // URL of your Flask API
    private string apiUrl = "http://localhost:5001/translate";

    // Function to send a translation request to the backend
    public void TranslateText(string fromLanguage, string toLanguage, string userInput)
    {
        StartCoroutine(SendTranslationRequest(fromLanguage, toLanguage, userInput));
    }

    // Coroutine to send POST request to Flask API
    private IEnumerator SendTranslationRequest(string fromLanguage, string toLanguage, string userInput)
    {
        Debug.Log("Sending translation request...");
        // Create a JSON object to send to the API
        string jsonData = JsonUtility.ToJson(new TranslationRequest
        {
            from_language = fromLanguage,
            to_language = toLanguage,
            user_input = userInput
        });

        // Prepare the request (POST method)
        using (UnityWebRequest www = new UnityWebRequest(apiUrl, "POST"))
        {
            // Add the JSON data to the request body
            byte[] jsonToSend = new UTF8Encoding().GetBytes(jsonData);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for a response
            yield return www.SendWebRequest();

            // Check if the request was successful
            if (www.result == UnityWebRequest.Result.Success)
            {
                // Parse the JSON response
                string jsonResponse = www.downloadHandler.text;
                TranslationResponse translationResponse = JsonUtility.FromJson<TranslationResponse>(jsonResponse);

                // Output the translation result
                Debug.Log("Translated Text: " + translationResponse.translated_text);
            }
            else
            {
                Debug.LogError("Request failed: " + www.error);
            }
        }
    }

    void Start()
    {
        // Example usage
        TranslateText("English", "portuguese", "Hello, how are you?");
    }

    // Data structure to hold the request data
    [System.Serializable]
    public class TranslationRequest
    {
        public string from_language;
        public string to_language;
        public string user_input;
    }

    // Data structure to hold the response data
    [System.Serializable]
    public class TranslationResponse
    {
        public string translated_text;
    }

}
