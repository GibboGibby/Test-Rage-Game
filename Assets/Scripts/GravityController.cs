using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    [SerializeField] private float pullForce;
    //[SerializeField] private Rigidbody2D[] objectsToPull;
    Dictionary<int, Rigidbody2D> rigidbodyMap = new Dictionary<int, Rigidbody2D>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (rigidbodyMap.Count == 0) return;
        foreach (Rigidbody2D rb in rigidbodyMap.Values)
        {
            Vector3 dir = rb.gameObject.transform.position - transform.transform.position;
            rb.AddForce(-dir.normalized * pullForce);
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        rigidbodyMap.Add(collision.gameObject.GetInstanceID(), collision.GetComponent<Rigidbody2D>());
    }

    

    private void OnTriggerExit2D(Collider2D collision)
    {
        //rigidbodyMap.Add(collision.gameObject.GetInstanceID(), collision.otherRigidbody);
        rigidbodyMap.Remove(collision.gameObject.GetInstanceID());
    }
    
}
