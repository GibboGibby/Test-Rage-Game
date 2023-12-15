using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float speed = 2.0f;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [SerializeField] private float camSizeMin;
    [SerializeField] private float camSizeMax;

    bool shouldClamp = false;
    // Start is called before the first frame update
    void Start()
    {
        //cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        CamMove();
        CamZoom();
    }

    void CamMove()
    {
        float interp = speed * Time.deltaTime;

        Vector3 pos = transform.position;

        pos.y = Mathf.Lerp(transform.position.y, player.transform.position.y, interp);
        pos.x = Mathf.Lerp(transform.position.x, player.transform.position.x, interp);

        if (shouldClamp)
        {
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
        }
        transform.position = pos;
    }

    void CamZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            cam.orthographicSize -= Input.mouseScrollDelta.y;

            if (cam.orthographicSize > camSizeMax) cam.orthographicSize = camSizeMax;
            if (cam.orthographicSize < camSizeMin) cam.orthographicSize = camSizeMin;
        }

    }

}
