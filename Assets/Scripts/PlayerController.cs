using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;

public class PlayerController : MonoBehaviour
{
    public int credits = 0;
    public float speed = 5.0f;
    public float rotSpeed = 5.0f;
    public bool isLanded = false;
    public float fuel = 3000f;
    public bool inRefuelRange = false;

    Rigidbody rb;
    Keyboard keyboard;
    MissionHandler missionHandler;

    AudioSource thruster;
    AudioSource powerDown;
    AudioSource ambience;
    AudioSource fuelWarning;

    public Transform groundCheck;
    public LayerMask groundCheckLayer;

    Color baseTextColor;

    MeshRenderer cameraScreen;
    public Material cameraScreenOff;
    public Material cameraScreenOn;

    //Ship UI Variables
    GameObject displayText;
    TextMeshPro dockedMessage;
    TextMeshPro fuelCounter;
    TextMeshPro velCounter;
    TextMeshPro angVelCounter;
    TextMeshPro missionAlert;
    TextMeshPro pickupMessage;
    TextMeshPro dropoffMessage;
    TextMeshPro timer;
    TextMeshPro creditsCounter;
    TextMeshPro bestCreditsCounter;
    TextMeshPro commsMessage;

    // Start is called before the first frame update
    void Start()
    {
        displayText = GameObject.Find("DisplayText");
        dockedMessage = GameObject.Find("Docked").GetComponent<TextMeshPro>();
        fuelCounter = GameObject.Find("FuelCounter").GetComponent<TextMeshPro>();
        velCounter = GameObject.Find("Velocity").GetComponent<TextMeshPro>();
        angVelCounter = GameObject.Find("AngularVelocity").GetComponent<TextMeshPro>();
        missionAlert = GameObject.Find("MissionAlert").GetComponent<TextMeshPro>();
        pickupMessage = GameObject.Find("PickUpMessage").GetComponent<TextMeshPro>();
        dropoffMessage = GameObject.Find("DropOffMessage").GetComponent<TextMeshPro>();
        timer = GameObject.Find("Timer").GetComponent<TextMeshPro>();
        creditsCounter = GameObject.Find("Credits").GetComponent<TextMeshPro>();
        bestCreditsCounter = GameObject.Find("BestCredits").GetComponent<TextMeshPro>();
        commsMessage = GameObject.Find("CommsMessage").GetComponent<TextMeshPro>();

        baseTextColor = pickupMessage.color;

        fuelWarning = GameObject.Find("FuelWarning").GetComponent<AudioSource>();
        ambience = GameObject.Find("Ambience").GetComponent<AudioSource>();
        powerDown = GameObject.Find("PowerDown").GetComponent<AudioSource>();
        thruster = GameObject.Find("Thrusters").GetComponent<AudioSource>();
        cameraScreen = GameObject.Find("CameraScreen").GetComponent<MeshRenderer>();
        missionHandler = GameObject.Find("GameManager").GetComponent<MissionHandler>();

        rb = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;
    }

    void FixedUpdate()
    {
        if(fuel > 0 && !missionHandler.gameOver)
        {
            Movement();
            displayText.SetActive(true);
            cameraScreen.material = cameraScreenOn;
        }
        else if(fuel <= 0 && !missionHandler.gameOver)
        {
            displayText.SetActive(false);
            cameraScreen.material = cameraScreenOff;
            thruster.Stop();
            ambience.Stop();
            fuelWarning.Stop();
            if(!powerDown.isPlaying)
            {
                powerDown.Play();
            }
            missionHandler.gameOver = true;
        }
        GroundCheck(groundCheck);
    }

    private void Update()
    {
        UpdateUI();
        RequestMission();
        QuitGame();
        MissionChecker();

        missionHandler.currentScore = credits;
    }

    void Movement()
    {
        float zInput = keyboard.wKey.ReadValue() - keyboard.sKey.ReadValue();
        float xInput = keyboard.dKey.ReadValue() - keyboard.aKey.ReadValue();
        float yInput = keyboard.rKey.ReadValue() - keyboard.fKey.ReadValue();

        float zRoll = keyboard.qKey.ReadValue() - keyboard.eKey.ReadValue();
        float xRoll = keyboard.upArrowKey.ReadValue() - keyboard.downArrowKey.ReadValue();
        float yRoll = keyboard.rightArrowKey.ReadValue() - keyboard.leftArrowKey.ReadValue();

        Vector3 rotation = new Vector3(xRoll, yRoll, zRoll);
        Vector3 move = new Vector3(xInput, yInput, zInput);

        //CONTROL THRUSTER SOUND
        thruster.panStereo = move.x - rotation.y - rotation.z;
        if(!thruster.isPlaying)
        {
            if(move != Vector3.zero || rotation != Vector3.zero)
            {
                thruster.Play();
            }
        }
        else if(move == Vector3.zero && rotation == Vector3.zero)
        {
            thruster.Stop();
        }

        rb.AddRelativeForce(move * speed * Time.fixedDeltaTime, ForceMode.Force);
        rb.AddRelativeTorque(rotation * rotSpeed * Time.fixedDeltaTime, ForceMode.Force);

        UpdateFuel(move, rotation);
    }

    void GroundCheck(Transform _groundCheckTarget)
    {
        bool hit = Physics.Raycast(transform.position, _groundCheckTarget.position, Vector3.Distance(transform.position, _groundCheckTarget.position), groundCheckLayer);
        if (hit && rb.velocity.magnitude <= 0.01f && rb.angularVelocity.magnitude <= 0.01f)
        {
            isLanded = true;
        }
        else
        {
            isLanded = false;
        }
    }

    void RequestMission()
    {
        bool missionKey = keyboard.vKey.isPressed;
        if (missionKey)
        {
            missionHandler.GeneratePickUp();
        }
    }

    void QuitGame()
    {
        bool quitKey = keyboard.escapeKey.isPressed;
        if(quitKey)
        {
            Application.Quit();
        }
    }

    void MissionChecker()
    {
        float checkDist;
        if (missionHandler.currentPickup != null)
        {
            checkDist = Vector3.Distance(transform.position, missionHandler.currentPickup.transform.position);
            //Debug.Log(checkDist);
            if (checkDist < 1.0f && isLanded)
            {
                missionHandler.GenerateDropOff();
                Destroy(missionHandler.currentPickup.gameObject);
            }
        }
        else if (missionHandler.currentDropoff != null)
        {
            checkDist = Vector3.Distance(transform.position, missionHandler.currentDropoff.transform.position);
            //Debug.Log(checkDist);
            if (checkDist < 1.0f && isLanded)
            {
                missionHandler.onMission = false;
                Destroy(missionHandler.currentDropoff.gameObject);
                missionHandler.timerMins += 1;
                credits += 500;
            }
        }
    }

    void UpdateFuel(Vector3 _move, Vector3 _rotation)
    {
        if (_move != Vector3.zero || _rotation != Vector3.zero)
        {
            fuel -= Time.fixedDeltaTime * speed;
        }

        //REFUEL
        if(keyboard.cKey.isPressed && inRefuelRange && credits > 0)
        {
            fuel += 4;
            credits--;
        }
    }

    void UpdateUI()
    {
        //HANDLE CREDITS DISPLAY TEXT
        creditsCounter.text = "Credits: " + credits;
        bestCreditsCounter.text = "Best Credits: " + missionHandler.gameScore.score;

        //HANDLE TIMER DISPLAY TEXT
        if (missionHandler.timerSecs > 10)
        {
            timer.text = "Time Remaining: " + (int)missionHandler.timerMins + " : " + (int)missionHandler.timerSecs; 
        }
        else
        {
            timer.text = "Time Remaining: " + (int)missionHandler.timerMins + " : " + "0" + (int)missionHandler.timerSecs;
        }

        if(missionHandler.timerMins < 1)
        {
            timer.color = Color.red;
        }
        else
        {
            timer.color = baseTextColor;
        }

        if(missionHandler.timeOut)
        {
            timer.text = null;
        }


        //HANDLE FUEL DISPLAY TEXT
        int fuelInt = (int)fuel;
        fuelCounter.text = "Fuel: " + fuelInt.ToString();

        if(fuelInt < 200 && !missionHandler.gameOver)
        {
            fuelCounter.color = Color.red;
            if(!fuelWarning.isPlaying)
            {
                fuelWarning.Play();
            }
        }
        else if(fuelInt >= 200 || missionHandler.gameOver)
        {
            fuelCounter.color = baseTextColor;
            fuelWarning.Stop();
        }

        velCounter.text = (rb.velocity.magnitude * speed).ToString();
        angVelCounter.text = (rb.angularVelocity.magnitude * speed).ToString();

        //HANDLE MISSION DISPLAY TEXT
        if(missionHandler.currentPickup != null)
        {
            missionAlert.enabled = false;
            pickupMessage.text = "Distance to Pick-Up:" + "\n" + Vector3.Distance(transform.position, missionHandler.currentPickup.transform.position);
            dropoffMessage.text = "Land at Destination to Collect your Payload";
        }
        else if(missionHandler.currentDropoff != null)
        {
            missionAlert.enabled = false;
            pickupMessage.text = "Payload Collected";
            dropoffMessage.text = "Distance to Drop-Off:" + "\n" + Vector3.Distance(transform.position, missionHandler.currentDropoff.transform.position);
        }
        else
        {
            pickupMessage.text = null;
            dropoffMessage.text = null;
            if(!missionHandler.timeOut)
            {
                missionAlert.enabled = true;
            }
            else if(missionHandler.timeOut)
            {
                missionAlert.text = "NO DELIVERIES AVAILABLE \n press 'Esc' to Warp Home.";
                missionAlert.enabled = true;
            }
            missionAlert.enabled = true;
        }

        //HANDLE LANDING DISPLAY TEXT
        if (isLanded)
        {
            dockedMessage.enabled = true;
        }
        else
        {
            dockedMessage.enabled = false;
        }

        //HANDLE COMMS DISPLAY TEXT
        if(inRefuelRange)
        {
            commsMessage.text = " Fuel Station: \n 4 Fuel per Credit. \n Hold 'C' to Refuel. \n No Credits? No Fuel!";
        }
        else if(missionHandler.timeOut)
        {
            commsMessage.text = "Courier HQ: \nThat will do. \nFinish up and come home.";
        }
        else
        {
            commsMessage.text = "No New Messages!";
        }
    }
}
