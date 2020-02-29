using System;
using System.Collections.Generic;
using backend;
using backend.level_serialization;
using backend.threat_modelling;
using DefaultNamespace.node;
using UnityEngine;

/*
 * Classes implementing this interface are where the Attack() method is defined for each node type.
 *
 * It is also where modifiable parameters are kept.
 */


namespace DefaultNamespace {
    public class NodeBehaviour {

        protected Node node;
        protected NodeField[] fields;
        protected NodeField selectedStartingThreats;
        protected int startingHealth;
        protected int[] threatStrengths;
        
        public NodeBehaviour(Node node) {
            this.node = node;
            threatStrengths = new int[node.nodeObject.GetNodeDefinition().GetStartingThreatsOptionSet().options.Length];
            InitialiseFields();
        }

        protected virtual void InitialiseFields() {
            
        }
        
        public virtual void InitialiseStartingThreatSet() {
            Control_Dropdown_Option_Set optionSet = node.nodeObject.GetNodeDefinition().GetStartingThreatsOptionSet();
            selectedStartingThreats = new NodeField("Starting Threats", optionSet);
        }
        
        public virtual ThreatStatus Attack(Threat threat) {       
            Node[] connectedNodes = node.GetConnectedNodes();
            
            threat.SetNodeHealth(startingHealth);
            
            // Check if any of the logical nodes connected are effective against the threat
            foreach (Node n in connectedNodes) {
                if (n.nodeObject.GetNodeDefinition().nodeFamily == NodeFamily.Logical) {
                    ThreatStatus status = n.GetThreatEffect(threat);
                    if (status == ThreatStatus.Failure) {
                        return status;
                    }
                }
            }

            return node.GetThreatEffect(threat);
        }

        public NodeField[] GetFields() {
            return fields;
        }
        
        public void SetFields(NodeSave nodeSave) {
            fields = nodeSave.fields;

            char[] selectedStartingThreatsMask = (char[]) nodeSave.selectedStartingThreats.GetValue();

            // If the selected starting threats mask matches the number of options
            if (selectedStartingThreatsMask.Length == selectedStartingThreats.GetOptionSet().options.Length) {
                selectedStartingThreats = nodeSave.selectedStartingThreats;
            }
        }

        public virtual Threat[] GenerateThreats() {
            char[] selectedThreats = (char[])selectedStartingThreats.GetValue();
            
            List<Threat> outputThreats = new List<Threat>();

            ThreatType[] threatTypes = node.nodeObject.GetNodeDefinition().startingThreats;
            
            for(int i = 0; i < selectedThreats.Length; i++) {
                if (selectedThreats[i] != '1') {
                    continue;
                }
                
                Threat t = new Threat(threatTypes[i], null, node, threatStrengths[i]);
                outputThreats.Add(t);
            }

            return outputThreats.ToArray();
        }

        public NodeField GetFieldWithSet(ControlDropdownOptionSets set) {
            foreach(NodeField n in fields) {

                Control_Dropdown_Option_Set optionSet = n.GetOptionSet();

                if (n.GetOptionSetName() == set) {
                    return n;
                }   
            }

            return null;
        }
        
        public int GetTotalHealth() {
            int totalHealth = 0;
            
            foreach (NodeField f in fields) {
                NodeFieldType fieldType = f.GetFieldType();
                
                // Health from dropdowns
                if (fieldType == NodeFieldType.enumerable_single) {
                    totalHealth += f.GetOptionSet().options[(int) f.GetValue()].health;
                    continue;
                }

                // Health from tickboxes
                if (fieldType == NodeFieldType.enumerable_many) {
                    char[] bitmask = (char[]) f.GetValue();

                    for(int i = 0; i < bitmask.Length; i++) {
                        // Multiplies health of that option with the bitmask value of 1 or 0
                        int bitValue = Int32.Parse(""+bitmask[i]);
                        totalHealth += f.GetOptionSet().options[i].health * bitValue;
                    }
                }

                continue;
            }

            return totalHealth;
        }

        public NodeField GetSelectedStartingThreats() {
            return selectedStartingThreats;
        }

    }
}