using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
	public Vector2	speed = new Vector2(50, 50);
    public LayerMask mask;
    public float deltaTimeJump;
	private Vector2 movement;
	private bool grounded = false;
    private int walled = 0;
    private float timeJump = 0;
    private Vector3 startposition;
    private Vector2 hitbox;

    void Start()
    {
        anim = GetComponent<Animator>();
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
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

        anim.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
		
		// 5 - Shooting
		/*if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
			}
		}*/

        if (!grounded)
            timeJump += Time.deltaTime;

		if (Input.GetButtonDown("Jump"))
		{
            if (grounded && timeJump > deltaTimeJump)
            {
                rigidbody2D.AddForce(new Vector2(0, speed.y));
                timeJump = 0;
            }
            else if (walled != 0)
            {
                rigidbody2D.velocity = new Vector2(0, 0);
                rigidbody2D.AddForce(new Vector2(2000 * -walled, speed.y));
                /*RaycastHit2D hit;
                if ((hit = Physics2D.Raycast(transform.position, new Vector2(1, 0.5f).normalized, hitbox.x + 0.2f, mask)) && hit.collider)
                {
                    rigidbody2D.velocity = new Vector2(0, 0);
                    rigidbody2D.AddForce(new Vector2(-2000, speed.y));
                }
                else if ((hit = Physics2D.Raycast(transform.position, new Vector2(-1, -0.5f).normalized, hitbox.x + 0.2f, mask)) && hit.collider)
                {
                    rigidbody2D.velocity = new Vector2(0, 0);
                    rigidbody2D.AddForce(new Vector2(2000, speed.y));
                }*/
            }
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
