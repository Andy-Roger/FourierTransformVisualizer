using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{
    public int position = 0;
    public int samplerate = 48000;
    public float frequency = 440;

    public fourierManager fourierManager;
    public List<float> harmonics;
    private AudioSource aud;

    void Start()
    {
        AudioClip myClip = AudioClip.Create("clip", samplerate * 3, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        aud = GetComponent<AudioSource>();
        aud.clip = myClip;
        aud.loop = true;
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
            float harmonicSummation = 0;
            for (int i = 0; i < harmonics.Count; i++)
                harmonicSummation = addOscillator(harmonics[i], harmonicSummation);

            data[count] = harmonicSummation * fourierManager.audioGain;

            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }


    float addOscillator(float harmonic, float _harmonicSum)
    {
        float osc;
        if (harmonic < 0)
            osc = Mathf.Sin(Mathf.PI * frequency * position / samplerate / Mathf.Abs(harmonic));
        else
            osc = Mathf.Sin(harmonic * Mathf.PI * frequency * position / samplerate);

        return _harmonicSum + osc;
    }
}