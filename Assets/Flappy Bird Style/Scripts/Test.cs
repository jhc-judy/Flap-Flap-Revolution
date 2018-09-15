using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {
    public GameObject ball;
    private Text timer;
    private List<GameObject> list;
    public AudioSource audio;

    public const float min = 0f;                                 //Minimum y value of the column position.
    public const float max = 4f;

    // Use this for initialization
    DateTime start; 
	void Start () {
        start = DateTime.Now;
        StartCoroutine(waiter());
        timer = FindObjectOfType<Text>();
        audio.PlayDelayed(NewBehaviourScript.DELAY_TIME);
    }

    public double GetRandomNumber(double minimum, double maximum)
    {
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    IEnumerator waiter()
    {


        // Display the file contents to the console. Variable text is a string.

        // Example #2
        // Read each line of the file into a string array. Each element
        // of the array is one line of the file.
        string[] lines = System.IO.File.ReadAllLines(@"Assets/realshit.txt");

        // Display the file contents by using a foreach loop.
        foreach (string line in lines)
        {
            double diffInSeconds = (DateTime.Now - start).TotalSeconds - NewBehaviourScript.DELAY_TIME;
            yield return new WaitForSeconds(float.Parse(line) - (float)diffInSeconds - NewBehaviourScript.PERIOD);
            // Use a tab to indent each line of the file.
            float y = (float)GetRandomNumber(min, max);
            
            GameObject o = Instantiate(ball, new Vector3(10, y, 0), transform.rotation);
            o.AddComponent<BoxCollider2D>();
            //Debug.Log(y);
            o.SetActive(true);
            //Wait for 2 seconds
            
        }
        

        //Wait for 4 seconds
        //yield return new WaitForSeconds(1);

        //Rotate 40 deg
        //Instantiate(ball, new Vector3(0, 0, 0), transform.rotation);



        //Rotate 20 deg
        //Instantiate(ball, new Vector3(0, 0, 0), transform.rotation);
    }

    // Update is called once per frame
    void Update () {
        double diffInSeconds = (DateTime.Now - start).TotalSeconds - NewBehaviourScript.DELAY_TIME;

        //timer.text = "" + diffInSeconds;
    }
}
