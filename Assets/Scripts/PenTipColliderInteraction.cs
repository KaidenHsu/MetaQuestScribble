using UnityEngine;
using System.Collections;

public class PenTipColliderInteraction : MonoBehaviour
{
    public SerialSender serialSender; // Reference to the SerialSender script
    public string signalToSend = "PenTouching"; // The signal to send to Arduino
    public float messageInterval = 0.5f; // Interval between messages (in seconds)

    private bool isTouchingCanvas = false; // Tracks if the pen tip is touching the canvas
    private Coroutine sendMessageCoroutine; // Reference to the active coroutine

    private void OnCollisionEnter(Collision collision)
    {
        // Start sending messages if the pen tip touches the canvas
        if (collision.gameObject.CompareTag("canvas"))
        {
            isTouchingCanvas = true;

            // Start the coroutine to send messages
            if (sendMessageCoroutine == null)
            {
                sendMessageCoroutine = StartCoroutine(SendMessageAtIntervals());
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Stop sending messages if the pen tip stops touching the canvas
        if (collision.gameObject.CompareTag("canvas"))
        {
            isTouchingCanvas = false;

            // Stop the coroutine
            if (sendMessageCoroutine != null)
            {
                StopCoroutine(sendMessageCoroutine);
                sendMessageCoroutine = null;
            }
        }
    }

    private IEnumerator SendMessageAtIntervals()
    {
        while (isTouchingCanvas)
        {
            serialSender.SendMessage(signalToSend); // Send the message to Arduino
            yield return new WaitForSeconds(messageInterval); // Wait for the interval
        }
    }
}