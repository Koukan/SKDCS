using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    private Animator anim;
	public Vector2	speed = new Vector2(50, 50);
	private Vector2 movement;
	private bool	grounded = false;
	
    void Start()
    {
        anim = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		if (inputX != 0 || inputY != 0)
		{
			// 4 - Movement per direction
			movement = new Vector2 (speed.x * inputX, speed.y * inputY);
			rigidbody2D.AddForce (movement);
            //anim.
		}
		
		// 5 - Shooting
		bool shoot = Input.GetButtonDown("Fire1");
		shoot |= Input.GetButtonDown("Fire2");
		// Careful: For Mac users, ctrl + arrow is a bad idea
		
		if (shoot)
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
			rigidbody2D.AddForce(new Vector2(0, 200));
			grounded = false;
		}


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

		if (v.x < 0 && transform.localScale.x > 0 || v.x > 0 && transform.localScale.x < 0)
			transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
	}
}
