using System.Collections;using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {
    public const float DELAY_TIME = 3;
    public const float PERIOD = 3;
    public AudioClip error;
    private bool dead;

    // Use this for initialization
    void Start()
    {
        dead = false;
    }
	
	// Update is called once per frame
	void Update () {
        // Move the object forward along its z axis 1 unit/second.
        //transform.Translate(Vector3.forward * Time.deltaTime);
        // Move the object upward in world space 1 unit/second.
        transform.Translate(Vector3.left * (17.5f/PERIOD) * Time.deltaTime, Space.World);
        if (transform.position.x <= -7.45 && transform.position.x >= -7.55)
        {
            Debug.Log("in hit box at " + FindObjectOfType<Text>().text);
            //Do stuff
        }
        if(transform.position.x < -9 && !dead)
        {
            FindObjectOfType<Bird>().rb2d.AddForce(new Vector2(0, -800));
            Debug.Log("Miss");
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = error;
            audio.Play();
            dead = true;
        }

        if (transform.position.x <= -10)
        {
            Destroy(gameObject);
        }

    }
    public string randomWords()
    {
        
        ArrayList randomWords = new ArrayList();
        randomWords.Add("a");
        randomWords.Add("b");
        randomWords.Add("c");
        Random random = new Random();
        return (string)randomWords[random.Next(randomWords.Count)];
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
    
}
