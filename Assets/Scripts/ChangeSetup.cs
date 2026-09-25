using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeSetup : MonoBehaviour {
    public InputActionReference moveAction;
    public Rigidbody carBody;
    public Transform carVisual; 
    
    public float accel=22;
    public float torque = 100f;
    public float rollResistance = 2f;
    
    private Vector2 _inputVector;
    
    void Start()
    {
	    if (!moveAction)
	    {
		    Debug.LogWarning("No move action assigned!");
		    return;
	    }
	    if (!carBody || !carVisual)
	    {
		    Debug.LogWarning("No car assigned!");
		    return;
	    }
	    
	    moveAction.action.Enable();
    }
    private void FixedUpdate()
    {
	    _inputVector = moveAction.action.ReadValue<Vector2>();

	    if (Mathf.Abs(_inputVector.y) > 0.01f)
		    //forward
		    carBody.AddForce(carVisual.forward * (_inputVector.y * accel), ForceMode.Acceleration);

	    if (Mathf.Abs(_inputVector.x) > 0.01f)
		    // turny
		    carBody.AddTorque(Vector3.up * (_inputVector.x * torque));
	
	    var lateralVelocity = Vector3.Dot(carBody.linearVelocity, carVisual.right) * carVisual.right;
	    carBody.linearVelocity -= lateralVelocity;
		//resistance
	    var forwardVelocity = Vector3.Dot(carBody.linearVelocity, carVisual.forward) * carVisual.forward;
	    carBody.AddForce(-forwardVelocity * rollResistance, ForceMode.Acceleration);
    }
    private void OnDestroy()
    {
	    
	    moveAction.action.Disable();
    }
}
