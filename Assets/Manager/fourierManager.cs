using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fourierManager : MonoBehaviour
{
    public List<float> harmonics = new List<float>();
    public GameObject audioVisual;

    public List<GameObject> audioVisualsObjects;

    public GameObject oscillatorInput;
    public GameObject oscillatorInputParent;

    public float audioGain = 1;

    void Start()
    {
        harmonics.Add(1);

        for (int i = 0; i < 55; i++)
            audioVisualsObjects.Add(new GameObject());
    }

    void Update()
    {
        keyUp();
        keyDown();
    }

    void createWave(int halfStepsFromA440)
    {
        int keyId = halfStepsFromA440;
        float hz = 440 * Mathf.Pow(1.059463094359f, halfStepsFromA440);

        var newAudioVisual = Instantiate(audioVisual);
        var armController = newAudioVisual.GetComponentInChildren<armController>();
        var audioController = newAudioVisual.GetComponentInChildren<audioController>();

        destroyWave(keyId + 9);
        audioVisualsObjects[keyId + 9] = newAudioVisual;

        audioController.harmonics = harmonics;
        armController.harmonics = harmonics;
        audioController.fourierManager = this;
        armController.fourierManager = this;
        audioController.frequency = hz;
        armController.speed = hz / 3;
    }

    void destroyWave(int keyId)
    {
        if(audioVisualsObjects[keyId + 9] != null)
        {
            Destroy(audioVisualsObjects[keyId + 9]);
        }
    }

    public void addOscillator()
    {
        float addOne = harmonics[harmonics.Count - 1] + 1;
        var newOscillatorInput = Instantiate(oscillatorInput, oscillatorInputParent.transform);

        newOscillatorInput.GetComponent<InputField>().text = addOne.ToString();

        harmonics.Add(float.Parse(newOscillatorInput.GetComponent<InputField>().text));
        newOscillatorInput.GetComponent<InputField>().onEndEdit.AddListener(delegate { onOscillatorValueChanged(newOscillatorInput.transform.GetSiblingIndex()); });
        newOscillatorInput.GetComponentInChildren<Button>().onClick.AddListener(delegate { removeOscillator(newOscillatorInput.transform.GetSiblingIndex()); });
    }

    void removeOscillator(int childIndex)
    {
        harmonics.RemoveAt(childIndex + 1);
        Destroy(oscillatorInputParent.transform.GetChild(childIndex).gameObject);
    }

    public void onOscillatorValueChanged(int childIndex)
    {
        float value = float.Parse(oscillatorInputParent.transform.GetChild(childIndex).GetComponent<InputField>().text);
        harmonics[childIndex + 1] = value;
    }

    void keyUp()
    {
        if (Input.GetKeyUp(KeyCode.A))
            destroyWave(-8);
        if (Input.GetKeyUp(KeyCode.W))
            destroyWave(-7);
        if (Input.GetKeyUp(KeyCode.S))
            destroyWave(-6);
        if (Input.GetKeyUp(KeyCode.E))
            destroyWave(-5);
        if (Input.GetKeyUp(KeyCode.D))
            destroyWave(-4);
        if (Input.GetKeyUp(KeyCode.F))
            destroyWave(-3);
        if (Input.GetKeyUp(KeyCode.T))
            destroyWave(-2);
        if (Input.GetKeyUp(KeyCode.G))
            destroyWave(-1);
        if (Input.GetKeyUp(KeyCode.Y))
            destroyWave(0);
        if (Input.GetKeyUp(KeyCode.H)) //A 440hz
            destroyWave(1);
        if (Input.GetKeyUp(KeyCode.U))
            destroyWave(2);
        if (Input.GetKeyUp(KeyCode.J))
            destroyWave(3);
        if (Input.GetKeyUp(KeyCode.K))
            destroyWave(4);
        if (Input.GetKeyUp(KeyCode.O))
            destroyWave(5);
    }

    void keyDown()
    {
        if (Input.GetKeyDown(KeyCode.A))
            createWave(-8);
        if (Input.GetKeyDown(KeyCode.W))
            createWave(-7);
        if (Input.GetKeyDown(KeyCode.S))
            createWave(-6);
        if (Input.GetKeyDown(KeyCode.E))
            createWave(-5);
        if (Input.GetKeyDown(KeyCode.D))
            createWave(-4);
        if (Input.GetKeyDown(KeyCode.F))
            createWave(-3);
        if (Input.GetKeyDown(KeyCode.T))
            createWave(-2);
        if (Input.GetKeyDown(KeyCode.G))
            createWave(-1);
        if (Input.GetKeyDown(KeyCode.Y))
            createWave(0);
        if (Input.GetKeyDown(KeyCode.H)) //A 440hz (if no octave shifting)
            createWave(1);
        if (Input.GetKeyDown(KeyCode.U))
            createWave(2);
        if (Input.GetKeyDown(KeyCode.J))
            createWave(3);
        if (Input.GetKeyDown(KeyCode.K))
            createWave(4);
        if (Input.GetKeyDown(KeyCode.O))
            createWave(5);
    }
}
