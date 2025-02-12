using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    public static byte[] FromAudioClip(AudioClip audioClip)
    {
        using (MemoryStream stream = new MemoryStream())
        {
            ConvertAndWrite(stream, audioClip);
            return stream.ToArray();
        }
    }

    private static void ConvertAndWrite(Stream stream, AudioClip clip)
    {
        int headerSize = 44;
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
            writer.Write(36 + samples.Length * 2);
            writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
            writer.Write(new char[4] { 'f', 'm', 't', ' ' });
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)clip.channels);
            writer.Write(clip.frequency);
            writer.Write(clip.frequency * clip.channels * 2);
            writer.Write((short)(clip.channels * 2));
            writer.Write((short)16);
            writer.Write(new char[4] { 'd', 'a', 't', 'a' });
            writer.Write(samples.Length * 2);

            foreach (float sample in samples)
            {
                writer.Write((short)(Mathf.Clamp(sample, -1f, 1f) * short.MaxValue));
            }
        }
    }
}
