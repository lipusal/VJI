using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public Transform ballPosition;

    private float speed = 15;

    private float hitForce = 28;

    public Transform aimTarget;

    private CharacterController _characterController;
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToBall();
    }

    private void MoveToBall()
    {
        Vector3 movingDirection = new Vector3(transform.position.x,transform.position.y, ballPosition.position.z) - transform.position;
        _characterController.Move(movingDirection * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            
            other.GetComponent<Rigidbody>().velocity = aimDirection * hitForce + new Vector3(0, 6.2f, 0);
        }
    }
}
