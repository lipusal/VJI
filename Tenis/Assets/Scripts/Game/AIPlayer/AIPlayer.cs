using System.Collections;
using System.Collections.Generic;
using FrameLord;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    public Transform ballPosition;

    private float speed = 15;

    private float hitForce = 28;

    public Transform aimTarget;

    private CharacterController _characterController;
    
    /* player id according to court side,
    * 1 if player is on team one or
    * 2 if player is on team two
   */
    private int _id;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        if (transform.position.x < 0)
        {
            _id = 1;
        }
        else
        {
            _id = 2;
        }
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
        AudioManager.Instance.PlaySound(transform.position, (int) SoundId.SOUND_STEPS);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Vector3 aimDirection = (aimTarget.position - transform.position).normalized;
            AudioManager.Instance.PlaySound(other.transform.position, (int) SoundId.SOUND_HIT);

            other.GetComponent<Rigidbody>().velocity = aimDirection * hitForce + new Vector3(0, 7f, 0);
            BallLogic.Instance.SetHittingPlayer(_id);

        }
    }
}
