using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour {
    public string nextSceneName;

    private GameController gameController;
    private bool triggered = false;

	private void Start () {
        gameController = GameObject.FindObjectOfType<GameController>();	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            triggered = true;
            gameController.NextLevelStart(nextSceneName);
        }
    }

}
