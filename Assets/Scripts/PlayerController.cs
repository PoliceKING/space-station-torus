using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float rotSpeed = 5.0f;

    Rigidbody rb;
    Keyboard keyboard;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        keyboard = Keyboard.current;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float zInput = keyboard.wKey.ReadValue() - keyboard.sKey.ReadValue();
        float xInput = keyboard.dKey.ReadValue() - keyboard.aKey.ReadValue();
        float yInput = keyboard.rKey.ReadValue() - keyboard.fKey.ReadValue();

        float zRoll = keyboard.qKey.ReadValue() - keyboard.eKey.ReadValue();
        float xRoll = keyboard.upArrowKey.ReadValue() - keyboard.downArrowKey.ReadValue();
        float yRoll = keyboard.rightArrowKey.ReadValue() - keyboard.leftArrowKey.ReadValue();

        Vector3 rotation = new Vector3(xRoll, yRoll, zRoll);
        Vector3 move = new Vector3(xInput, yInput, zInput);

        rb.AddRelativeForce(move * speed * Time.deltaTime, ForceMode.Force);
        rb.AddRelativeTorque(rotation * rotSpeed * Time.deltaTime, ForceMode.Force);
    }
}
