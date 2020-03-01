using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeHealthBar : MonoBehaviour {
    public RectTransform container;
    public Image bar;
    public CanvasGroup canvasGroup;

    public void SetHealth(int health, int max) {
        health = Mathf.Max(0, health);

        float percent = (float) health / max;

        float barWidth = container.rect.width* percent;

        bar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, barWidth);
    }

    public void SetVisible(bool visible) {
        if (visible) {
            canvasGroup.alpha = 1;
            return;
        }

        canvasGroup.alpha = 0;
    }

}
