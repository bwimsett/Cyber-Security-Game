using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;


public class Connection : MonoBehaviour {

    public Node start;
    public Node end;
    public float defaultLineThickness;
    public float colliderThickness;
    public LineRenderer lineRenderer;
    public ConnectionCollider connectionCollider;
    public Sprite duplexChevron;
    public Sprite simplexChevron;
    
    // True if data flows in both directions for this connection
    private bool duplex;
    
    public SpriteRenderer connectionChevron;

    public void Refresh() {
        RefreshPosition();
        RefreshGradient();
        RefreshChevron();
    }
    
    // Refreshes the positions of the two ends of the line
    public void RefreshPosition() {
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

        Vector2 pos = Vector2.Lerp(pos1, pos2, 0.5f);
        connectionChevron.transform.localPosition = pos;

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
}
