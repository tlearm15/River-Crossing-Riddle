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
        //json is used as http is stateless, so serializing to json allows objects to be stored in session
        private static readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings
        {
            //changing settings to ensure class types are preserved during serialization 
            TypeNameHandling = TypeNameHandling.All
        };

        //key used to store/retrieve game state in session
        private const string SessionKey = "GameState";

        private GameState LoadGame()
        
            {
                //loads game state json from session as string
                var json = Session[SessionKey] as string;

                //if no game state exists in session, create new game state and save it
                if (string.IsNullOrEmpty(json))
                {
                    //create new game state
                    var newGame = new GameState();
                    //save new game state to session using SaveGame method
                    SaveGame(newGame);
                    //return new game state
                    return newGame;
                }
            //if game state exists, deserialize json to GameState object and return it
            return JsonConvert.DeserializeObject<GameState>(json, _jsonSettings);

            }
        

        private void SaveGame(GameState game)
        {
            //serialize GameState object to json and store it in session
            Session[SessionKey] = JsonConvert.SerializeObject(game, _jsonSettings);
        }

        // GET: Game
        public ActionResult PlayGame()
        {
            //load game state from session
            GameState game = LoadGame();
            //return view with game state model
            return View(game);
        }

        [HttpPost]
        public ActionResult TogglePassenger(string name)
        {
            //debug line to check if session exists
            System.Diagnostics.Debug.WriteLine($"Session Exists: {Session[SessionKey] != null}");

            //load game state from session
            var game = LoadGame();
            //toggle passenger on boat
            game.TogglePassenger(name);
            //save updated game state to session
            SaveGame(game);
            //return partial view of game board with updated game state
            return PartialView("_GameBoard",game);
        }

        [HttpPost]
        public ActionResult MoveBoat()
        {
            //debug line to check if session exists
            System.Diagnostics.Debug.WriteLine($"Session Exists: {Session[SessionKey] != null}");

            //load game state from session
            var game = LoadGame();
            //move boat
            game.MoveBoat();
            //save updated game state to session
            SaveGame(game);
            //return partial view of game board with updated game state
            return PartialView("_GameBoard", game);
        }

        [HttpPost]
        public ActionResult ResetGame()
        {
            //removes game state from session to reset the game
            Session.Remove(SessionKey);

            //loads a new game state
            GameState game = LoadGame();
            //updates status to show game has been reset
            game.Status = "Game Reset";
            //saves new game state to session
            return PartialView("_GameBoard",game);
        }
    }
}