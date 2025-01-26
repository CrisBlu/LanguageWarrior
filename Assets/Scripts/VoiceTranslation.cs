using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class VoiceTranslation : MonoBehaviour
{
    // URL of your Flask API
    private string apiUrl = "https://0e5c-18-29-9-169.ngrok-free.app/translate";

    private AudioClip audioClip;
    private string audioFilePath;

    // Start recording audio
    public void StartRecording()
    {
        audioClip = Microphone.Start(null, false, 10, 44100); // Record for 10 seconds
        Debug.Log("Recording started...");
    }

    // Stop recording audio
    public void StopRecording()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            Debug.Log("Recording stopped...");
            SaveAudioToWav();
        }
    }

    // Save the recorded audio as a WAV file
    private void SaveAudioToWav()
    {
        audioFilePath = Path.Combine(Application.persistentDataPath, "recorded_audio.wav");

        // Convert AudioClip data to WAV
        byte[] wavData = ConvertAudioClipToWav(audioClip);

        // Write the WAV data to a file
        File.WriteAllBytes(audioFilePath, wavData);
        Debug.Log($"Audio saved to: {audioFilePath}");

        // Send the audio to the Flask backend
        StartCoroutine(UploadAudio());
    }

    // Upload the audio file to the Flask backend
    private IEnumerator UploadAudio()
    {
        Debug.Log("Uploading audio...");
        WWWForm form = new WWWForm();
        form.AddBinaryData("audio", File.ReadAllBytes(audioFilePath), "recorded_audio.wav", "audio/wav");

        using (UnityWebRequest www = UnityWebRequest.Post(apiUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
    }

    // Convert an AudioClip to a WAV file (manually)
    private byte[] ConvertAudioClipToWav(AudioClip clip)
    {
        int headerSize = 44;
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        short[] intData = new short[samples.Length];
        byte[] bytesData = new byte[samples.Length * 2];
        int rescaleFactor = 32767; // Convert to 16-bit integer

        for (int i = 0; i < samples.Length; i++)
        {
            intData[i] = (short)(samples[i] * rescaleFactor);
            byte[] byteArray = System.BitConverter.GetBytes(intData[i]);
            byteArray.CopyTo(bytesData, i * 2);
        }

        byte[] wavData = new byte[headerSize + bytesData.Length];
        WriteWavHeader(wavData, clip);
        bytesData.CopyTo(wavData, headerSize);

        return wavData;
    }

    // Write WAV header
    private void WriteWavHeader(byte[] wavData, AudioClip clip)
    {
        int sampleRate = clip.frequency;
        int channels = clip.channels;
        int samples = clip.samples;

        System.Text.Encoding.UTF8.GetBytes("RIFF").CopyTo(wavData, 0); // Chunk ID
        System.BitConverter.GetBytes(wavData.Length - 8).CopyTo(wavData, 4); // Chunk Size
        System.Text.Encoding.UTF8.GetBytes("WAVE").CopyTo(wavData, 8); // Format
        System.Text.Encoding.UTF8.GetBytes("fmt ").CopyTo(wavData, 12); // Subchunk1 ID
        System.BitConverter.GetBytes(16).CopyTo(wavData, 16); // Subchunk1 Size
        System.BitConverter.GetBytes((short)1).CopyTo(wavData, 20); // Audio Format (PCM)
        System.BitConverter.GetBytes((short)channels).CopyTo(wavData, 22); // Num Channels
        System.BitConverter.GetBytes(sampleRate).CopyTo(wavData, 24); // Sample Rate
        System.BitConverter.GetBytes(sampleRate * channels * 2).CopyTo(wavData, 28); // Byte Rate
        System.BitConverter.GetBytes((short)(channels * 2)).CopyTo(wavData, 32); // Block Align
        System.BitConverter.GetBytes((short)16).CopyTo(wavData, 34); // Bits Per Sample
        System.Text.Encoding.UTF8.GetBytes("data").CopyTo(wavData, 36); // Subchunk2 ID
        System.BitConverter.GetBytes(samples * channels * 2).CopyTo(wavData, 40); // Subchunk2 Size
    }
}