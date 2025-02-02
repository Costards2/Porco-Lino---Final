using UnityEngine;

public class Bird : MonoBehaviour
{
    public float walkSpeed = 5f;
    public Transform pointRight;
    public Transform pointLeft;
    //public AudioClip deathClip;

    private float right;
    private float left;
    private SpriteRenderer sprite;
    public Collider2D colliderBird;
    public Collider2D trigger;
    private Rigidbody2D rb;

    private enum EnemyState
    {
        Idle,
        WalkingRight,
        WalkingLeft,
        Dead
    }

    private EnemyState currentState;

    void Start()
    {
        colliderBird = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        right = pointRight.position.x;
        left = pointLeft.position.x;

        if(right == left)
        {
            currentState = EnemyState.Idle;
        }
        else 
        {
            currentState = EnemyState.WalkingRight;
        }
        //sprite.flipX = true;
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.WalkingRight:
                WalkRight();
                break;
            case EnemyState.WalkingLeft:
                WalkLeft();
                break;
            case EnemyState.Dead:
                Dead();
                break;
        }
    }

    void WalkRight()
    {
        transform.Translate(Time.deltaTime * walkSpeed * Vector2.right);

        if (transform.position.x >= right)
        {
            currentState = EnemyState.WalkingLeft;
            sprite.flipX = true; // Flip the sprite to face left
        }
    }

    void WalkLeft()
    {
        transform.Translate(Time.deltaTime * walkSpeed * Vector2.left);

        if (transform.position.x <= left)
        {
            currentState = EnemyState.WalkingRight;
            sprite.flipX = false; // Flip the sprite to face right
        }
    }

    void Dead()
    {
        Animator animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            colliderBird.enabled = false;
            trigger.enabled = false;

            collision.GetComponent<Rigidbody2D>().linearVelocityY = 0;

            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(collision.GetComponent<Rigidbody2D>().linearVelocityX, 6f), ForceMode2D.Impulse);

            currentState = EnemyState.Dead;


            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.AddForce(new Vector3(0, 5, 0), ForceMode2D.Impulse);

            Destroy(gameObject, 3f);
        }
    }
}
