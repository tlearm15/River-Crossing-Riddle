using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RiverRiddle.Models;
using Newtonsoft.Json;

namespace RiverRiddle.Controllers
{
    public class GameController : Controller
    {

        private const string SessionKey = "GameState";

        private GameState LoadGame()
        
            {
                var json = Session[SessionKey] as string;
                if (string.IsNullOrEmpty(json))
                {
                    var newGame = new GameState();
                    SaveGame(newGame);
                    return newGame;
                }
                return JsonConvert.DeserializeObject<GameState>(json);

            }
        

        private void SaveGame(GameState game)
        {
            Session[SessionKey] = JsonConvert.SerializeObject(game);
        }

        // GET: Game
        public ActionResult PlayGame()
        {
            var game = LoadGame();
            return View(game);
        }

        [HttpPost]
        public ActionResult TogglePassenger(string name)
        {
            var game = LoadGame();
            game.TogglePassenger(name);
            SaveGame(game);
            return RedirectToAction("PlayGame");
        }

        [HttpPost]
        public ActionResult MoveBoat()
        {
            var game = LoadGame();
            game.MoveBoat();
            SaveGame(game);
            return RedirectToAction("PlayGame");
        }
    }
}