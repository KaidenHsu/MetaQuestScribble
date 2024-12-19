using UnityEngine;
using Oculus.Interaction.Input; // Ensure Meta XR SDK namespace is included
using System.Collections;

public class PinkyToCanvasInteraction : MonoBehaviour
{
    public Hand hand; // Reference to the hand object (from Meta XR SDK)
    public HandJointId pinkyIntermediateJoint = HandJointId.HandPinky2; // Pinky intermediate joint
    public Transform canvas; // Canvas (whiteboard) object
    public Vector3 canvasMinBounds; // Local minimum bounds of the canvas
    public Vector3 canvasMaxBounds; // Local maximum bounds of the canvas
    public SerialSender serialSender; // Reference to the SerialSender script
    public string canvasTag = "canvas"; // Tag to check for the canvas
    public float messageInterval = 0.5f; // Interval for sending signals

    private bool isPinkyTouchingCanvas = false; // Tracks whether pinky is touching the canvas
    private Coroutine sendMessageCoroutine; // Reference to the active coroutine

    void Update()
    {
        // Attempt to get the pinky intermediate joint's pose
        Pose jointPose;
        if (hand.GetJointPose(pinkyIntermediateJoint, out jointPose))
        {
            // Get the pinky intermediate joint's global position
            Vector3 pinkyGlobalPosition = jointPose.position;

            // Convert canvas bounds to world space
            Vector3 canvasMinGlobal = canvas.TransformPoint(canvasMinBounds);
            Vector3 canvasMaxGlobal = canvas.TransformPoint(canvasMaxBounds);

            // Check if the pinky is within the canvas bounds
            if (pinkyGlobalPosition.z <= 0 &&
                pinkyGlobalPosition.x >= canvasMinGlobal.x && pinkyGlobalPosition.x <= canvasMaxGlobal.x && // x is within range
                pinkyGlobalPosition.y >= canvasMinGlobal.y && pinkyGlobalPosition.y <= canvasMaxGlobal.y)   // y is within range
            {
                if (!isPinkyTouchingCanvas)
                {
                    isPinkyTouchingCanvas = true;
                    Debug.Log("Pinky started touching the canvas!");
                    sendMessageCoroutine = StartCoroutine(SendMessageAtIntervals("PinkyTouching"));
                }
            }
            else
            {
                if (isPinkyTouchingCanvas)
                {
                    isPinkyTouchingCanvas = false;
                    Debug.Log("Pinky stopped touching the canvas!");
                    StopSendingMessages();
                }
            }
        }
        else
        {
            Debug.Log("Failed to retrieve pinky joint position.");
        }
    }

    private IEnumerator SendMessageAtIntervals(string message)
    {
        while (isPinkyTouchingCanvas)
        {
            serialSender.SendMessage(message); // Send the signal to Arduino
            yield return new WaitForSeconds(messageInterval); // Wait for the specified interval
        }
    }

    private void StopSendingMessages()
    {
        if (sendMessageCoroutine != null)
        {
            StopCoroutine(sendMessageCoroutine);
            sendMessageCoroutine = null;
        }
    }
}