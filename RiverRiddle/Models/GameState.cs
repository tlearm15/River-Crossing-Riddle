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
        public MoveState MoveState { get; set; } = MoveState.Valid;
        public string Status { get; set; } = "Game Started!";

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

                this.MoveState = ValidateMove();
                if (this.MoveState == MoveState.Win)
                {
                    this.Status = "Congratulations! You Win!";
                }
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
                this.Status = "The farmer must be on the boat to move it.";
                return;
            }

            // Move boat to the other side (flip)
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
            //validates the move after updating positions to check for win/loss conditions
            this.MoveState = ValidateMove();

            switch (this.MoveState)
            {
                case MoveState.Valid:
                    this.Status = "Move Successful";
                    break;
                    // successful move status message
                case MoveState.Invalid:

                    bool foxEatChicken = (Fox.riverSide == Chicken.riverSide) && (Farmer.riverSide != Fox.riverSide);
                    if (foxEatChicken)
                    {
                        this.Status = "The Fox has eaten the Chicken!";
                    }
                    // status message for if the fox has been left alone with the chicken
                    else
                    {
                        this.Status = "The Chicken has eaten the Corn!";
                    }
                    // status message for if the chicken has been left alone with the corn
                    break;
                case MoveState.Win:
                    this.Status = "Congratulations! You Win!";
                    break;
                    // winning status message
            }
        }

        private MoveState ValidateMove()
        {
            //Fox eats chicken if on same side as chicken without farmer
            bool foxEatChicken = (Fox.riverSide == Chicken.riverSide) && (Farmer.riverSide != Fox.riverSide);

            //Chicken eats corn if on same side as corn without farmer
            bool chickenEatCorn = (Chicken.riverSide == Corn.riverSide) && (Farmer.riverSide != Chicken.riverSide);

            if (foxEatChicken || chickenEatCorn)
            {
                return MoveState.Invalid;
            }

            //Check for win condition (everyone on east side)
            if (Farmer.riverSide == RiverSide.East &&
                Fox.riverSide == RiverSide.East &&
                Chicken.riverSide == RiverSide.East &&
                Corn.riverSide == RiverSide.East &&
                boatPassengers.Count == 0)
            {
                 return MoveState.Win;
            }

            return MoveState.Valid;
        }

        //Helper method to get characters object from name
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
