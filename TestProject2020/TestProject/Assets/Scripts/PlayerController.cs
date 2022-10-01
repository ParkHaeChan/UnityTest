using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 대체로 무난하지만 벽붙기를 사용할 수 있음 (벽을 계속 밀면 아래로 안떨어지고 버틸 수 있음)

    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight;

    public Rigidbody2D rb;
    float inputDirection;

    Vector2 velocity;

    bool jump = false;
    bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
                jump = true;
        }
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        if (jump)
        {
            velocity.y += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
            jump = false;
        }
        velocity.x = inputDirection * speed;
        
        rb.velocity = velocity;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter: " + collision.name);
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("TriggerExit: " + other.name);
        isGrounded = false;
    }
}
