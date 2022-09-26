using Fusion;
using Services.Data.Netowrk;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.Core.Player
{
    public class PlayerMover : NetworkBehaviour
    {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private NetworkCharacterControllerPrototype _characterController;
        private float _turntSmoothSpeed = 0.1f;
        private float _turntSmoothVelocity;
        private bool _isGrounded = false;

        public float Speed => 2;

        public void Initilization()
        {
            //_characterController = GetComponent<NetworkCharacterControllerPrototype>();
        }

        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                var direction = data.direction.normalized;

                _isGrounded = true;
                if (direction.magnitude >= 0.1f)
                {
                    //float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turntSmoothVelocity, _turntSmoothSpeed);
                    transform.rotation = Quaternion.Euler(0f, angle, 0f);
                    Vector3 moveDirectoin = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                    Debug.Log("moveDirectoin.normalized  " + moveDirectoin.normalized * Speed * Time.deltaTime);
                    _characterController.Move(moveDirectoin.normalized * Speed * Time.deltaTime);
                }
                //_characterController.Move(5 * data.direction * Runner.DeltaTime);

                if (_isGrounded)
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
            }
        }

        private void Jump()
        {
            Debug.Log("Jumb");
            //velocity.y = Mathf.Sqrt(jumpHight * -2 * gravity);
            //GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHight, ForceMode.Impulse);
            //animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
            animator.SetTrigger("Jump");
        }
        private void Ideal()
        {
            animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);

        }

        private void Run()
        {
            //speed = runSpeed;
            animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);

        }

        private void Walk()
        {
            //speed = walkSpeed;
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
    }
}