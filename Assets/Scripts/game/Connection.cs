using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connection : MonoBehaviour {

    public Node start;
    public Node end;
    public float defaultLineThickness;
    public float colliderThickness;
    public LineRenderer lineRenderer;
    public ConnectionCollider connectionCollider;
    
    // Refreshes the positions of the two ends of the line
    public void RefreshPosition() {
        Vector3 startPos = start.transform.position;
        if (end) {
            Vector3 endPos = end.transform.position;
            lineRenderer.SetPosition(1, endPos);
        }

        lineRenderer.SetPosition(0, startPos);
        lineRenderer.startWidth = defaultLineThickness;
        connectionCollider.Refresh();
    }
    
    // Refreshes the gradient of the line
    public void RefreshGradient() {
        Color startColor = start.nodeColor;
        
        GradientColorKey startColorKey = new GradientColorKey(startColor, 0);
        GradientColorKey endColorKey = new GradientColorKey();
        
        GradientAlphaKey startAlphaKey = new GradientAlphaKey(1,0);
        GradientAlphaKey endAlphaKey = new GradientAlphaKey(1,1);  
        
        if (end) {
            Color endColor = end.nodeColor;
            endColorKey = new GradientColorKey(endColor, 1);
        }

        GradientColorKey[] gradientKey = new[] {startColorKey, endColorKey};
        GradientAlphaKey[] alphaKey = new[] {startAlphaKey, endAlphaKey}; 
        
        Gradient gradient = new Gradient();
        gradient.SetKeys(gradientKey, alphaKey);

        lineRenderer.colorGradient = gradient;
    }

    // Sets the ends of the connection, and refreshes the line.
    public void SetEnds(Node start, Node end) {  
        if (start == end) {
            return;
        }
        
        this.start = start;
        this.end = end;
        
        RefreshPosition();
        RefreshGradient();
    }
    
    // Compare against another connection for equality
    public override bool Equals(object other) {
        if (other.GetType() != typeof(Connection)) {
            return false;
        }

        Connection connection = (Connection) other;

        bool startMatches = connection.start == start || connection.end == start;
        bool endMatches = connection.end == start || connection.end == end;

        if (startMatches && endMatches) {
            return true;
        }

        return false;
    }

    // Output a string description of the connection
    public override string ToString() {
        if (end) {
            return "(" + start.nodeName + " -> " + end.nodeName + ")";
        } 
        
        return "(" + start.nodeName + " -> null )";
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            Debug.Log("Connection: "+this+" clicked.");
        }
    }
}
