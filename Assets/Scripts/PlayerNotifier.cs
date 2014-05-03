using UnityEngine;
using System.Collections;

public class PlayerNotifier : MonoBehaviour
{
    public GameObject player;
    public string message;

    void OnTriggerEnter2D(Collider2D other)
    {
        player.SendMessage("Message" + message + "Enter", other, SendMessageOptions.DontRequireReceiver);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        player.SendMessage("Message" + message + "Exit", other, SendMessageOptions.DontRequireReceiver);
    }
}
