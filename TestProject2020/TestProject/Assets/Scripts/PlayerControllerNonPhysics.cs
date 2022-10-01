using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNonPhysics : MonoBehaviour
{
    // 리지드 바디는 사용하되 움직임 적용은 리지드 바디로 하지 않고 transform으로 움직임
    // 벽붙기는 방지할 수 있으나 쉬버링 현상 나타남 (Update와 물리처리간 시간차에 의한 것으로 보임)

    [SerializeField]
    float speed;
    [SerializeField]
    float jumpHeight;

    Rigidbody2D rigidbody;
    float inputDirection;

    bool jump = false;
    bool isGrounded = true;

    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector2.zero;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += Physics2D.gravity.y * Time.deltaTime;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        inputDirection = Input.GetAxis("Horizontal");
        velocity.x = inputDirection * speed;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                velocity.y += Mathf.Sqrt(2f * -Physics2D.gravity.y * jumpHeight);
            }
        }

        Vector2 moved = velocity * Time.deltaTime;

        // 물리 적용하지 않고 움직임
        transform.position = (rigidbody.position + moved);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerStay: " + collision.name);
        isGrounded = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("TriggerExit: " + other.name);
        isGrounded = false;
    }
}
