using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtraCamController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject teleporter;
    [SerializeField] private GameObject extraCam;
    [SerializeField] private RawImage image;
    [SerializeField] private GameObject cameraStuff;
    private 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (teleporter == null)
        {
            //image.enabled = false;
            cameraStuff.SetActive(false);
        }
        else
        {
            //image.enabled = true;
            cameraStuff.SetActive(true);
            extraCam.transform.position = new Vector3(teleporter.transform.position.x, teleporter.transform.position.y, extraCam.transform.position.z);
        }
    }

    public void SetTeleporter(GameObject teleporter)
    {
        this.teleporter = teleporter;
    }

    public void UnsetTeleporter()
    {
        teleporter = null;
    }
}
