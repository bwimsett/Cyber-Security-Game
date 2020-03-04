/*
 * ScriptableObject, outlines all the different sentences a given threat can return
 */

using DefaultNamespace;
using UnityEngine;

namespace backend {
    [CreateAssetMenu(fileName = "Threat Summary", menuName ="Threat Summary")]
    public class ThreatSummary : ScriptableObject {

        
        public ThreatType threatType;
        public string threatName;

        public ThreatSummaryContext[] startSentences; // Used when this threat is the root
        public ThreatSummaryContext[] middleSentences;
        public ThreatSummaryContext[] endSentences;

    }
}