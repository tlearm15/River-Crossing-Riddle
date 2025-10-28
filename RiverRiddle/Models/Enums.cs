using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiverRiddle.Models
{

    public enum RiverSide
    {
        East,
        West
    }


    public enum MoveState
        {
            Valid,
            Invalid,
            Win
            //Lose
        }
}