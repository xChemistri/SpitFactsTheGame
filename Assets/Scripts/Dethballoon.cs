using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dethballoon : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    private Vector2 vel;
    private Rigidbody2D rig;

    private GameObject whoTouchedMe;
    private static bool areTheyAlive = true;

    [SerializeField] private AudioClip oof;
    [SerializeField] private AudioClip bong;

    private Scoring scorer;
    
    void Start()
    {
        if (rig == null)
            rig = gameObject.GetComponent<Rigidbody2D>();

        vel = new Vector2(speed, speed);

         rig.position = new Vector2
        (
            UnityEngine.Random.Range(-3,3),
            UnityEngine.Random.Range(-5,5)
        );

        scorer = GameObject.FindWithTag("Scorekeeper").GetComponent<Scoring>();
        areTheyAlive = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        rig.velocity = vel;

        //Reverse directions if exceeding camera boundaries.
        if (rig.position.x > 8.0)
            vel = new Vector2(-1*speed, vel.y);
        else if (rig.position.x < -8.0)
            vel = new Vector2(speed, vel.y);

        if (rig.position.y > 5.0)
            rig.position = new Vector2(rig.position.x, -5.0f);
        else if (rig.position.y < -5.0)
            rig.position = new Vector2(rig.position.x, 5.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        whoTouchedMe = collision.gameObject;

        //Destroy the man
        if (collision.gameObject.tag == "Player")
            YouGotOofed();
        else if (collision.gameObject.tag == "Laser")
            Destroy(collision.gameObject);
    }

    // specific methods
    private void YouGotOofed ()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Laser"))
            Destroy(obj);

        whoTouchedMe.GetComponent<Animator>().SetInteger("death",1);

        //Remove control and show death by removal of colliders.
        Destroy( whoTouchedMe.GetComponent<Movement>() );
        Destroy( whoTouchedMe.GetComponent<BoxCollider2D>());
        Destroy( whoTouchedMe, 0.3f);
        Invoke("Restart", 3.0f);

        if (areTheyAlive)
        {
            areTheyAlive = false;

            AudioSource.PlayClipAtPoint
            (
                oof,
                new Vector3(0f,0f,-10f),
                PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
            );

            scorer.increment(-150);
        }
    }

    private void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GotHit ()
    {
        AudioSource.PlayClipAtPoint
        (
            bong,
            new Vector3(0f,0f,-10f),
            PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
        );
        Destroy(gameObject);
    }

}
