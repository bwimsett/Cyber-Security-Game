using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.node {
    public class NodeAttackAnimationMonitor : MonoBehaviour {

        public bool finishedAttacking = false;
        public Image attackCircle;
        public Animator attackAnimator;

        void Start() {
            attackCircle.color = GameManager.levelScene.threatManager.threatColor;
        }

        public void Reset() {
            finishedAttacking = false;
            attackAnimator.SetBool("attacked", false);
            attackAnimator.SetTrigger("reset");
        }
        
        public void FinishedAttacking() {
            finishedAttacking = true;
        }

    }
}