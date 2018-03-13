using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollision : MonoBehaviour {
    public Transform puffPrefab;

    private GameController gameController;
    

    private void Start()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            gameController.soundController.PlayCoinSound();
            Instantiate(puffPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            gameController.GetCoin();
        }
    }
}
