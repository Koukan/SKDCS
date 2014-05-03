using UnityEngine;
using System.Collections;

public class DirectionTrigger : MonoBehaviour {

    public int Direction;

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            collider.SendMessage("DirectionTrigger", Direction, SendMessageOptions.DontRequireReceiver);
    }
}
