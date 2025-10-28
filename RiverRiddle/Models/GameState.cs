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

        public RiverSide BoatSide { get; set; } = RiverSide.West;
        public List<Character> BoatPassengers { get; set; } = new List<Character>();

        public GameState()
        {
            // initialize characters
            Farmer = new Farmer { RiverSide = RiverSide.West };
            Fox = new Fox { RiverSide = RiverSide.West };
            Chicken = new Chicken { RiverSide = RiverSide.West };
            Corn = new Corn { RiverSide = RiverSide.West };
        }

        public void TogglePassenger(string name)
        {
            var passenger = GetCharacterByName(name);
            if (passenger == null) return;

            // Remove if already on the boat
            if (BoatPassengers.Contains(passenger))
            {
                BoatPassengers.Remove(passenger);
                return;
            }

            // Passenger must be on the same side as the boat
            if (passenger.RiverSide != BoatSide) return;

            int maxPassengers = 2; // farmer + 1 other character
            if (BoatPassengers.Count >= maxPassengers) return;

            // Add passenger to boat
            BoatPassengers.Add(passenger);
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
