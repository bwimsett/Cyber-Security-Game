/*
 * Summarises a threat into a readable sentence, describing its path through the system.
 */

using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace backend {
    public class ThreatSummariser : MonoBehaviour {

        public ThreatSummary[] threatSummaries;
        public string[] evolutionAdjectives; // Words that can be used to describe evolution

        private enum SummaryPosition {
            Start,
            Middle,
            End
        };

        public string SummariseThreat(Threat threat) {
            string summary = "";
            summary += GetStartSummary(threat);
            //summary += GetMiddleSummary(threat);
            summary += GetEndSummary(threat);
            return summary;
        }

        private string GetStartSummary(Threat threat) {
            Threat root = threat.GetRoot();

            ThreatSummary rootSummary = GetSummary(root);

            if (rootSummary == null) {
                return root.threatType + " originating at " + root.GetNode().nodeObject.GetNodeDefinition().nodeName;
            }

            return GetStringFromSummary(root, rootSummary, SummaryPosition.Start);
        }

        private string GetMiddleSummary(Threat threat) {
            Threat prevThreat = threat;
            Threat currentThreat = threat.GetParent();

            List<string> outputParts = new List<string>();
            
            while(currentThreat != null) {
                
                // If an evolution has been found, construct a sentence describing it
                if (currentThreat.GetStatus() == ThreatStatus.Evolve) {
                    int chosenAdjective = Random.Range(0, evolutionAdjectives.Length);
                    string outputPart = evolutionAdjectives[chosenAdjective]+" ";
                    outputPart += prevThreat.threatType + " at " +
                                  prevThreat.GetNode().nodeObject.GetNodeDefinition().nodeName;
                    outputParts.Add(outputPart);
                }

                prevThreat = currentThreat;
                currentThreat = prevThreat.GetParent();
            }

            string outputString = "";
            
            // Convert output parts into a single string.
            foreach (string s in outputParts) {
                outputString += ", " + s;
            }
            
            return outputString;
        }

        private string GetEndSummary(Threat threat) {
            ThreatSummary summary = GetSummary(threat);

            if (summary == null) {
                return ", resulting in "+threat.threatType + " at " + threat.GetNode().nodeObject.GetNodeDefinition().nodeName;
            }

            return " "+GetStringFromSummary(threat, summary, SummaryPosition.End);
        }

        public string GetThreatName(Threat threat) {
            ThreatSummary threatSummary = GetSummary(threat);
            if (threatSummary) {
                return GetSummary(threat).threatName;
            }

            return "";
        }

        private ThreatSummary GetSummary(Threat threat) {
            foreach (ThreatSummary s in threatSummaries) {
                if (s.threatType == threat.threatType) {
                    return s;
                }
            }

            return null;
        }

        private string GetStringFromSummary(Threat threat, ThreatSummary summary, SummaryPosition pos) {
            
            // Establish what set of sentences the string should come from based on position
            ThreatSummaryContext[] set = null;           
            
            switch (pos) {
                case SummaryPosition.Start: set = summary.startSentences;
                    break;
                case SummaryPosition.Middle: set = summary.middleSentences;
                    break;
                case SummaryPosition.End: set = summary.endSentences;
                    break;
            }

            if (set == null || set.Length == 0) {
                return "";
            }

            // Find options from the set which relate to this node in particular
            List<ThreatSummaryContext> optionsFromSet = new List<ThreatSummaryContext>();
            
            foreach(ThreatSummaryContext context in set) {
                if (context.node == threat.GetNode().nodeObject.GetNodeDefinition().nodeType) {
                    optionsFromSet.Add(context);
                }
            }

            if (optionsFromSet.Count == 0) {
                return "";
            }
            
            // Select an option at random from the ones available
            int selectedPos = Random.Range(0, optionsFromSet.Count);
            ThreatSummaryContext selectedContext = optionsFromSet[selectedPos];

            // Return the selected string
            return selectedContext.summary;
        }
        
    }
}