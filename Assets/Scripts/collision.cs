using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision : MonoBehaviour {
    //public float m_ExplosionForce;
    //public float m_ExplosionRadius;
    public float power = 50;

    private Rigidbody rb;
    private move m;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        m = GetComponent<move>();
    }

    // Use this for initialization
    void Start () {

    }
	
    private void OnEnable()
    {
        rb.isKinematic = false;
    }

    private void OnDisable()
    {
        rb.isKinematic = true;
    }

	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        if (collision.transform.tag == "Player")
        {
            /*
            ContactPoint contact = collision.contacts[0];
            rb.AddExplosionForce(m_ExplosionForce, contact.point, m_ExplosionRadius);
            m.setSpeed(rb.velocity.magnitude);
            */
            m.setSpeed(0);

            ContactPoint contact = collision.contacts[0];
            Vector3 dir = rb.position - contact.point;
            dir = dir * power * rb.velocity.magnitude;
            dir.y = 3;

            rb.AddForce(dir, ForceMode.Impulse);
            Debug.Log(dir);
        }
        
    }
}
