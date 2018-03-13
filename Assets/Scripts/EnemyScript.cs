using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    private bool facingRight = true;
    private Rigidbody2D rb2D;
    public float walkSpeed;
    private GameController gameController;
    public Transform destroyPrefab;
    public EdgeCollider2D frontCollider;
    public Transform frontDownCollider;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindObjectOfType<GameController>();        
    }

    private void Update()
    {
        if (gameController.activeState == GameController.States.running)
        {
            ContactFilter2D f = new ContactFilter2D();
            Collider2D[] array = new Collider2D[1];
            Physics2D.OverlapCollider(frontCollider, f, array);

            if (array[0] != null && (array[0].tag == "Level" || array[0].tag == "Enemy"))
            {
                Flip();
            }

            // Check if there is a hole in front of the enemy
            Collider2D coll2D = Physics2D.OverlapCircle(frontDownCollider.position, 0.1f);
            if (coll2D == null)
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameController.activeState == GameController.States.running)
        {
            int direction = facingRight ? -1 : 1;
            rb2D.velocity = new Vector2(direction * walkSpeed, rb2D.velocity.y);
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            gameController.PlayerHit(transform);
        }
    }

}
