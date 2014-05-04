using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {
    public GUIText instructionsText;
    public GUIText gameoverText;
    private static bool _gameover = false;
	// Use this for initialization
	void Start ()
    {
        gameoverText.enabled = _gameover;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            gameoverText.enabled = false;
            instructionsText.enabled = false;
            GameEventManager.TriggerGameStart();
            enabled = false;
            _gameover = true;
            Destroy(gameObject);
        }
	}

    public void GameOver() {
        gameoverText.enabled = true;
    }

 }
