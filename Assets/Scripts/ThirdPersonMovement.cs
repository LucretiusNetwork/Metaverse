using GameWarriors.PoolDomain.Abstraction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour,IPoolable
{
    public CharacterController controller;
    private float speed = 6f;
    public float walkSpeed = 6f;
    public float runSpeed = 8f;
    public float jumpHight = 6f;

    public float turntSmoothSpeed = 0.1f;
    public float turntSmoothVelocity;
    public Transform cam;
    public Animator animator;
    //private Vector3 velocity;
    [SerializeField]
    private Rigidbody rigidbody;

    private bool isGrounded = false;
    private float groundCheckDistance;
    private LayerMask groundMask;
    private float gravity;

    public string PoolName => throw new NotImplementedException();

    public void CharacterUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        isGrounded = true;
        //if (isGrounded && rigidbody.velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turntSmoothVelocity, turntSmoothSpeed);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirectoin = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Debug.Log("moveDirectoin.normalized  " + moveDirectoin.normalized * speed * Time.deltaTime);
            //controller.Move(new Vector3(moveDirectoin.normalized.x, -1 *Time.deltaTime, moveDirectoin.normalized.z) * speed * Time.deltaTime);
            controller.Move(moveDirectoin.normalized * speed * Time.deltaTime);
            //rigidbody.velocity = new Vector3(0, -1 * Time.deltaTime, 0);
            //     transform.position = new Vector3(transform.position.x, 22.86f,transform.position.z);
        }

        //  rigidbody.velocity = new Vector3(transform.position.x, rigidbody.velocity.y, transform.position.z);

        if (isGrounded)
        {
            if (direction.magnitude >= 0.1f && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (direction.magnitude >= 0.1f && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (direction.magnitude < 0.1f)
            {
                Ideal();
            }

        }

        //velocity.y += gravity * Time.deltaTime;
        //   controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.E))
            Talking();
        if (Input.GetKeyDown(KeyCode.Q))
            Selecting();

    }
    private void Jump()
    {
        Debug.Log("Jumb");
        //velocity.y = Mathf.Sqrt(jumpHight * -2 * gravity);
        rigidbody.AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
        //animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        animator.SetTrigger("Jump");
    }
    private void Ideal()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);

    }

    private void Run()
    {
        speed = runSpeed;
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);

    }
    private void Walk()
    {
        speed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Talking()
    {
        animator.SetTrigger("Talk");
    }
    private void Selecting()
    {
        animator.SetTrigger("Select");
    }

    public void Initialize(IServiceProvider serviceProvider)
    { 
    }

    public void OnPopOut()
    {
        throw new NotImplementedException();
    }

    public void OnPushBack()
    {
        throw new NotImplementedException();
    }
}
