using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
    public float duration = 1.0f;
    public float massGain = 1.0f;
    private GameObject target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!renderer.enabled)
        {
            duration -= Time.deltaTime;
            if (duration <= 0)
            {
                ReverseEffect();
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (renderer.enabled && other.tag == "Player")
        {
            renderer.enabled = false;
            target = other.gameObject;
            ApplyEffect();
        }
    }

    void ApplyEffect()
    {
        target.rigidbody2D.mass += massGain;
    }

    void ReverseEffect()
    {
        target.rigidbody2D.mass -= massGain;
    }
}
