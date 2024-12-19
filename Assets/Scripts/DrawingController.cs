using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingController : MonoBehaviour
{
    public GameObject canvas; // Reference to the canvas (the cube)
    public Material blackMaterial; // Black material
    public Material blueMaterial;  // Blue material
    public Material redMaterial;   // Red material
    public float pencilWidth = 0.01f;
    public float penWidth = 0.02f;
    public float paintWidth = 0.05f;

    // Brush and color buttons
    public Button pencilButton;
    public Button penButton;
    public Button paintButton;
    public Button blackButton;
    public Button blueButton;
    public Button redButton;

    private LineRenderer currentLineRenderer;
    private List<Vector3> drawnPoints = new List<Vector3>();
    private Transform penTip;
    private string currentBrush = "Pencil"; // Default brush
    private Material currentColorMaterial; // Default color
    private Button currentBrushButton; // Currently selected brush button
    private Button currentColorButton; // Currently selected color button

    void Start()
    {
        penTip = transform; // Attach the script to the pen tip
        currentColorMaterial = blackMaterial; // Default color is black

        // Set default selected buttons
        currentBrushButton = pencilButton;
        currentColorButton = blackButton;

        // Highlight default buttons
        SetButtonSelected(currentBrushButton, true);
        SetButtonSelected(currentColorButton, true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == canvas)
        {
            StartNewLine();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == canvas && currentLineRenderer != null)
        {
            Vector3 newPoint = penTip.position;

            if (drawnPoints.Count == 0 || Vector3.Distance(drawnPoints[drawnPoints.Count - 1], newPoint) > 0.01f)
            {
                AddPointToLine(newPoint);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == canvas)
        {
            currentLineRenderer = null;
        }
    }

    void StartNewLine()
    {
        GameObject newLine = new GameObject("Line");
        newLine.transform.SetParent(canvas.transform);

        currentLineRenderer = newLine.AddComponent<LineRenderer>();
        ApplyBrushSettings();

        currentLineRenderer.positionCount = 0;
        drawnPoints.Clear();
    }

    void AddPointToLine(Vector3 point)
    {
        drawnPoints.Add(point);
        currentLineRenderer.positionCount = drawnPoints.Count;
        currentLineRenderer.SetPositions(drawnPoints.ToArray());
    }

    public void SetBrush(string brush)
    {
        currentBrush = brush;

        // Change button highlight
        if (currentBrushButton != null)
            SetButtonSelected(currentBrushButton, false);

        switch (brush)
        {
            case "Pencil":
                currentBrushButton = pencilButton;
                break;
            case "Pen":
                currentBrushButton = penButton;
                break;
            case "Paint":
                currentBrushButton = paintButton;
                break;
        }

        SetButtonSelected(currentBrushButton, true);
    }

    public void SetColor(string color)
    {
        switch (color)
        {
            case "Black":
                currentColorMaterial = blackMaterial;
                if (currentColorButton != null) SetButtonSelected(currentColorButton, false);
                currentColorButton = blackButton;
                break;
            case "Blue":
                currentColorMaterial = blueMaterial;
                if (currentColorButton != null) SetButtonSelected(currentColorButton, false);
                currentColorButton = blueButton;
                break;
            case "Red":
                currentColorMaterial = redMaterial;
                if (currentColorButton != null) SetButtonSelected(currentColorButton, false);
                currentColorButton = redButton;
                break;
        }

        SetButtonSelected(currentColorButton, true);
    }

    void ApplyBrushSettings()
    {
        switch (currentBrush)
        {
            case "Pencil":
                currentLineRenderer.material = currentColorMaterial;
                currentLineRenderer.startWidth = pencilWidth;
                currentLineRenderer.endWidth = pencilWidth;
                break;
            case "Pen":
                currentLineRenderer.material = currentColorMaterial;
                currentLineRenderer.startWidth = penWidth;
                currentLineRenderer.endWidth = penWidth;
                break;
            case "Paint":
                currentLineRenderer.material = currentColorMaterial;
                currentLineRenderer.startWidth = paintWidth;
                currentLineRenderer.endWidth = paintWidth;
                break;
        }
    }

    void SetButtonSelected(Button button, bool selected)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = selected ? Color.yellow : Color.white; // Yellow for selected, white for unselected
        colors.highlightedColor = selected ? Color.yellow : Color.white;
        button.colors = colors;
    }
}