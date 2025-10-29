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

        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

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
                return JsonConvert.DeserializeObject<GameState>(json, _jsonSettings);

            }
        

        private void SaveGame(GameState game)
        {
            Session[SessionKey] = JsonConvert.SerializeObject(game, _jsonSettings);
        }

        // GET: Game
        public ActionResult PlayGame()
        {
            GameState game = LoadGame();
            return View(game);
        }

        [HttpPost]
        public ActionResult TogglePassenger(string name)
        {
            System.Diagnostics.Debug.WriteLine($"Session Exists: {Session[SessionKey] != null}");

            var game = LoadGame();
            game.TogglePassenger(name);
            SaveGame(game);
            return PartialView("_GameBoard",game);
        }

        [HttpPost]
        public ActionResult MoveBoat()
        {
            System.Diagnostics.Debug.WriteLine($"Session Exists: {Session[SessionKey] != null}");

            var game = LoadGame();
            game.MoveBoat();
            SaveGame(game);
            return PartialView("_GameBoard", game);
        }
    }
}