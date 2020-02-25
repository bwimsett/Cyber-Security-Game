using System;
using TMPro;
using UnityEngine;

namespace gui.levelSummary {
    public class LevelSummary_Medal : MonoBehaviour {

    public TextMeshProUGUI medalText;
        
    private float sizeFalloff;
    private Vector3 targetPos;
    private float speed;
    private float tolerance;
    private float minScale;

    private bool targetReached;


    void Update() {
        if (targetReached) {
            return;
        }

        // Adjust target to take the scale of the medal into account
        Vector3 newTarget = Vector3.Lerp(Vector3.zero, targetPos, transform.localScale.x*1.4f);
        
        
        // Animate position
        if (Vector3.Distance(newTarget, transform.localPosition) > tolerance) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newTarget, speed*Time.deltaTime);
        }
        else {
            transform.localPosition = newTarget;
            targetReached = true;
        }

        // Calculate scale to be inversely proportionate with distance to the center
        float distanceFromCenter = Vector3.Distance(transform.localPosition, Vector3.zero);
        float scale = 1 - (distanceFromCenter * sizeFalloff);

        scale = Mathf.Max(scale, minScale);

        transform.localScale = new Vector3(scale, scale, scale);
    }


    public void SetTargets(Vector3 targetPos, float sizeFalloff, float speed, float tolerance, float minScale) {
        this.targetPos = targetPos;
        this.sizeFalloff = sizeFalloff;
        this.speed = speed;
        this.tolerance = tolerance;
        this.minScale = minScale;
        targetReached = false;
    }

    public void SetText(string text) {
        if (!medalText) {
            return;
        }
        
        medalText.text = text;
    }
    }
}