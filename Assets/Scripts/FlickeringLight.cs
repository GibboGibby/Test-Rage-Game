using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FlickeringLight : MonoBehaviour
{

    [SerializeField] private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float maxChange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        light.intensity += Random.Range(-maxChange, maxChange);
        light.intensity = Mathf.Clamp(light.intensity, minIntensity, maxIntensity);

    }
}
