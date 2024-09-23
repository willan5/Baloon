using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public float floatForce;
    public AudioClip moneySound;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;
    public AudioClip explodeSound;
    public AudioClip baloonBounceSound;
    public float upperLimit = 19.0f;

    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    




    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }

        // set the upperlimit on the y to limit the floating distance
        if (transform.position.y > upperLimit)
        {
            transform.position = new Vector3(transform.position.x, upperLimit, transform.position.z);
            playerRb.velocity = Vector3.zero;
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            

            //stop the spinning and rotation if game over
           // playerRb.velocity = Vector3.zero;
           // playerRb.constraints = RigidbodyConstraints.FreezeRotation;
            //playerRb.angularVelocity = Vector3.zero;

            Debug.Log("Game Over!");
            Destroy(other.gameObject);



        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            //Keep playing the bouncesound only if game is playing
            if (!gameOver)
            {
                playerAudio.PlayOneShot(baloonBounceSound, 1.0f);
                playerRb.AddForce(Vector3.up * 2, ForceMode.Impulse);
            }

        }
       

    }

}
