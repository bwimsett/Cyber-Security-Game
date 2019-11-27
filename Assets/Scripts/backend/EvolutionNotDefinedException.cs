using System;
using UnityEngine.UIElements;

namespace backend {
    public class EvolutionNotDefinedException : Exception {

        public EvolutionNotDefinedException(string message) : base(message){
            
        }
        
    }
}