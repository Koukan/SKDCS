using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
	public Vector2 speed = new Vector2(50, 50);
    public Vector2 speedMax = new Vector2(5, 5);
    public LayerMask mask;
    public float deltaTimeJump;
    public float wallJumpForce;
    public float accelerationCoef = 0.01f;
    public float jumpLooseWeightCoef = 2f;
    public float minMass = 5;
    public int Direction = 1;

	private Vector2 movement;
	private bool grounded = true;
    private int walled = 0;
    private float timeJump = 0;
    private Vector3 startposition;
    private float jumpActivated = 0;
    private float speedx;

    private int nbFrame = 0;
    private Vector2 allValueOfSpeed = new Vector2(0, 0);

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        timeJump = Mathf.Infinity;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        enabled = false;
        speedx = speed.x;
    }

	// Update is called once per frame
	void Update () {
        // Animation Variable
        anim.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));

        if (!grounded)
            timeJump += Time.deltaTime;

		if (Input.GetButtonDown("Jump") || jumpActivated > 0)
		{
            if (grounded && timeJump > deltaTimeJump)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
                rigidbody2D.AddForce(new Vector2(0, speed.y));
                timeJump = 0;
                jumpActivated = 0;
            }
            else if (walled != 0 && wallJumpForce > 0)
            {
                rigidbody2D.velocity = new Vector2(0, 0);
                rigidbody2D.AddForce(new Vector2(wallJumpForce * -walled, speed.y));
                speedx = -speedx;
                jumpActivated = 0;
            }
            else if (jumpActivated > 0.1)
                jumpActivated = 0;
            else if (jumpActivated > 0)
                jumpActivated += Time.deltaTime;
            else
                jumpActivated = Time.deltaTime;
		}

        if (transform.position.y < -20)
            GameEventManager.TriggerGameOver();
	}

	void FixedUpdate()
	{
         if (grounded)
            rigidbody2D.AddForce(new Vector2(speedx, 0.1f));
        else
            rigidbody2D.AddForce(new Vector2(0.1f * speedx, 0.1f));

		Vector2 v = rigidbody2D.velocity;
        rigidbody2D.velocity = new Vector2(Mathf.Clamp(v.x, -speedMax.x, speedMax.x), v.y);

		if (v.x < -0.1f && transform.localScale.x < 0 || v.x > 0.1f && transform.localScale.x > 0)
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);

        allValueOfSpeed.x += rigidbody2D.velocity.x;
        if (rigidbody2D.velocity.y > 0)
            allValueOfSpeed.y += rigidbody2D.velocity.y;
        if (nbFrame > 30)
        {

            float coef = ((allValueOfSpeed.x / nbFrame / speedMax.x) + (allValueOfSpeed.y / nbFrame / speedMax.y) * jumpLooseWeightCoef * 10f) * accelerationCoef;
            LooseWeight(coef);
            nbFrame = 0;
            allValueOfSpeed = new Vector2(0, 0);
       }
        ++nbFrame;
	}

    void LooseWeight(float coef)
    {
        if (rigidbody2D.mass > minMass)
        {
            speedMax.x += 0.2f * coef;
            rigidbody2D.mass -= 0.2f * coef;
        }
    }

    void GainWeight(float coef)
    {
        speedMax.x -= 0.5f * coef;
        rigidbody2D.mass += 0.2f * coef;
    }

    void GameStart()
    {
        startposition = transform.position;
        enabled = true;
        rigidbody2D.isKinematic = false;
    }

    void GameOver()
    {
        transform.position = startposition;
        rigidbody2D.isKinematic = true;
        animation.Play("Idle");
        enabled = false;
    }

    void MessageWallEnter(Collider2D other)
    {
        walled = other.gameObject.transform.position.x > transform.position.x ? 1 : -1;
    }

    void MessageWallStay(Collider2D other)
    {
        walled = other.gameObject.transform.position.x > transform.position.x ? 1 : -1;
    }

    void MessageWallExit(Collider2D other)
    {
        walled = 0;
    }

    void MessageGroundEnter(Collider2D other)
    {
        grounded = true;
    }

    void MessageGroundStay(Collider2D other)
    {
        grounded = true;
    }

    void MessageGroundExit(Collider2D other)
    {
        grounded = false;
    }

    void DirectionTrigger(int direction)
    {
        speedx = direction * Mathf.Abs(speedx);
        Direction = direction;
    }
}
