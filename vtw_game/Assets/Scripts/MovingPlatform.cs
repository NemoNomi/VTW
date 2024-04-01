using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private Vector3 nextPosition;

    // Start is called before the first frame update
    void Start()
    {
        nextPosition = pointB.position;
    }

    // Update is called once per frame
void FixedUpdate()
{
    transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.fixedDeltaTime);
    if(transform.position == nextPosition)
    {
        nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
    }
}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }
        private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}