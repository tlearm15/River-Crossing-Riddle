using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiverRiddle.Models
{
    public class GameState
    {

        public Farmer Farmer { get; set; }
        public Fox Fox { get; set; }
        public Chicken Chicken { get; set; }
        public Corn Corn { get; set; }

        public RiverSide boatSide { get; set; } = RiverSide.West;

        public GameState()
        {

            //initialize characters
            Farmer = new Farmer();
            Fox = new Fox();
            Chicken = new Chicken();
            Corn = new Corn();

            //set initial river sides
            Farmer.riverSide = RiverSide.West;
            Fox.riverSide = RiverSide.West;
            Chicken.riverSide = RiverSide.West;
            Corn.riverSide = RiverSide.West;

            List<Characters> boatPassangers = new List<Characters>(); //initialize empty list of boat passengers

        }

        public void TogglePassenger(string name)
        {
            var passenger = GetCharacterName(name);
            
            //checks passanger is not null
            if (passenger != null)
            {
               if (boatPassengers.contains(passenger))
               {
                   //removes passanger from boat
                   boatPassengers.Remove(passenger); 
                    return;
                }
               
               //passenger must be on same side as boat
               if (passenger.RiverSide != boatSide)
                {
                    return;
                }

               int maxPassengers = 2; //farmer + 1 other character
                if (boatPassangers.Count >= maxPassengers)
                {
                    //boat is full
                    return; 
                }

                //passenger is valid, add to boat
                boatPassengers.Add(passenger);

            }

        }

        private Characters GetCharacterName(string name)
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