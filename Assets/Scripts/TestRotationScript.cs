using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotationScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject arm;
    [SerializeField] private Transform armRotPoint;

    [SerializeField] private SpriteRenderer sr;
    void Start()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerDist = player.transform.position - mousePos;
        playerDist.z = 0;
        Debug.Log(playerDist.x);
        sr.flipX = (playerDist.x > 0.0f) ? true : false;
        mousePos.z = armRotPoint.position.z;
        Vector3 dist = mousePos - armRotPoint.position;
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        armRotPoint.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
    }

}
