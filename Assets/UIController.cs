using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField] private GameObject[] middleBits;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool followCursor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!followCursor) return;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        newPos.z = 0;
        transform.position = newPos;
    }

    public void UpdateUI(int elementsToShow)
    {
        for (int i = 0; i < middleBits.Length; i++)
        {
            if (i < elementsToShow)
                middleBits[i].SetActive(true);
            else                  
                middleBits[i].SetActive(false);
        }
    }
}
