using ConOOD_RpgGame.Model;
using ConOOD_RpgGame.Network.DTO;

namespace ConOOD_RpgGame.World.Controllers
{
    public class GameController
    {
        private readonly MultiplayerGameModel model;

        public GameController(MultiplayerGameModel model)
        {
            this.model = model;
        }

        public bool HandlePlayerAction(NetworkMessageDto message)
        {
            if (message == null)
            {
                return false;
            }

            if (!IsValidAction(message.Action))
            {
                return false;
            }

            model.EnqueueAction(message.PlayerId, message.Action);
            return true;
        }

        private bool IsValidAction(string action)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                return false;
            }

            if (action == "UP") return true;
            if (action == "DOWN") return true;
            if (action == "LEFT") return true;
            if (action == "RIGHT") return true;
            if (action == "PICKUP") return true;
            if (action == "INVENTORY") return true;
            if (action == "ATTACK_NORMAL") return true;
            if (action == "ATTACK_STEALTH") return true;
            if (action == "ATTACK_MAGICAL") return true;

            if (action.StartsWith("INV_")) return true;

            return false;
        }
    }
}