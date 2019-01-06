using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armController : MonoBehaviour
{
    public GameObject rotatorArm;
    private List<GameObject> rotatorArms = new List<GameObject>();

    private float firstArmLength = 20f;
    public float thickness = 0.5f;
    public float speed = 10f;

    public LineRenderer lineRenderer;
    public TrailRenderer trailRenderer;

    public fourierManager fourierManager;
    public List<float> harmonics;

    private GameObject firstArm;

    void Start()
    {
        createArms(harmonics);
        lineRenderer = Instantiate(lineRenderer);
        trailRenderer = Instantiate(trailRenderer);
        trailRenderer.transform.position = new Vector3(rotatorArms[0].transform.position.x + 3, rotatorArms[rotatorArms.Count - 1].transform.position.y, 0);
    }

    void Update()
    {
        rotateArms();
        updateLine();
        updateTrail();
    }

    void createArms(List<float> _nValues)
    {
        if (rotatorArms.Count > 1)
            Destroy(rotatorArms[0]);
        rotatorArms.Clear();
        for (int i = 0; i < _nValues.Count + 1; i++)
        {
            GameObject arm;
            if (i == 0)
            {
                arm = Instantiate(rotatorArm, Camera.main.transform.position + transform.forward, Quaternion.identity, Camera.main.transform);
                arm.transform.GetChild(0).localScale = new Vector3(firstArmLength, thickness, thickness);
                rotatorArms.Add(arm);
                firstArm = arm;
            }
            else if (i < _nValues.Count)
            {
                arm = Instantiate(rotatorArm, rotatorArms[i - 1].transform);
                arm.transform.localPosition = new Vector3(rotatorArms[i - 1].transform.GetChild(0).localScale.x, 0, 0);
                arm.transform.localScale = Vector3.one;
                arm.transform.GetChild(0).localScale = new Vector3(firstArmLength / _nValues[i], thickness, thickness);
                arm.transform.GetChild(0).localPosition = new Vector3(arm.transform.GetChild(0).localScale.x / 2, 0, 0);
                rotatorArms.Add(arm);
            }
            else
            {
                arm = Instantiate(rotatorArm, rotatorArms[i - 1].transform);
                arm.transform.localPosition = new Vector3(rotatorArms[i - 1].transform.GetChild(0).localScale.x, 0, 0);
                arm.transform.localScale = Vector3.one;
                Destroy(arm.transform.GetChild(0).gameObject);
                rotatorArms.Add(arm);
            }
        }
    }

    void rotateArms()
    {
        float harmonicSum = 0;
        float harmonicSkips = 0;
        for (int i = 0; i < rotatorArms.Count - 1; i++)
        {
            float armSpeed;
            harmonicSum += harmonics[i];
            armSpeed = (speed * (harmonicSum - i)) - ((harmonicSum - harmonics[i]) * speed);
            var removeSkipsSpeed = armSpeed - (speed * harmonicSkips);
            if (i > 0)
                harmonicSkips += (harmonics[i] - 1) - harmonics[i - 1];

            rotatorArms[i].transform.Rotate(0, 0, removeSkipsSpeed * Time.deltaTime);
        }
    }

    void updateLine()
    {
        var tip = rotatorArms[rotatorArms.Count - 1].transform.position;
        var rightToCenter = rotatorArms[0].transform.position.x + 3;
        lineRenderer.SetPosition(0, tip);
        lineRenderer.SetPosition(1, new Vector3(rightToCenter, tip.y, rotatorArms[0].transform.position.z));
    }

    void updateTrail()
    {
        trailRenderer.transform.position = lineRenderer.GetPosition(1);
    }

    void OnDisable()
    {
        Destroy(firstArm);
        Destroy(lineRenderer.gameObject);
    }
}
