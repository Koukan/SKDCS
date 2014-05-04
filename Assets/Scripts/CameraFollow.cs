using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.
    public float xPadding = 0f;
    public float yPadding = 0f;
    public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.
    public LayerMask triggerMask;
    public Vector2 RaycastDistance = new Vector2(15, 5);
    private Transform player;		// Reference to the player's transform.
    private PlayerScript playerScripts;
    private Vector2 lastVelocity;

    void Awake()
    {
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScripts = player.GetComponent<PlayerScript>();
    }

    float     GetVelocityX()
    {
        float   v = player.rigidbody2D.velocity.x;
        if (v < 0.1f && v > -0.1f)
            return lastVelocity.x;
        if (v > 0)
        {
            lastVelocity.x = 1;
            return 1;
        }
        lastVelocity.x = -1;
        return -1;
    }

    float GetVelocityY()
    {
        float v = player.rigidbody2D.velocity.y;
        if (v < 0.1f && v > -0.1f)
            return lastVelocity.y;
        if (v > 0)
        {
            lastVelocity.y = 1;
            return 1;
        }
        lastVelocity.y = -1;
        return -1;
    }

    float GetPlayerX()
    {
        return player.position.x + xPadding * GetVelocityX();
    }

    float GetPlayerY()
    {
        return player.position.y + yPadding;
    }

    bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - GetPlayerX()) > xMargin;
    }

    bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - GetPlayerY()) > yMargin;
    }


    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;
        bool checkX = true;
        bool checkY = true;

        RaycastHit2D hit;
        if (GetPlayerX() > targetX)
        {
            if (playerScripts.Direction != 1)
                checkX = false;
            else
            {
                hit = Physics2D.Raycast(transform.position, new Vector2(1, 0), RaycastDistance.x, triggerMask);
                if (hit && hit.collider)
                    checkX = false;
            }
        }
        else if (playerScripts.Direction != -1)
        {
            checkX = false;
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(-1, 0), RaycastDistance.x, triggerMask);
            if (hit && hit.collider)
                checkX = false;
        }

        if (GetPlayerY() > targetY)
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(0, 1), RaycastDistance.y, triggerMask);
            if (hit && hit.collider)
                checkY = false;
        }
        else
        {
            hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), RaycastDistance.y, triggerMask);
            if (hit && hit.collider)
                checkY = false;
        }

        // If the player has moved beyond the x margin...
        if (checkX && CheckXMargin())
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(transform.position.x, GetPlayerX(), xSmooth * Time.deltaTime);

        // If the player has moved beyond the y margin...
        if (checkY && CheckYMargin())
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(transform.position.y, GetPlayerY(), ySmooth * Time.deltaTime);

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
