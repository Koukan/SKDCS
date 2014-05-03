using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
	public Vector2	speed = new Vector2(50, 50);
	private Vector2 movement;
	private bool	grounded = false;
    private Vector3 startposition;
	
    void Start()
    {
        anim = GetComponent<Animator>();
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        enabled = false;
    }

	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis("Horizontal");

		if (inputX != 0)
			rigidbody2D.AddForce(new Vector2(speed.x * inputX, 0.1f));

        anim.SetFloat("Speed", Mathf.Abs(rigidbody2D.velocity.x));
		
		// 5 - Shooting
		if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
		{
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
			}
		}

		if (grounded && Input.GetButtonDown("Jump"))
		{
			rigidbody2D.AddForce(new Vector2(0, speed.y));
			grounded = false;
		}
        if (transform.position.y < -20)
            GameEventManager.TriggerGameOver();
	}

	void OnCollisionEnter2D(Collision2D col) 
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
			grounded = true;
	}

	void FixedUpdate()
	{
		Vector2 v = rigidbody2D.velocity;
		rigidbody2D.velocity = new Vector2(Mathf.Clamp(v.x, -5, 5), v.y);

		if (v.x < -0.1f && transform.localScale.x < 0 || v.x > 0.1f && transform.localScale.x > 0)
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
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
}
