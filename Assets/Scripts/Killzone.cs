using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour {
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameController.KillPlayer();
        }
        
    }
}
