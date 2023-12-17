using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    // Start is called before the first frame update

    private PlayerController player;
    [SerializeField] private float power;
    [SerializeField] private Rigidbody2D rb;
    private ExtraCamController extraCamController;

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
