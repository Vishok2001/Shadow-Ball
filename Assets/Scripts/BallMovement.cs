using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody body;
    //private GameObject ball;
    public float speed = 30;
    public float jumpPower = 10;
    //public float sprintspeed = 1000f

    public Vector3 facing;
    public Vector3 perpendicular;
    public Vector3 originalPos = new Vector3(-13.5f, 3.2f, -5.7f);

    [SerializeField]
    private bool isJumping = false;

    private float xInput;
    private float zInput;

    void Start()
    {
        //ball = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody>();

        facing = transform.forward;
        perpendicular = GetPerpendicular(facing);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            print("jump");
            isJumping = true;
            body.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        Vector3 movement = body.velocity;
        movement += ((facing * zInput) + (perpendicular * xInput)) * speed;
        if (movement.magnitude > 0)
            movement = Vector3.ClampMagnitude(movement, speed);
        else
            movement = Vector3.ClampMagnitude(movement, -speed);

        //print(movement);
        /*   
         Vector3 movement = new Vector3(xInput, 0, zInput);
         movement*=speed;
         movement = Vector3.ClampMagnitude(movement,speed);

          */
        body.AddForce(movement);
        body.velocity = Vector3.ClampMagnitude(body.velocity, speed);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);

        }



        if (body.velocity.magnitude != 0)
        {
            facing.x = body.velocity.x;
            facing.z = body.velocity.z;
            perpendicular = GetPerpendicular(facing);
        }
        if ((body.velocity.x == 0 && body.velocity.z == 0))
        {
            facing = transform.forward;
            perpendicular = GetPerpendicular(facing);

        }
    }

    //void FixedUpdate_base()
    //{
    //    float xInput = Input.GetAxis("Horizontal");
    //    float zInput = Input.GetAxis("Vertical");

    //    Vector3 movement = new Vector3(xInput, 0, zInput);
    //    movement*=speed;
    //    movement = Vector3.ClampMagnitude(movement,speed);
    //    body.AddForce(movement);
    //}


    private Vector3 GetPerpendicular(Vector3 inVec)
    {
        return new Vector3(inVec.z, 0, -inVec.x);
    }

    public void OnCollisionEnter(Collision col)
    {
        Vector3 delta = Vector3.zero;
        List<ContactPoint> list = new List<ContactPoint>();
        col.GetContacts(list);
        // print("Landing: " + col.contactCount);
        for (int i = 0; i < col.contactCount; i++)
        {
            delta += transform.position - list[i].point;
            //print(transform.position + " - " + list[i].point + " " + delta);
        }
        delta /= col.contactCount;
        //Debug.Log("Landing: Done " + delta + " --- " + Mathf.Abs(delta.y));
        if (Mathf.Abs(delta.y) > 0.25)
            isJumping = false;
    }
}


