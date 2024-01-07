using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private BoxCollider2D bc2d;
    [SerializeField] private Animator animator;
    [SerializeField] private float teleportCollisionScale = 1.8f;

    [SerializeField] private GameObject arm;
    [SerializeField] private Transform armRotPoint;
    [SerializeField] private Transform armEndPoint;

    [SerializeField] private float minPower;
    [SerializeField] private float maxPower;
    [SerializeField] private float maxDistForFullPower;

    [SerializeField] private int lineSteps;

    [SerializeField] private GameObject debugTeleportPos;

    [SerializeField] private GameObject throwingArm;

    private bool canFire = true;
    //[SerializeField] private bool shotCooldown = false;
    [SerializeField] private float cooldownTime = 0.0f;

    [SerializeField] private GameObject teleporterPrefab;

    [SerializeField] private UIController uiController;

    [SerializeField] private float shootForce = 10.0f;

    [Header("Animations")]
    [SerializeField] private AnimationClip idle;
    [SerializeField] private AnimationClip walk;
    [SerializeField] private AnimationClip throwing;
    private bool isWalking;
    private int direction = 1;
    private int bounces;

    private float boxColliderXOffset;

    private bool readying;
    // Start is called before the first frame update
    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        if (lr == null) lr = GetComponent<LineRenderer>();
        if (bc2d == null) bc2d = GetComponent<BoxCollider2D>();
        if (animator == null) animator = GetComponent<Animator>();

        arm.SetActive(false);

        if (debugTeleportPos == null) debugTeleportPos = GameObject.Find("DebugTeleporter");

        lr.positionCount = 0;
        boxColliderXOffset = bc2d.offset.x;
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

        bool moving = false;
        if (Input.GetKey(KeyCode.D))
        {
            //rb.AddForce(transform.right * moveSpeed, ForceMode2D.Force);
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            direction = 1;
            moving = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //rb.AddForce(-transform.right * moveSpeed, ForceMode2D.Force);
            transform.position += -transform.right * moveSpeed * Time.deltaTime;
            direction = -1;
            moving = true;
        }


        sr.flipX = (direction == 1) ? false : true;
        bc2d.offset = (direction == 1) ? new Vector2(boxColliderXOffset, bc2d.offset.y) : new Vector2(-boxColliderXOffset, bc2d.offset.y);
        animator.SetBool("moving", moving);
    }


    public void Teleport(Vector2 pos, Vector2 normal)
    {
        // Add the amount penertrated into the object (to offest and speed)
        Vector2 scale = bc2d.size * new Vector2(transform.localScale.x, transform.localScale.y);
        scale /= 1.8f;
        //Vector2 scale = new Vector2(sr.bounds.size.x / 2, sr.bounds.size.y / 2);
        Vector2 movementAmt = normal * scale;
        transform.position = pos + movementAmt;
    }

    private void Fire()
    {
        //Debug.Log("Fired!");
        GameObject teleporter = Instantiate(teleporterPrefab, transform.position, Quaternion.identity);
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - armEndPoint.position;
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
            arm.SetActive(true);
            readying = true;
            animator.SetBool("readying", readying);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            arm.SetActive(false);
            readying = false;
            animator.SetBool("readying", readying);
            lr.positionCount = 0;
        }

        


        if (Input.GetMouseButton(0))
        {
            if (readying)
            {
                UpdateArm();
                //Perform the Parabola creation or whatever
                lr.positionCount = lineSteps;
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 charPos = armEndPoint.position;
                //for (int i = lineSteps; i > 0; i--)
                float dist = Vector2.Distance(mousePos, charPos);
                if (dist > maxDistForFullPower) shootForce = maxPower;
                else
                {
                    float multiplier = dist / maxDistForFullPower;
                    float maxMinusMin = maxPower - minPower;
                    shootForce = minPower + (maxMinusMin * multiplier);
                }
                Vector2 addedExtra = armEndPoint.position - transform.position;
                for (int i = 0; i < lineSteps; i++)
                {
                    lr.SetPosition(i, addedExtra + PointPosition(i * 0.1f, mousePos - charPos));
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (readying)
            {
                arm.SetActive(false);
                lr.positionCount = 0;
                canFire = false;
                readying = false;
                animator.SetBool("readying", readying);
                Fire();
            }
        }
    }

    private void UpdateArm()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerDist = transform.position - mousePos;
        sr.flipX = (playerDist.x > 0.0f) ? true : false;
        mousePos.z = armRotPoint.position.z;
        Vector3 dist = mousePos - armRotPoint.position;
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        armRotPoint.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
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
