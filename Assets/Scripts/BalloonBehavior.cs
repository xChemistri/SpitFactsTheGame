using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BalloonBehavior : MonoBehaviour
{
    [SerializeField] int speed = 1;

    [SerializeField] int potential = 500;
    [SerializeField] private int potentialLoss;

    [SerializeField] Vector2 vel;
    [SerializeField] Rigidbody2D rig;

    [SerializeField] private AudioClip oof;
    [SerializeField] private AudioClip bong;

    [SerializeField] private GameObject player;
    
    private Scoring scorer;


    void Start()
    {
        potentialLoss = (potential/100);
        
        if (rig == null)
            rig = gameObject.GetComponent<Rigidbody2D>();
        
        vel = new Vector2(speed, speed);

        rig.position = new Vector2
        (
            UnityEngine.Random.Range(-8,8),
            UnityEngine.Random.Range(-5,5)
        );

        scorer = GameObject.FindWithTag("Scorekeeper").GetComponent<Scoring>();

        //Player no longer exists? Restablish the soviet union and make another one
        if (GameObject.FindWithTag("Player") == null)
            Instantiate(player);

        InvokeRepeating("Grow", 3, 0.025f);
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
            vel = new Vector2(vel.x,-1*speed);
        else if (rig.position.y < -4.5)
            vel = new Vector2(vel.x, speed);

        // If too big, oof.
         if (rig.transform.localScale.x >= 0.15)
            Oof();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy when colliding with a laser.
        if (collision.gameObject.tag == "Laser")
            GotHit();
    }

    // specific methods
    private void Oof ()
    {
        AudioSource.PlayClipAtPoint
        (
            oof,
            new Vector3(0f,0f,-10f),
            PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
        );
        
        gameObject.SetActive(false);
        Invoke("Redo",3);
    }

    private void GotHit ()
    {
        AudioSource.PlayClipAtPoint
        (
            bong,
            new Vector3(0f,0f,-10f),
            PlayerPrefs.GetFloat("GAME_VOLUME",0.5f)
        );
        
        scorer.increment(potential);
        
        gameObject.SetActive(false);
        Invoke("Next",3);
    }

    private void Next ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    private void Redo ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Grow ()
    {
        rig.transform.localScale = new Vector3
            (rig.transform.localScale.x+0.001f,
            rig.transform.localScale.x+0.001f,
            rig.transform.localScale.x+0.001f);

        potential -= potentialLoss;
    }
}
