using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int maxBounces;
    [SerializeField] private int minBounces;

    [SerializeField] private ExtraCamController ecc;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private LineRenderer lr;

    [SerializeField] private float minPower;
    [SerializeField] private float maxPower;
    [SerializeField] private float maxDistForFullPower;

    [SerializeField] private int lineSteps;

    [SerializeField] private GameObject debugTeleportPos;

    private bool canFire = true;
    //[SerializeField] private bool shotCooldown = false;
    [SerializeField] private float cooldownTime = 0.0f;

    [SerializeField] private GameObject teleporterPrefab;

    [SerializeField] private UIController uiController;

    [SerializeField] private float shootForce = 10.0f;
    private int bounces;

    private bool readying;
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (lr == null) lr = GetComponent<LineRenderer>();

        if (debugTeleportPos == null) debugTeleportPos = GameObject.Find("DebugTeleporter");

        lr.positionCount = 0;
    }

    private IEnumerator ShowCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
        //return;
    }

    public void StartCooldown()
    {
        StartCoroutine(ShowCooldown());
    }

    private void Movement()
    {
        if (readying) return;
        if (Input.GetKey(KeyCode.D))
        {
            //rb.AddForce(transform.right * moveSpeed, ForceMode2D.Force);
            transform.position += transform.right * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //rb.AddForce(-transform.right * moveSpeed, ForceMode2D.Force);
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
        }
    }

    public void Teleport(Vector2 pos, Vector2 normal)
    {
        Vector2 scale = new Vector2(transform.localScale.x / 2, transform.localScale.y / 2);
        //Vector2 scale = new Vector2(sr.bounds.size.x / 2, sr.bounds.size.y / 2);
        Vector2 movementAmt = normal * scale;
        transform.position = pos + movementAmt;
    }

    private void Fire()
    {
        //Debug.Log("Fired!");
        GameObject teleporter = Instantiate(teleporterPrefab, transform.position, Quaternion.identity);
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        TeleporterController te = teleporter.GetComponent<TeleporterController>();
        te.SetPlayer(this);
        te.SetBounces(bounces);
        te.SetPower(shootForce);
        pos.Normalize();
        Debug.Log(pos);
        te.Fire(pos);
    }

    public void CanFire(bool _canFire)
    {
        canFire = _canFire;
    }

    private void UpdateBounce()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                bounces--;
                if (bounces < minBounces) bounces = maxBounces;
            }
            else
            {
                bounces++;
                if (bounces > maxBounces) bounces = minBounces;
            }
            //Debug.Log("Bounces - " + bounces);
        }
    }
    private void Shooting()
    {
        if (!canFire) return;
        if (Input.GetMouseButtonDown(0))
        {
            readying = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            readying = false;
            lr.positionCount = 0;
        }

        


        if (Input.GetMouseButton(0))
        {
            if (readying)
            {
                //Perform the Parabola creation or whatever
                lr.positionCount = lineSteps;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 charPos = transform.position;
                //for (int i = lineSteps; i > 0; i--)
                float dist = Vector2.Distance(mousePos, charPos);
                if (dist > maxDistForFullPower) shootForce = maxPower;
                else
                {
                    float multiplier = dist / maxDistForFullPower;
                    float maxMinusMin = maxPower - minPower;
                    shootForce = minPower + (maxMinusMin * multiplier);
                }
                for (int i = 0; i < lineSteps; i++)
                {
                    lr.SetPosition(i, PointPosition(i * 0.1f, mousePos - charPos));
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (readying)
            {
                lr.positionCount = 0;
                canFire = false;
                readying = false;
                Fire();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        UpdateBounce();
        Shooting();
        uiController.UpdateUI(bounces);

        if (Input.GetKeyDown(KeyCode.T)) transform.position = debugTeleportPos.transform.position;
    }

    Vector2 PointPosition(float t, Vector2 dir)
    {
        Vector2 currentPointPos = (Vector2)transform.position + (dir.normalized * shootForce * t) + 0.5f * Physics2D.gravity * (t * t);
        return currentPointPos;

    }
}
