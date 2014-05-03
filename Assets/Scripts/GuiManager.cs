using UnityEngine;
using System.Collections;

public class GuiManager : MonoBehaviour {

    bool test;

    public GUIText gameoverText, instructionsText;
	// Use this for initialization
	void Start ()
    {
        gameoverText.enabled = false;
        test = true;
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
	}
	
	// Update is called once per frame
	void Update () {
        if (test && Input.GetButtonDown("Jump"))
            GameEventManager.TriggerGameStart();
	}

    void GameStart()
    {
        gameoverText.enabled = false;
        instructionsText.enabled = false;
        enabled = false;
        test = false;
    }

    void GameOver()
    {
        gameoverText.enabled = true;
        instructionsText.enabled = true;
        enabled = true;
        test = true;
    }
}
