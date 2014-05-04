using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {
    public GUIText instructionsText;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            instructionsText.enabled = false;
            GameEventManager.TriggerGameStart();
            enabled = false;
            Destroy(gameObject);
        }
	}
}
