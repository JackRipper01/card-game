using GameLibrary.Objects;

namespace MiniCompiler;
public static class GameData
{
    public delegate void Actions();
    public static Dictionary<string, int> CardStats = new Dictionary<string, int>(); //relaciona cada nombre de variable en string con su valor en el juego
    public static Dictionary<string, Actions> GameActions = new Dictionary<string, Actions>();//relaciona cada nombre de accion en string con su accion en el juego
    public static void PreparingGameActionsDic()
    // rellena el diccionario con los string de las acciones para q luego solo tenga q ser actualizado 
    {
        void Void() { }
        GameActions.Add("Draw(Player1)", Void);
        GameActions.Add("Draw(Player2)", Void);
        GameActions.Add("IncreaseEnergy(Player1)", Void);
        GameActions.Add("IncreaseEnergy(Player2)", Void);
        GameActions.Add("DecreaseEnergy(Player1)", Void);
        GameActions.Add("DecreaseEnergy(Player2)", Void);
    }

    public static void PreparingGameStatsDic()// rellena el diccionario con los string de las variables  para q luego solo tenga q ser actualizado 
    {
        CardStats.Add("ownCard.HP", 0);
        CardStats.Add("ownCard.MaxHP", 0);
        CardStats.Add("ownCard.ATK", 0);
        CardStats.Add("ownCard.MaxATK", 0);

        CardStats.Add("targetCard.HP", 0);
        CardStats.Add("targetCard.MaxHP", 0);
        CardStats.Add("targetCard.ATK", 0);
        CardStats.Add("targetCard.MaxATK", 0);

        CardStats.Add("NOCInHand(Player1)", 0);
        CardStats.Add("NOCInHand(Player2)", 0);
        CardStats.Add("NOCInField(Player1)", 0);
        CardStats.Add("NOCInField(Player2)", 0);
        CardStats.Add("NOCInDeck(Player1)", 0);
        CardStats.Add("NOCInDeck(Player2)", 0);
        CardStats.Add("GetEnergy(Player1)", 0);
        CardStats.Add("GetEnergy(Player2)", 0);


    }

    public static void UpdatingGameActionsDic(Card ownCard, Card targetCard, Game gameState)
    //actualiza cada accion del diccionario con una verdadera accion q parte del delegado mas arriba " void Actions() "para q puedan ser guardadas en el diccionario.
    {
        GameActions["Draw(Player1)"] = gameState.Player1.Draw;
        GameActions["Draw(Player2)"] = gameState.Player2.Draw;
        GameActions["IncreaseEnergy(Player1)"] = gameState.Player1.IncreaseEnergy;
        GameActions["IncreaseEnergy(Player2)"] = gameState.Player2.IncreaseEnergy;
        GameActions["DecreaseEnergy(Player1)"] = gameState.Player1.DecreaseEnergy;
        GameActions["DecreaseEnergy(Player2)"] = gameState.Player2.DecreaseEnergy;
    }

    public static void UpdatingGameStatsDic(Card ownCard, Card targetCard, Game gameState)
    //actualiza cada valor del diccionario ,con su valor correspondiente del juego
    {
        CardStats["ownCard.HP"] = ownCard.HealthValue;
        CardStats["ownCard.MaxHP"] = ownCard.MaxHealthValue;
        CardStats["ownCard.ATK"] = ownCard.AttackValue;
        CardStats["ownCard.MaxATK"] = ownCard.MaxAttackValue;

        CardStats["targetCard.HP"] = targetCard.HealthValue;
        CardStats["targetCard.MaxHP"] = targetCard.MaxHealthValue;
        CardStats["targetCard.ATK"] = targetCard.AttackValue;
        CardStats["targetCard.MaxATK"] = targetCard.MaxAttackValue;

        //note: NOC=number of cards
        CardStats["NOCInHand(Player1)"] = gameState.Player1.Hand.Count;
        CardStats["NOCInHand(Player2)"] = gameState.Player2.Hand.Count;
        CardStats["NOCInField(Player1)"] = GetNOCInPlayerField(gameState, gameState.Player1);
        CardStats["NOCInField(Player2)"] = GetNOCInPlayerField(gameState, gameState.Player2);
        CardStats["NOCInDeck(Player1)"] = gameState.Player1.Deck.Count;
        CardStats["NOCInDeck(Player2)"] = gameState.Player2.Deck.Count;

        CardStats["GetEnergy(Player1)"] = gameState.Player1.Energy;//Get es clave ya q significa ''obterer valor de''
        CardStats["GetEnergy(Player2)"] = gameState.Player2.Energy;
        //note: Can (and will) be added so many more actions.

    }

    public static void UpdateCardStats(Card ownCard, Card targetCard)
    //actualiza cada estadistica de las cartas interactuando en juego ,con los valores actuales del diccionario.
    {
        ownCard.HealthValue = CardStats["ownCard.HP"];
        ownCard.MaxHealthValue = CardStats["ownCard.MaxHP"];
        ownCard.AttackValue = CardStats["ownCard.ATK"];
        ownCard.MaxAttackValue = CardStats["ownCard.MaxATK"];

        targetCard.HealthValue = CardStats["targetCard.HP"];
        targetCard.MaxHealthValue = CardStats["targetCard.MaxHP"];
        targetCard.AttackValue = CardStats["targetCard.ATK"];
        targetCard.MaxAttackValue = CardStats["targetCard.MaxATK"];
    }

    ///////////////////////// Auxiliar Methods ////////////////////////////

    static int GetNOCInPlayerField(Game gameState, Player player) => gameState.Board[player].Count;


}
