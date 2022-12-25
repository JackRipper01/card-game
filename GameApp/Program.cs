﻿using GameLibrary;
using GameLibrary.Objects;
using MiniCompiler;
using Spectre.Console;


namespace GameApp;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("//////Welcome//////");
        Print.PressEnterToContinue();
        Console.Clear();

        Console.WriteLine("Would you like to play against AI? (press: 1 = true,0 = false)");
        bool wantIA = UtilsForConsole.UserAnswer();
        Console.WriteLine();
        Console.WriteLine("Creating Decks...");
        var deck1 = Start.GenerateDeck(1);
        var deck2 = Start.GenerateDeck(2);
        Console.WriteLine();

        Console.WriteLine("Choosee a name for Player1: ------ ");
        Player player1 = new ConsolePlayer(new Player(Console.ReadLine(), deck1, 1, false));
        Console.WriteLine("Choosee a name for Player2: ------ ");
        Player player2 = new ConsolePlayer(new Player(Console.ReadLine(), deck2, 2, false));

        // //entradas de usuario de prueba 
        //string Card = "Name: Franco Henandez ; ATK: 0 ; HP: 20 ; Specie: Angel ;";
        //string Card = "Name: Magela Cornelio ; ATK: 2 ; HP: 50 ; Specie: Angel ;";
        // string Effect = "EffectName: Personalized ; ownCard.ATK = targetCard.ATK ; IF: ownCard.HP ownCard.MaxHP < ; ownCard.HP = 5 4 + 1 - ; targetCard.ATK = 1 ; EndIF ; IF: targetCard.HP targetCard.MaxHP 2 / > ; IF: targetCard.HP 50 < ; ownCard.ATK = 50 ; EndIF ; IF: targetCard.HP 50 > ; ownCard.HP = 100 ; EndIF ;";
        // //Explicacion del efecto de arriba :atake de mi carta = atake del enemigo, si mi vida es menor q la mitad de la vida max ,curame 8 de vida y el atake enemigo =1.end if.  si vida enemigo es mayor q su mitad max entonces si su vida es < 50 ,mi atake = 50 fin if. Si vida enemigp > 50 , mi vida = 100. endif

        // //" Effecto de prueba gordo con IF anidado EffectName: Personalized Effect ; enemyCard.AttackValue = 0 ; IF: ownCard.Health ownCard.MaxHealth < ; ownCard.MaxHealth = 5 4 + 1 - ; ownCard.AttackValue = ownCard.MaxAttackValue ; EndIF ; IF: enemyCard.Health enemyCard.MaxHealth 2 / > ; IF: enemyCard.Health 50 > ; EndIF ; IF: enemyCard.Health 50 < ; ownCard.AttackValue = 50 ; ownCard.Health = 100 ; EndIF ;"
        for (int a = 1; a < 3; a++)
        {
            Player player = player1.Number == a ? player1 : player2;

            Console.WriteLine($" {player.Name}, Would you like to create your own Card/Cards? (press: 1 = true ,0 = false");

            bool wantCreateCard = UtilsForConsole.UserAnswer();
            // adding cards to a list to test the interpreter
            if (wantCreateCard)
            {
                Interpreter interpreter = new Interpreter();
                Console.WriteLine();
                Console.WriteLine("Select the amount of Cards to create");
                int amountOfCards = int.Parse(Console.ReadLine());

                for (int i = 0; i < amountOfCards; i++)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Write the code of the New Card Number {i + 1} :");

                    interpreter.EatUserCode(Console.ReadLine());
                    player.Deck.Enqueue(interpreter.BuildCard());
                    interpreter.BuildEffect(player.Deck.Last());

                    Console.WriteLine($"{player.Deck.Last().Name} has born !!");
                    Console.WriteLine();

                    Console.WriteLine("Would you like to add more effects to this Card? (press 1 = true, 0 = false)");
                    if (UtilsForConsole.UserAnswer())
                    {
                        Console.WriteLine("Write the number of Effets to be added");
                        int amountOfAditionalEffects = int.Parse(Console.ReadLine());
                        for (int j = 1; j <= amountOfAditionalEffects; j++)
                        {
                            Console.WriteLine($"Write the code of the Effect to Add :");
                            interpreter.EatUserCode(Console.ReadLine());
                            interpreter.BuildEffect(player.Deck.Last());
                            Console.WriteLine($"{player.Deck.Last().Effects.Last().Name} Effect Added !!");
                        }
                    }
                }

            }
        }
        //creando player AI
        if (wantIA)
            player2 = new ConsolePlayer(new Player(player2.Name, deck2, 2, true));

        var game = new Game(player1, player2);

        Console.Clear();
        Console.WriteLine("////////// THE DUEL HAS STARTED!! ////////////");
        Print.PressEnterToContinue();
        Console.Clear();

        int drawEvery = 2;
        while (!Rules.IsEndOfGame(player1, player2, game))
        {
            Player currentPlayer = Game.GetPlayerTurnOrder(player1, player2, game.TurnCounter)[0];
            Player enemyPlayer = Game.GetPlayerTurnOrder(player1, player2, game.TurnCounter)[1];

            if (Game.IsBeginningOfTurn(game.TurnCounter, drawEvery, player1, player2))
            {
                System.Console.WriteLine("//// Draw Phase ////");
                System.Console.WriteLine();
                System.Console.WriteLine("Player 1 has Draw a Card.");
                System.Console.WriteLine("");
                System.Console.WriteLine("Player 2 has Draw a Card.");
                System.Console.WriteLine();
                Console.WriteLine($"{currentPlayer.Name} Energy +2 .");//en el metodo game.EndTurn es donde es aumentada la energia
                Console.WriteLine($"{enemyPlayer.Name} Energy +2 .");
                Print.PressEnterToContinue();
                System.Console.Clear();
            }
            System.Console.WriteLine($"Is {currentPlayer.Name}'s turn:");
            System.Console.WriteLine();
            Print.GameInformation(currentPlayer, enemyPlayer, game);
            Print.PressEnterToContinue();
            System.Console.Clear();

            ConsolePlayer playingPlayer = (ConsolePlayer)currentPlayer;
            System.Console.WriteLine("//// Invocation Phase ////"); Console.WriteLine(); playingPlayer.PlayInvocationPhase(enemyPlayer, game);
            System.Console.WriteLine("//// Fight Phase ////"); Console.WriteLine(); playingPlayer.Fight(enemyPlayer, game);
            Console.Clear();

            Print.GameInformation(currentPlayer, enemyPlayer, game);
            Print.PressEnterToContinue();
            Console.Clear();

            System.Console.WriteLine("////// End Turn //////");
            System.Console.WriteLine();
            Print.PressEnterToContinue();

            Console.Clear();
            game.EndTurn();//dentro es aumentada la energy de cada player en 2

        }
        Console.WriteLine("///////////GAME OVER//////////");
        Print.PressEnterToContinue();

        Player winnerPlayer = Rules.GetWinnerPlayer(player1, player2, game);
        if (winnerPlayer == null)
            Console.WriteLine("///////////DRAW///////////");
        else
            Console.WriteLine($"THE WINNER WINNER CHICKEN DINNER ISSSSS {winnerPlayer.Name}");
    }


}
