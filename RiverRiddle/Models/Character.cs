using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiverRiddle.Models
{
    public abstract class Character
    {

        public string Name { get; set; }
        public RiverSide riverSide { get; set; }


    }
}