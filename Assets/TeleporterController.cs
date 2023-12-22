using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerController player;
    [SerializeField] private float power;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bounceMultiplier = 2;
    private ExtraCamController extraCamController;
    [SerializeField] private float timeToReturnToNormalGravity = 3f;
    private float elapsedTime = 0;
    bool counting = false;
    

    private int bounces;
    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        extraCamController = GameObject.Find("Extra Cam").GetComponent<ExtraCamController>();
        extraCamController.SetTeleporter(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (counting)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToReturnToNormalGravity)
            {
                elapsedTime = 0;
                rb.gravityScale = 1;
                counting = false;
            }
        }
    }

    public void SetBounces(int b)
    {
        bounces = b;
    }

    public void SetPlayer(PlayerController pc)
    {
        player = pc;
    }

    public void SetPower(float value)
    {
        power = value;
    }

    public void Fire(Vector3 normalisedDir)
    {
        rb.AddForce(normalisedDir * power, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NegativeGravity")) rb.gravityScale = -1;
        if (collision.gameObject.CompareTag("NoGravity"))
        {
            Debug.Log("Touching no grav");
            rb.gravityScale = 0;
        }
        if (collision.gameObject.CompareTag("PositiveGravity")) rb.gravityScale = 1;

        counting = false;
        elapsedTime = 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (rb.gravityScale == 0 || rb.gravityScale == -1)
        {
            elapsedTime = 0;
            counting = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.tag != "player")
        //collision.contacts[0].normal;

        if (collision.collider.CompareTag("GravityCollider")) return;
        

        if (collision.collider.CompareTag("KillPlane"))
        {
            player.StartCooldown();
            Destroy(this.gameObject);
            return;
        }
        if (collision.collider.CompareTag("ExtraBounce"))
        {
            rb.velocity *= bounceMultiplier;
        }

        if (bounces == 0)
        {
            player.Teleport(collision.contacts[0].point, collision.contacts[0].normal);
            //player.CanFire(true);
            player.StartCooldown();
            extraCamController.UnsetTeleporter();
            Destroy(this.gameObject);
        }
            

        bounces--;
    }
}
