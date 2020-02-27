/*
 * Used when summarising a threat. Describes different outcomes depending on the node.
 */

using DefaultNamespace;

namespace backend {
    [System.Serializable]
    public class ThreatSummaryContext {

        public NodeType node;
        public string summary;

    }
}