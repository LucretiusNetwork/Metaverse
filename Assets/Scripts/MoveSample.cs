using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSample : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform target;
    private float moveSpeed = 5;
    private float rotationSpeed = 2;
    private void Start()
    {
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }

    private void Update()
    {
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);

        //transform.position = Vector3.Lerp(start.position, end.position, Time.time);
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 0.02f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationSpeed * Time.deltaTime);
        //transform.position += transform.forward * Time.deltaTime * moveSpeed;
        //transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);


    }
}
