using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 440;

    public fourierManager fourierManager;
    public List<float> harmonics;
    private AudioSource aud;

    void Start()
    {
        AudioClip myClip = AudioClip.Create("clip", samplerate * 10, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.Play();
    }

    void Update()
    {
        harmonics = fourierManager.harmonics;
    }

    void OnAudioRead(float[] data)
    {
        int count = 0;

        while (count < data.Length)
        {
            if (harmonics[count % harmonics.Count] < 0)
                data[count] = Mathf.Sin((2) * Mathf.PI * (frequency / Mathf.Abs(harmonics[count % harmonics.Count])) * position / samplerate); //frequency lower than fundamental
            else
                data[count] = Mathf.Sin((2) * Mathf.PI * (frequency * harmonics[count % harmonics.Count]) * position / samplerate); //overtone/harmonics

            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }
}