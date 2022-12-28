namespace GameLibrary.Objects;



public static class Rules
{
    public static bool CanInvoke(Player currentPlayer, Game game) => currentPlayer.Energy - 3 >= 0 && currentPlayer.Hand.Count != 0 && game.Board[currentPlayer].Count < Game.BoardSize;
    public static bool CanFight(Player currentPlayer, Player enemyPlayer, Game game) => currentPlayer.Energy - 2 >= 0 && game.Board[currentPlayer].Count != 0 && game.Board[enemyPlayer].Count != 0;
    public static void InvokeCard(Player currentPlayer, Dictionary<Player, List<Card>> board, int userInput)
    {
        if (board[currentPlayer].Count >= Game.BoardSize)
            throw new Exception("No puedes invocar , campo lleno.");
        var cardToInvoke = currentPlayer.Hand.ElementAt(userInput);
        currentPlayer.Invoke(cardToInvoke, board);
        currentPlayer.DecreaseEnergy(3);
    }
    public static void AttackCard(Player currentPlayer, Player currentOpponent, int attackingCardCoordinates, int targetCardCoordinates, Game game)
    {
        var attackingCard = game.Board[currentPlayer].ElementAt(attackingCardCoordinates);
        var cardToAttack = game.Board[currentOpponent].ElementAt(targetCardCoordinates);
        attackingCard.Attack(cardToAttack);
        currentPlayer.DecreaseEnergy(2);//atacar cuesta 2 de energia
    }
    public static void CastEffect(Player currentPlayer, Player enemyPlayer, int cardCoordinates, int targetCardCoordinates, Game game, IEffect effect)
    {
        Card ownCard = game.Board[currentPlayer][cardCoordinates];//obteniendo la carta seleccionada
        Card targetCard = game.Board[enemyPlayer][targetCardCoordinates];//obteniendo la carta objetivo

        effect.ActivateEffect(ownCard, targetCard, game, currentPlayer.Number, enemyPlayer.Number);
        currentPlayer.DecreaseEnergy(3);//lanzar efecto cuesta 2 de energia

    }
    public static bool IsEndOfGame(Player Player1, Player Player2, Game game)
    {
        if (HasLost(Player1, game) || HasLost(Player2, game))
            return true;
        else if (game.TurnCounter >= 20)
            return true;
        return false;
    }
    public static Player GetWinnerPlayer(Player player1, Player player2, Game game)
    {
        if (HasLost(player1, game))
            return player2;
        if (HasLost(player2, game))
            return player1;
        else
        {
            double player1Score = GetFinalScore(player1, game);
            double player2Score = GetFinalScore(player2, game);

            if (player1Score > player2Score)
                return player1;
            else if (player1Score < player2Score)
                return player2;
            else if (player1Score == player2Score)
                return null;
        }
        return null;
    }
    public static bool HasLost(Player player, Game game)//basicamente si no tienes nada,nignuna carta,pierdes
    => player.Deck.Count == 0 && player.Hand.Count == 0 && game.Board[player].Count == 0;

    public static double GetFinalScore(Player player1, Game game)//suma la vida de todas las cartas sobrevivientes,esto hace entender q no es un juego de matar simplemente,es un juego de sobrevivir
    {
        double result = 0;
        foreach (Card card in game.Board[player1])
        {
            result += card.HealthValue;
        }
        return result;
    }
}
//public static List<string> GetRulesList(Player currentPlayer, Player enemyPlayer, Game game)
// {
//     List<string> actions = new List<string>();
//     Player playingPlayer = Game.GetPlayerTurnOrder(currentPlayer, enemyPlayer, game.TurnCounter)[0];
//     if (playingPlayer.Hand.Count != 0)
//         actions.Add("Invocation's Phase");
//     if ((playingPlayer.Energy != 0) && (game.Board[playingPlayer].Count != 0))
//         actions.Add("Fight's Phase");
//     actions.Add("End Turn");
//     return actions;
// }
