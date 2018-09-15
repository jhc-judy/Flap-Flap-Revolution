using UnityEngine;
using System.Collections;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class Bird : MonoBehaviour 
{
	public float upForce;					//Upward force of the "flap".
	private bool isDead = false;			//Has the player collided with a wall?

	private Animator anim;					//Reference to the Animator component.
	private Rigidbody2D rb2d;               //Holds a reference to the Rigidbody2D component of the bird.

    //[SerializeField]
    //private Text m_Recognitions;
    //private DictationRecognizer m_DictationRecognizer;

    //[SerializeField]
    //ConfidenceLevel m_MinimumConfidence = ConfidenceLevel.High;

    public static float MicLoudness;

    private string _device;
    private AudioClip _clipRecord;

    void Start()
	{
		//Get reference to the Animator component attached to this GameObject.
		anim = GetComponent<Animator> ();
		//Get and store a reference to the Rigidbody2D attached to this GameObject.
		rb2d = GetComponent<Rigidbody2D>();

        //m_DictationRecognizer = new DictationRecognizer();


        //m_DictationRecognizer.DictationResult += (text, confidence) =>
        //{
        //    Debug.Log(text);
        //    m_Recognitions.text += text + "\n";
        //};

        //m_DictationRecognizer.Start();
        InitMic();
    }

	void Update()
	{
		//Don't allow control if the bird has died.
		if (isDead == false) 
		{
			//Look for input to trigger a "flap".
			if (Input.GetMouseButtonDown(0) || Input.anyKeyDown || MicLoudness > 0.0001) //|| m_Recognitions.text != "" 
            {
				//...tell the animator about it and then...
				anim.SetTrigger("Flap");
				//...zero out the birds current y velocity before...
				rb2d.velocity = Vector2.zero;
				//	new Vector2(rb2d.velocity.x, 0);
				//..giving the bird some upward force.
				rb2d.AddForce(new Vector2(0, upForce));
                //StartCoroutine(WaitForSeconds());
            }
		}
        

        //if (m_DictationRecognizer.Status.Equals("Running"))
        //{
        //    m_DictationRecognizer.Stop();
        //    m_DictationRecognizer.Start();
        //}
        //Debug.Log(m_DictationRecognizer.Status.ToString());

        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();
        //Debug.Log(MicLoudness);
    }



    //mic initialization
    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = AudioClip.Create("Mic", 44100 * 2, 1, 44100, true);
        _clipRecord = Microphone.Start(_device, true, 999, 44100);

    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }
    int _sampleWindow = 128;

    //get data from microphone into audioclip
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        
        return levelMax;
    }

    bool _isInitialized;
    // start mic when scene starts
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //Debug.Log("Focus");

            if (!_isInitialized)
            {
                //Debug.Log("Init Mic");
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            StopMicrophone();
            //Debug.Log("Stop Mic");
            _isInitialized = false;

        }
    }

    private IEnumerator WaitForSeconds()
    {
        // process pre-yield
        yield return new WaitForSeconds(1.0f);
        // process post-yield

        //m_Recognitions.text = "";

    }

	void OnCollisionEnter2D(Collision2D other)
	{
		// Zero out the bird's velocity
		rb2d.velocity = Vector2.zero;
		// If the bird collides with something set it to dead...
		isDead = true;
		//...tell the Animator about it...
		anim.SetTrigger ("Die");
		//...and tell the game control about it.
		GameControl.instance.BirdDied ();
        //m_DictationRecognizer.Stop();
        StopMicrophone();
    }
}
