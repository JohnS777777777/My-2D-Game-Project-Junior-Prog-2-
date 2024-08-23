using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 8.0f;
    //public float zBound = 6;
    public float yBound = 4;
    public float xBound = 7;
    private Rigidbody playerRb;

    public float shootingDelay = 0.2f;
    public bool shootingDelayed;

    public GameObject projectile;
    public Transform playerShip;
    public AudioSource gunAudio;

    public ScreenBounds screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        //MovePlayer();
        ConstrainPlayerPosition();

        //get input and move
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();
        Vector3 velocity = speed * input;
        Vector3 tempPosition = transform.localPosition + velocity * Time.deltaTime;
        if (screenBounds.AmIOutOfBounds(tempPosition))
        {
            Vector2 newPosition = screenBounds.CalculateWrappedPosition(tempPosition);
            transform.position = newPosition;
        }
        else
        {
            transform.position = tempPosition;
        }


        //shooting
        if (Input.GetKey(KeyCode.Space))
        {
            if (shootingDelayed == false)
            {
                shootingDelayed = true;
                gunAudio.Play();
                GameObject p = Instantiate(projectile, transform.position, Quaternion.identity);
                StartCoroutine(DelayShooting());
            }
        }
    }

    /*
void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        playerRb.AddForce(Vector3.forward * speed * verticalInput);
        playerRb.AddForce(Vector3.right * speed * horizontalInput);
    }
    */

    void ConstrainPlayerPosition()
    {

        // changed position z to x, testing
        if (transform.position.x < -xBound)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
        }
        
        if (transform.position.x > xBound)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
        }

        if (transform.position.y < -yBound)
        {
            transform.position = new Vector3(transform.position.x, -yBound, transform.position.z);
        }

        if (transform.position.y > yBound)
        {
            transform.position = new Vector3(transform.position.x, yBound, transform.position.z);
        }
    }

    private IEnumerator DelayShooting()
    {
        yield return new WaitForSeconds(shootingDelay);
        shootingDelayed = false;
    }
}
