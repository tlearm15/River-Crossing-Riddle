using System;
using System.Collections.Generic;

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

            // Remove if already on the boat
            if (boatPassengers.Contains(passenger))
            {
                boatPassengers.Remove(passenger);
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

            if (!boatPassengers.Contains(Farmer))
            {
                // Farmer must be on the boat to move it
                return;
            }

            // Move boat to the other side (flip)
            boatSide = boatSide == RiverSide.West ? RiverSide.East : RiverSide.West;

            foreach (var passenger in boatPassengers)
            {
                // Update each passenger's river side to the new boat side
                passenger.riverSide = boatSide;
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
