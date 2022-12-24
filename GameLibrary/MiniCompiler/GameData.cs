using GameLibrary.Objects;

namespace MiniCompiler;
public static class GameData
{
    public delegate void Actions();
    public static Dictionary<string, int> CardStats = new Dictionary<string, int>(); //relaciona cada nombre de variable entre ambas cartas con su valor
    public static Dictionary<string, Actions> GameActions = new Dictionary<string, Actions>();

    public static void PreparingGameActionsDic()
    {
        void Void() { }
        GameActions.Add("Draw()", Void);
        GameActions.Add("IncreaseEnergy()", Void);
        GameActions.Add("DecreaseEnergy()", Void);
        GameActions.Add("enemyDraw()", Void);
        GameActions.Add("enemyIncreaseEnergy()", Void);
        GameActions.Add("enemyDecreaseEnergy()", Void);
    }

    public static void PreparingGameStatsDic()
    {
        CardStats.Add("ownCard.Health", 0);
        CardStats.Add("ownCard.MaxHealth", 0);
        CardStats.Add("ownCard.AttackValue", 0);
        CardStats.Add("ownCard.MaxAttackValue", 0);

        CardStats.Add("enemyCard.Health", 0);
        CardStats.Add("enemyCard.MaxHealth", 0);
        CardStats.Add("enemyCard.AttackValue", 0);
        CardStats.Add("enemyCard.MaxAttackValue", 0);

        CardStats.Add("NOCInPlayerHand", 0);
        CardStats.Add("NOCInEnemyPlayerHand", 0);
        CardStats.Add("NOCInPlayerField", 0);
        CardStats.Add("NOCInEnemyPlayerField", 0);

    }

    public static void UpdatingGameActionsDic(Card ownCard, Card enemyCard, Game gameState)
    {
        GameActions["Draw(Player1)"] = gameState.Player1.Draw;
        GameActions["Draw(Player2)"] = gameState.Player2.Draw;
        GameActions["IncreaseEnergy(Player1)"] = gameState.Player1.IncreaseEnergy;
        GameActions["IncreaseEnergy(Player2)"] = gameState.Player2.IncreaseEnergy;
        GameActions["DecreaseEnergy(Player1)"] = gameState.Player1.DecreaseEnergy;
        GameActions["DecreaseEnergy(Player2)"] = gameState.Player2.DecreaseEnergy;
    }

    public static void UpdatingGameStatsDic(Card ownCard, Card enemyCard, Game gameState)
    {
        CardStats["ownCard.Health"] = ownCard.HealthValue;
        CardStats["ownCard.MaxHealthValue"] = ownCard.MaxHealthValue;
        CardStats["ownCard.AttackValue"] = ownCard.AttackValue;
        CardStats["ownCard.MaxAttackValue"] = ownCard.MaxAttackValue;

        CardStats["ownCard.Health"] = enemyCard.HealthValue;
        CardStats["ownCard.MaxHealthValue"] = enemyCard.MaxHealthValue;
        CardStats["ownCard.AttackValue"] = enemyCard.AttackValue;
        CardStats["ownCard.MaxAttackValue"] = enemyCard.MaxAttackValue;

        //note: NOC=number of cards
        CardStats["NOCInHand(Player1)"] = gameState.Player1.Hand.Count;
        CardStats["NOCInHand(Player2)"] = gameState.Player2.Hand.Count;
        CardStats["NOCInField(Player1)"] = GetNOCInPlayerField(gameState, gameState.Player1);
        CardStats["NOCInField(Player2)"] = GetNOCInPlayerField(gameState, gameState.Player2);
        CardStats["NOCInDeck(Player1)"] = gameState.Player1.Deck.Count;
        CardStats["NOCInDeck(Player2)"] = gameState.Player2.Deck.Count;

        CardStats["GetEnergy(Player1)"] = gameState.Player1.Energy;
        CardStats["GetEnergy(Player2)"] = gameState.Player2.Energy;
        //note: Can (and will) be added so many more actions.

    }
    //esto es para modificar las estadisticas de las cartas 
    public static void UpdateCardStats(Card ownCard, Card enemyCard)
    {
        ownCard.HealthValue = CardStats["ownCard.Health"];
        ownCard.MaxHealthValue = CardStats["ownCard.MaxHealthValue"];
        ownCard.AttackValue = CardStats["ownCard.AttackValue"];
        ownCard.MaxAttackValue = CardStats["ownCard.MaxAttackValue"];

        enemyCard.HealthValue = CardStats["ownCard.Health"];
        enemyCard.MaxHealthValue = CardStats["ownCard.MaxHealthValue"];
        enemyCard.AttackValue = CardStats["ownCard.AttackValue"];
        enemyCard.MaxAttackValue = CardStats["ownCard.MaxAttackValue"];
    }

    ///////////////////////// Auxiliar Methods ////////////////////////////

    static int GetNOCInPlayerField(Game gameState, Player player) => gameState.Board[player].Count;


}
