using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RiverRiddle.Models
{
    public class GameState
    {
        public Farmer Farmer { get; set; }
        public Fox Fox { get; set; }
        public Chicken Chicken { get; set; }
        public Corn Corn { get; set; }

        public RiverSide boatSide { get; set; } = RiverSide.West;
        public List<Character> boatPassengers { get; set; } = new List<Character>();

        public GameState()
        {
            // initialize characters
            Farmer = new Farmer { riverSide = RiverSide.West };
            Fox = new Fox { riverSide = RiverSide.West };
            Chicken = new Chicken { riverSide = RiverSide.West };
            Corn = new Corn { riverSide = RiverSide.West };
        }


        public void TogglePassenger(string name)
        {
            var passenger = GetCharacterByName(name);
            if (passenger == null) return;

            // Use FirstOrDefault to reliably find the object reference 
            // already on the boat, based on its Type.
            var existingPassenger = boatPassengers.FirstOrDefault(p => p.GetType() == passenger.GetType());

            // Remove if already on the boat
            if (existingPassenger != null)
            {
                // Use the found, existing object reference for successful removal.
                boatPassengers.Remove(existingPassenger);
                return;
            }

            // Passenger must be on the same side as the boat
            if (passenger.riverSide != boatSide) return;

            int maxPassengers = 2; // farmer + 1 other character
            if (boatPassengers.Count >= maxPassengers) return;

            // Add passenger to boat
            boatPassengers.Add(passenger);
        }


        public void MoveBoat()
        {

            if (!boatPassengers.Any(p => p.GetType().Name == "Farmer"))
            {
                // Farmer must be on the boat to move it
                return;
            }

            // Move boat to the other side (flip)
            //boatSide = boatSide == RiverSide.West ? RiverSide.East : RiverSide.West;

            RiverSide newSide = boatSide == RiverSide.West ? RiverSide.East : RiverSide.West;
            boatSide = newSide;

            foreach (var passenger in boatPassengers)
            {
                // Update each passenger's river side to the new boat side
                if (passenger is Farmer)
                {
                    this.Farmer.riverSide = newSide;
                }
                else if (passenger is Fox)
                {
                    this.Fox.riverSide = newSide;
                }
                else if (passenger is Chicken)
                {
                    this.Chicken.riverSide = newSide;
                }
                else if (passenger is Corn)
                {
                    this.Corn.riverSide = newSide;
                }
            }

            // Clear boat passengers after the move
            boatPassengers.Clear();

        }

        private Character GetCharacterByName(string name)
        {
            switch (name.ToLower())
            {
                case "farmer":
                    return Farmer;
                case "fox":
                    return Fox;
                case "chicken":
                    return Chicken;
                case "corn":
                    return Corn;
                default:
                    return null;
            }
        }
    }
}
