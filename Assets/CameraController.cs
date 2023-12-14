using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private GameObject player;
    [SerializeField] private float speed = 2.0f;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    bool shouldClamp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
}
