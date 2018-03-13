using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour {
    private Rigidbody2D parentBody;
    private GameController gameController;
    public float killImpulseForce;
    public Transform enemyPuffPrefab;

    private void Start()
    {
        parentBody = GetComponentInParent<Rigidbody2D>();
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Instantiate(enemyPuffPrefab, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Debug.Log("Destroying " + collision.name);
            parentBody.AddForce(new Vector2(0.0f, killImpulseForce), ForceMode2D.Impulse);
            gameController.soundController.PlayKillSound();
        }
    }

}
