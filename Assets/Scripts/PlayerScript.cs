using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
	public Vector2	speed = new Vector2(50, 50);
    public LayerMask mask;
    public float deltaTimeJump;
    public float wallJumpForce;
	private Vector2 movement;
	private bool grounded = true;
    private int walled = 0;
    private float timeJump = 0;
    private Vector3 startposition;
    private Vector2 hitbox;
    private float jumpActivated = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        timeJump = Mathf.Infinity;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        enabled = false;
        CircleCollider2D    h = GetComponent<CircleCollider2D>();
        hitbox = new Vector2(h.radius * Mathf.Abs(transform.localScale.x), h.radius * Mathf.Abs(transform.localScale.y));
    }

	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis("Horizontal");

        if (inputX != 0)
        {
            if (grounded)
                rigidbody2D.AddForce(new Vector2(speed.x * inputX, 0.1f));
            else
                rigidbody2D.AddForce(new Vector2(0.15f * speed.x * inputX, 0.1f));
        }

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

	/*void OnCollisionEnter2D(Collision2D col) 
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
			grounded = true;
	}*/

	void FixedUpdate()
	{
		Vector2 v = rigidbody2D.velocity;
		rigidbody2D.velocity = new Vector2(Mathf.Clamp(v.x, -5, 5), v.y);

		if (v.x < -0.1f && transform.localScale.x < 0 || v.x > 0.1f && transform.localScale.x > 0)
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -hitbox.x, 0));
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
        enabled = false;
    }

    void MessageWallEnter(Collider2D other)
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

    void MessageGroundExit(Collider2D other)
    {
        grounded = false;
    }
}
