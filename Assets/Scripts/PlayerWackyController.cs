using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWackyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, moveSpeed, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, -moveSpeed, 0) * Time.deltaTime;
        }
    }
}
