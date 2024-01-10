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
    [SerializeField] private Animator animator;
    private ExtraCamController extraCamController;
    [SerializeField] private float timeToReturnToNormalGravity = 3f;
    private float elapsedTime = 0;
    bool counting = false;
    

    private int bounces;
    void Awake()
    {
        //rb = GetComponent<Rigidbody2D>();
        //extraCamController = GameObject.Find("Extra Cam").GetComponent<ExtraCamController>();
        //extraCamController.SetTeleporter(gameObject);
        if (animator == null) GetComponent<Animator>();
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

    private Collision2D teleCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.tag != "player")
        //collision.contacts[0].normal;
        if (collision.collider.CompareTag("Ice"))
        {
            Debug.Log("touching ice");
            rb.velocity /= 2;
            return;
        }
        if (collision.collider.CompareTag("GravityCollider")) return;
        

        if (collision.collider.CompareTag("KillPlane"))
        {
            player.StartCooldown();
            Destroy(this.gameObject);
            return;
        }
        if (collision.collider.CompareTag("ExtraBounce"))
        {
            Vector2 bounceDir = new Vector2(0,0);
            Vector2 dir = collision.GetContact(0).normal;
            if (dir.x == 1) bounceDir.x = 1;
            if (dir.x == -1) bounceDir.x = -1;
            if (dir.y == 1) bounceDir.y = 1;
            if (dir.y == -1) bounceDir.y = -1;
            /*
            //rb.velocity *= bounceMultiplier;
            Vector3 colPoint = transform.position;
            Vector3 dir = colPoint - collision.gameObject.transform.position;
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0) // Right
                    bounceDir = new Vector2(1, 0);
                else // Left
                    bounceDir = new Vector2(-1, 0);
            }
            else
            {
                if (dir.y > 0) // Up
                    bounceDir = new Vector2(0, 1);
                else // Down
                    bounceDir = new Vector2(0, -1);
            }
            //rb.AddForce(bounceDir * bounceMultiplier);
            //rb.AddForce(bounceDir * GameManager.Instance.GetBounceAmount());
            */
            rb.velocity += bounceDir * GameManager.Instance.GetBounceAmount();
        }

        if (bounces == 0)
        {
            GetComponent<Animator>().Play("Explode");
            teleCollision = collision;
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
        }
            

        bounces--;
    }

    public void TeleporterLanded()
    {
        player.Teleport(teleCollision.contacts[0].point, teleCollision.contacts[0].normal);
        //player.CanFire(true);
        //player.StartCooldown();
        //extraCamController.UnsetTeleporter();
        Destroy(this.gameObject);
    }
}
