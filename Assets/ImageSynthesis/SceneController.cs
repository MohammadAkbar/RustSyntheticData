using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class SceneController : MonoBehaviour
{
    [Header("How many Images to Generate?")]
    public int HowManyImages = 1000;

    [Header("Camera Distance")]
    public float distanceMin = 20f;
    public float distanceMax = 40f;

    [Header("Image Size")]
    public int ImgSizeX = 256;
    public int ImgSizeY = 256;

    [Header("Don't Mess with These")]
    public ImageSynthesis synth;
    public GameObject maincam;
    public GameObject target;
    public Slider slider;
    public GameObject DirectionalLight;

    private int frameCount = 0;

    Vector3 RandomBetweenRadius3D(float minRad, float maxRad)
    {
        float diff = maxRad - minRad;
        Vector3 point = Vector3.zero;
        while (point == Vector3.zero)
        {
            point = UnityEngine.Random.insideUnitSphere;
        }
        point = point.normalized * (UnityEngine.Random.value * diff + minRad);
        point.y = Mathf.Abs(point.y) + 1;
        return point;
    }

    void moveCamera()
    {
        maincam.transform.position = RandomBetweenRadius3D(distanceMin, distanceMax);
        maincam.transform.LookAt(target.transform.position);
    }
    void rotateLight()
    {
        float rotX = UnityEngine.Random.Range(20.0f, 70.0f);
        float rotY = UnityEngine.Random.Range(0.0f, 360.0f);
        DirectionalLight.transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount < HowManyImages)
        {
            moveCamera();
            rotateLight();
            string filename = $"{frameCount.ToString().PadLeft(10, '0')}";
            synth.Save(filename, ImgSizeX, ImgSizeY, "syntheticData");

            synth.OnSceneChange();
            frameCount++;
            slider.value = (float)frameCount / (float)HowManyImages;
        }
        else
        {
            EditorApplication.isPlaying = false;
        }

    }
}
