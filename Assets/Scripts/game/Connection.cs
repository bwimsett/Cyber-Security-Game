using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using JetBrains.Annotations;
using UnityEngine;


public class Connection : MonoBehaviour {

    public Node start;
    public Node end;
    public float defaultLineThickness;
    public float maxLineThickness;
    public float colliderThickness;
    public LineRenderer lineRenderer;
    public ConnectionCollider connectionCollider;
    public Sprite duplexChevron;
    public Sprite simplexChevron;

    private bool simulatingThreat;
    private float threatSimulationPercentComplete;
    private Node threatSimulationStartNode;
    private const float THREAT_SIMULATION_SPEED = 1.4f; // Percent of connection covered per second
    public bool threatSimulationComplete;

    private bool disabled = false;
    
    // True if data flows in both directions for this connection
    private bool duplex;
    
    public SpriteRenderer connectionChevron;
    
    void Update() {
        UpdateThreatSimulation();
    }
    
    void Start(){
        RefreshChevron();
    }
    
    public void Refresh() {
        RefreshPosition();
        RefreshGradient();
        RefreshChevron();
    }
    
    // Refreshes the positions of the two ends of the line
    public void RefreshPosition() {
        lineRenderer.positionCount = 2;
        
        if (lineRenderer == null) {
            Debug.Log("Null line renderer: "+this);
            return;
        }
        
        Vector3 startPos = start.nodeObject.transform.position;
        if (end != null) {
            Vector3 endPos = end.nodeObject.transform.position;
            lineRenderer.SetPosition(1, endPos);
        }

        lineRenderer.SetPosition(0, startPos);
        
        lineRenderer.startWidth = defaultLineThickness;
        connectionCollider.Refresh();
        
        RefreshChevron();
    }
    
    // Refreshes the gradient of the line
    public void RefreshGradient() {

        if (threatSimulationComplete) {
            return;
        }

        Color startColor = start.nodeObject.GetCurrentColor();
        
        GradientColorKey startColorKey = new GradientColorKey(startColor, 0);
        GradientColorKey endColorKey = new GradientColorKey();
        
        GradientAlphaKey startAlphaKey = new GradientAlphaKey(1,0);
        GradientAlphaKey endAlphaKey = new GradientAlphaKey(1,1);  
        
        if (end != null) {
            Color endColor = end.nodeObject.GetCurrentColor();
            endColorKey = new GradientColorKey(endColor, 1);
        }

        GradientColorKey[] gradientKey = new[] {startColorKey, endColorKey};
        GradientAlphaKey[] alphaKey = new[] {startAlphaKey, endAlphaKey}; 
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(gradientKey, alphaKey);

        lineRenderer.colorGradient = gradient;
    }

    public void RefreshChevron() {

        Vector2 pos1 = lineRenderer.GetPosition(0);
        Vector2 pos2 = lineRenderer.GetPosition(1);

        Vector2 normalised = (pos1 - pos2).normalized;
        
        float angle = Vector3.Angle(normalised, Vector3.right);

        if (normalised.y < 0) {
            angle = 360 - angle;
        }

        connectionChevron.transform.rotation = Quaternion.Euler(0,0,angle+90);

        connectionChevron.color = GetColorAtPoint(0.5f);

        Vector3 pos = Vector2.Lerp(pos1, pos2, 0.5f);
        connectionChevron.transform.localPosition = pos-GameManager.levelScene.transform.position;

        if (duplex) {
            connectionChevron.sprite = duplexChevron;
        }
        else {
            connectionChevron.sprite = simplexChevron;
        }

    }

    // Sets the ends of the connection, and refreshes the line.
    public void SetEnds(Node start, Node end, bool duplex) {  
        if (start == end) {
            return;
        }
        
        this.start = start;
        this.end = end;
        this.duplex = duplex;
        
        RefreshPosition();
        RefreshGradient();
        RefreshChevron();
    }
    
    // Compare against another connection for equality
    public override bool Equals(object other) {
        if (other.GetType() != typeof(Connection)) {
            return false;
        }

        Connection connection = (Connection) other;

        bool matchLR = connection.start == start && connection.end == end;
        bool matchRL = connection.start == end && connection.end == start;

        if (matchLR ^ matchRL) {
            return true;
        }

        return false;
    }

    // Output a string description of the connection
    public override string ToString() {
        if (end != null) {
            return "(" + start.nodeObject.GetNodeDefinition().nodeName + " -> " + end.nodeObject.GetNodeDefinition().nodeName + ")";
        } 
        
        return "(" + start.nodeObject.GetNodeDefinition().nodeName + " -> null )";
    }

    public Color GetColorAtPoint(float point) {
        return lineRenderer.colorGradient.Evaluate(point);
    }
    
    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Connection: "+this+" clicked.");
        }
    }

    public void SetVisible(bool visible) {
        if (visible) {
            lineRenderer.enabled = true;
            connectionChevron.enabled = true;
            return;
        }

        lineRenderer.enabled = false;
        connectionChevron.enabled = false;
    }

    public void ToggleDuplex() {
        duplex = !duplex;
        Debug.Log(duplex);
        RefreshChevron();
    }

    public void SetDuplex(bool value) {
        duplex = value;
        RefreshChevron();
    }

    public bool IsDuplex() {
        return duplex;
    }

    public void Disable() {
        lineRenderer.enabled = false;
        connectionChevron.enabled = false;
        disabled = true;
    }

    public void Enable() {
        lineRenderer.enabled = true;
        connectionChevron.enabled = true;
        disabled = false;
    }

    public bool isDisabled() {
        return disabled;
    }
    
    // Check whether data can flow from the start to the end along this connection.
    public bool FlowDirectionValid(Node start, Node end) {
        bool forward = (this.start == start && this.end == end);
        bool backward = (this.start == end && this.end == start);

        bool connectionMatches = forward ^ backward;

        if (!connectionMatches) {
            return false;
        }

        if (duplex && backward) {
            return true;
        }

        if (forward) {
            return true;
        }

        return false;
    }

    public void TriggerThreatSimulation(Node startNode) {
        if (startNode != start && startNode != end) {
            return;
        }

        if (simulatingThreat) {
            return;
        }

        simulatingThreat = true;
        threatSimulationStartNode = startNode;
        threatSimulationPercentComplete = 0;
        threatSimulationComplete = false;
    }

    public void ResetThreatSimulation() {
        simulatingThreat = false;
        threatSimulationComplete = false;
    }
    
    private void UpdateThreatSimulation() {
        if (!simulatingThreat) {
            return;
        }
        
        // Get the start and end points of the line
        Vector2 newPos = Vector2.zero;
        Vector2 lineStart = lineRenderer.GetPosition(0);
        Vector2 lineEnd = lineRenderer.GetPosition(1);

        if (lineRenderer.positionCount == 3) {
            lineStart = lineRenderer.GetPosition(0);
            lineEnd = lineRenderer.GetPosition(2);
        }
        
        lineRenderer.positionCount = 3;
             
        // Recalculate percentage complete (multiplied by Time.deltaTime to account for framerate differences)
        threatSimulationPercentComplete += THREAT_SIMULATION_SPEED * Time.deltaTime;

        if (threatSimulationPercentComplete > 1) {
            threatSimulationPercentComplete = 1;
        }
        
        float threatSimulationTime = threatSimulationPercentComplete;
            
        // Calculate position of new line vertex at percentage
        if (threatSimulationStartNode == start) {
            newPos = Vector2.Lerp(lineStart, lineEnd, threatSimulationPercentComplete);
        }
        else {
            newPos = Vector2.Lerp(lineEnd, lineStart, threatSimulationPercentComplete);
            threatSimulationTime = 1 - threatSimulationPercentComplete;
        }
        
        // Create line vertex at the calculated position
        Vector3[] positions = new Vector3[3];
            positions[0] = lineStart;
            positions[1] = newPos;
            positions[2] = lineEnd;
            lineRenderer.SetPositions(positions);
        
        // Set the colour of the gradient at the calculated position
        GradientColorKey[] colourKey = new GradientColorKey[3];
            colourKey[0] = new GradientColorKey(start.nodeObject.GetCurrentColor(), 0);
            colourKey[1] = new GradientColorKey(GameManager.levelScene.threatManager.threatColor, threatSimulationTime+0.001f);
            colourKey[2] = new GradientColorKey(end.nodeObject.GetCurrentColor(), 1);
            GradientAlphaKey[] alphaKey = new GradientAlphaKey[1];
            alphaKey[0] = new GradientAlphaKey(1,0);
        
            Gradient gradient = new Gradient();
            gradient.mode = GradientMode.Fixed;
            gradient.SetKeys(colourKey, alphaKey);

            lineRenderer.colorGradient = gradient;
        
        // Increase line thickness up to gradient position
        Keyframe[] lineThickness = new Keyframe[3];
            lineThickness[0] = new Keyframe(0, defaultLineThickness);
            lineThickness[1] = new Keyframe(threatSimulationTime, CalculateLineThickness(threatSimulationTime));
            lineThickness[2] = new Keyframe(1, defaultLineThickness);
            lineRenderer.widthCurve = new AnimationCurve(lineThickness);

        if (threatSimulationPercentComplete >= 1) {
            simulatingThreat = false;
            threatSimulationPercentComplete = 0;
            threatSimulationStartNode = null;
            threatSimulationComplete = true;
        }
    }

    // Interpolates between default and max line thickness depending on distance from centre of line
    private float CalculateLineThickness(float time) {
        float distanceFromCenter = 1-(2*Mathf.Abs(0.5f - time));
        float differenceBetweenSizes = maxLineThickness - defaultLineThickness;
        float sizeToAdd = distanceFromCenter * differenceBetweenSizes;
        float newSize = defaultLineThickness + sizeToAdd;

        return newSize;
    }
}
