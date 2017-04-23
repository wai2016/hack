using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour {
    public float m_TurnSpeed = 100;
    public float height = 2;
    public int m_PlayerNumber = 1;
    public float accel = 100;
    public float fiction = 10;

    private Rigidbody m_Rigidbody;
    private float m_Speed;
    private float maxspeed;
    private float initspeed;
    private bool jump;

    public void setSpeed(float s)
    {
        m_Speed = s;
    }

    public float getSpeed()
    {
        return m_Speed;
    }

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        maxspeed = 600f;
        initspeed = 0f;
        m_Speed = initspeed;
        jump = false;
    }

    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
    }


    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }

	// Update is called once per frame
    void Update()
    {
        m_Rigidbody.angularVelocity = Vector3.zero; // stop self rotation
    }
	
	void FixedUpdate () {
        var z = Input.GetAxis("Horizontal" + m_PlayerNumber);
        var x = Input.GetAxis("Vertical" + m_PlayerNumber);
        var y = Input.GetAxis("Jump" + m_PlayerNumber);

        Move(x);

        Rotate(z);

        Jump(y);
    }

    void Move(float x)
    {
        Vector3 movement = transform.forward * m_Speed * Time.deltaTime;
        movement.y = m_Rigidbody.velocity.y; // y should have no effect
        //Debug.Log(m_Speed);

        //m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        m_Rigidbody.velocity = movement;

        if (m_Speed <= maxspeed && x > 0)
            m_Speed += accel;

        m_Speed -= fiction;

        if (m_Speed > maxspeed)
            m_Speed = maxspeed;

        if (m_Speed < initspeed)
            m_Speed = initspeed;
    }

    void Rotate(float z)
    {
        float turn = m_TurnSpeed * z * Time.deltaTime;

        Quaternion turnRoataion = Quaternion.Euler(0f, turn, 0f);
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRoataion);
    }

    void Jump(float y)
    {
        // On Land
        if (m_Rigidbody.velocity.y == 0 && y > 0)
            jump = true;
        // Start jump.
        if (jump)
        {
            m_Rigidbody.AddForce(Vector3.up * height, ForceMode.Impulse);
            jump = false;
        }
    }
}
