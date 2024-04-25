using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float GroundSpeed;
    public float JumpSpeed;


    [Range(0f, 1f)]
    public float GroundDecay;

    public Rigidbody2D body;
    public BoxCollider2D GroundCheck;
    public LayerMask groundMask;

    public bool grounded;

    float xInput;
    float yInput;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        GetInput();
        MoveWithInput();
    }

    void FixedUpdate() {
        CheckGround();
        ApplyFriction();
        Debug.Log(body.velocity.x);
    }

    void GetInput()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    void MoveWithInput()
    {
        if (Mathf.Abs(xInput) > 0)
        {
            body.velocity = new Vector2(xInput * GroundSpeed, body.velocity.y);
        }

        if (Mathf.Abs(yInput) > 0 && grounded)
        {
            body.velocity = new Vector2(body.velocity.x, yInput * JumpSpeed);
        }
    }

    void CheckGround()
    {
        grounded = Physics2D.OverlapAreaAll(GroundCheck.bounds.min, GroundCheck.bounds.max, groundMask).Length > 0;
    }

    void ApplyFriction()
    {
        if (grounded && xInput == 0 && yInput == 0)
        {
            body.velocity *= GroundDecay;
        }
    }
}
