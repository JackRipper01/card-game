using GameLibrary;
using GameLibrary.Objects;
namespace MiniCompiler
{
    public interface IInterpreter
    {   //EatUserCode:
        //Recibe el codigo del usuario como string y lo "traduce",guardando en nuevas variables las propiedades de la nueva carta 
        //y guarda en ClientEffectInstruction,especificamente en su propiedad Iinstruction,el arbol donde se ejecutan las instrucciones.

        //BuildCard:
        //Devuelve un objeto tipo Card donde sus propiedades son las extraidas de por el EatUserCode y guardadas en newInitialName,etc,etc mas abajo.

        //BuildEffect:
        //toma una carta y en la lista de efectos de la carta,adiciona el efecto.
        public void EatUserCode(string input);
        public Card BuildCard();
        public void BuildEffect(Card card);
    }
    public class Interpreter : IInterpreter
    {
        List<string> splitedUserInput = new List<string>();//codigo del usuario dividido en strings.
        List<Token> tokenList = new List<Token>();//lista donde se encuentran todos los tokens extraidos del codigo del usuario.
        ClientEffectInstructionsAST userCardEffect = new ClientEffectInstructionsAST();//arbol de instrucciones en el q fue transformado el codigo del usuario.

        //estas variables no se usan d la misma manera q los efectos con el AST pq quiero "fijar" esos valores,por ejemplo el ataque y la vida ,esos valores son la vida y ataque MAXIMOS q es algo fijo.
        //por eso no las uso de la misma manera q las otras caracteristicas de las cartas con el AST.
        //////////////////////////////////
        string newInitialCardName = ""; int newInitialCardATK; int newInitialCardHealth; Species newInitialCardSpecie;
        string newEffectName = "";
        ///////////////////////////////////

        public void EatUserCode(string input)
        //Recibe el codigo del usuario como string y lo "traduce",guardando en nuevas variables las propiedades de la nueva carta 
        //y guarda en ClientEffectInstruction,especificamente en su propiedad Iinstruction,el arbol donde se ejecutan las instrucciones.
        {
            splitedUserInput = new List<string>();
            tokenList = new List<Token>();
            userCardEffect = new ClientEffectInstructionsAST();
            Tokenizing(input);
            ParseCardEffect(userCardEffect.instructions);
        }


        //Devuelve un objeto tipo Card donde sus propiedades son las extraidas de por el EatUserCode y guardadas en newInitialName,etc,etc mas abajo.
        public Card BuildCard() => new Card(newInitialCardName, newInitialCardATK, newInitialCardHealth, newInitialCardSpecie);


        //toma una carta y en la lista de efectos de la carta,adiciona el efecto.
        public void BuildEffect(Card card)
        {
            if (userCardEffect.instructions.Count > 0)
                card.Effects.Add(new UserEffect(newEffectName, userCardEffect));
        }


        private void Tokenizing(string input)
        //analiza cada string de la lista de string y si es compatible con su respectivo token,es adicionado a la lista de token como su correspondiente Token.
        {
            splitedUserInput = input.Split(' ', StringSplitOptions.TrimEntries).ToList();
            for (int position = 0; position < splitedUserInput.Count; position++)
            {
                string subject = splitedUserInput[position];
                if (UtilsForInterpreter.IsNumber(subject))
                { tokenList.Add(new Token(TokenType.NUMBER, subject)); continue; }

                else if (subject.Contains("Card.") || subject.Contains("NOC") || subject.Contains("Get"))
                { tokenList.Add(new Token(TokenType.IDENTIFIER, subject)); continue; }

                else if (subject.Contains("(Player1)") || subject.Contains("(Player2)"))
                { tokenList.Add(new Token(TokenType.ACTION, subject)); continue; }

                switch (subject)
                {
                    case "Name:":
                        tokenList.Add(new Token(TokenType.NAME, UtilsForInterpreter.AddStringFromPositionToEOF(splitedUserInput, position + 1)));
                        break;
                    case "HP:":
                        tokenList.Add(new Token(TokenType.HEALTH));
                        break;
                    case "ATK:":
                        tokenList.Add(new Token(TokenType.ATK));
                        break;
                    case "Specie:":
                        tokenList.Add(new Token(TokenType.SPECIE, UtilsForInterpreter.GetSpecieFromString(splitedUserInput, position + 1)));
                        break;
                    case "EffectName:":
                        tokenList.Add(new Token(TokenType.EFFECTNAME, UtilsForInterpreter.AddStringFromPositionToEOF(splitedUserInput, position + 1)));
                        break;
                    case "=":
                        tokenList.Add(new Token(TokenType.ASSIGN));
                        break;
                    case "IF:":
                        tokenList.Add(new Token(TokenType.IF));
                        break;
                    case "EndIF":
                        tokenList.Add(new Token(TokenType.ENDIF));
                        break;
                    case "+":
                        tokenList.Add(new Token(TokenType.PLUS, "+"));
                        break;
                    case "-":
                        tokenList.Add(new Token(TokenType.MULT, "-"));
                        break;
                    case "*":
                        tokenList.Add(new Token(TokenType.MINUS, "*"));
                        break;
                    case "/":
                        tokenList.Add(new Token(TokenType.DIV, "/"));
                        break;
                    case ";":
                        tokenList.Add(new Token(TokenType.END));
                        break;
                    case ">":
                        tokenList.Add(new Token(TokenType.HIGHER));
                        break;
                    case "<":
                        tokenList.Add(new Token(TokenType.MINOR));
                        break;
                    case "==":
                        tokenList.Add(new Token(TokenType.SAME));
                        break;
                    case "&&":
                        tokenList.Add(new Token(TokenType.AND));
                        break;
                    case "||":
                        tokenList.Add(new Token(TokenType.OR));
                        break;
                    default:
                        throw new FormatException();
                }
            }
        }

        private void ParseCardEffect(List<Iinstruction> instructionList)
        {
            //analiza token por token en la lista de token y eliminando cada token "comido",y va creando el AST
            while (tokenList.Count > 0)
            {//TO DO: make a few methods in UtilsInterpreter to verify if the input is valid
                Token subject = tokenList[0];
                if (subject.type == TokenType.END)
                { tokenList.RemoveAt(0); continue; }
                if (subject.type == TokenType.NAME)
                {
                    newInitialCardName = subject.value;
                    tokenList.RemoveAt(0);
                }
                else if (subject.type == TokenType.EFFECTNAME)
                {
                    newEffectName = subject.value;
                    tokenList.RemoveAt(0);
                }

                else if (subject.type == TokenType.HEALTH)
                {
                    newInitialCardHealth = int.Parse(tokenList[1].value);
                    tokenList.RemoveAt(0);
                    tokenList.RemoveAt(0);
                }

                else if (subject.type == TokenType.ATK)
                {
                    newInitialCardATK = int.Parse(tokenList[1].value);
                    tokenList.RemoveAt(0);
                    tokenList.RemoveAt(0);
                }

                else if (subject.type == TokenType.SPECIE)
                {
                    newInitialCardSpecie = subject.specie;
                    tokenList.RemoveAt(0);
                }

                else if (subject.type == TokenType.IDENTIFIER)
                {
                    if (tokenList[1].type == TokenType.ASSIGN)// <=> si lo q le sigue no es un "=",lanza excepcion
                        tokenList.RemoveAt(1);
                    else
                        throw new FormatException();

                    tokenList.RemoveAt(0);
                    instructionList.Add(new Assignment(subject, ParseExpr(0)));

                }

                else if (subject.type == TokenType.IF)
                {//aqui creo una nueval lista de instruccione para el if,y llamo recursivo para rellenar dicha lista ,caso base seria llegar al final d la lista de tokens,o llegar a un EndIf.
                    List<Iinstruction> newIfInstructionList = new List<Iinstruction>();
                    tokenList.RemoveAt(0);
                    instructionList.Add(new IF(ParseExpr(0), newIfInstructionList));
                    ParseCardEffect(newIfInstructionList);
                }
                else if (subject.type == TokenType.ACTION)
                {
                    instructionList.Add(new Action(subject));
                    tokenList.RemoveAt(0);
                }
                else if (subject.type == TokenType.ENDIF)
                {
                    tokenList.RemoveAt(0);
                    return;
                }
            }
        }

        private IExpr ParseExpr(int startOfExpr)
        {
            //parsea una expresion en concreto,una combinatoria en plan calculos,o una condicion.
            //se utiliza el stack ya q el lenguaje es en modo posfijo.Se trabaja con BinaryExpresion q hereda de IExpr 
            //,la logica seria,analizo cada tipo de token,lo voy pusheando,cuadno llegue al tercer token,analizo q tipo de 
            //operacion es,tanto condicional,como matematica,y sustituyo todo por una instancia de operacion binaria del AST 
            //donde sus elementos o hijos serian otras instancias de operaciones binarias,o serian simplemente valores enteros.

            Stack<IExpr> stack = new Stack<IExpr>();
            while (true)
            {
                //TO DO: make a few methods in UtilsInterpreter to verify if the input is valid
                Token subject = tokenList[startOfExpr];
                if (subject.type == TokenType.END)
                {
                    tokenList.RemoveAt(startOfExpr);
                    break;
                }
                if (subject.type == TokenType.NUMBER)
                {
                    stack.Push(new Number(int.Parse(subject.value)));
                    tokenList.RemoveAt(startOfExpr);
                }
                else if (subject.type == TokenType.IDENTIFIER)
                {
                    stack.Push(new Identifier(subject.value));
                    tokenList.RemoveAt(startOfExpr);
                }
                else if (stack.Count >= 2)
                {
                    var right = stack.Pop(); //el orden importa
                    var left = stack.Pop();

                    if (subject.type == TokenType.PLUS)
                    {
                        stack.Push(new Plus(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.MINUS)
                    {
                        stack.Push(new Minus(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.MULT)
                    {
                        stack.Push(new Mult(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.DIV)
                    {
                        stack.Push(new Div(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.HIGHER)
                    {
                        stack.Push(new Higher(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.MINOR)
                    {
                        stack.Push(new Minor(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.SAME)
                    {
                        stack.Push(new Same(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.AND)
                    {
                        stack.Push(new AND(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                    else if (subject.type == TokenType.OR)
                    {
                        stack.Push(new OR(left, right));
                        tokenList.RemoveAt(startOfExpr);
                    }
                }
            }
            return stack.Pop();//retorna un solo nodo expresion binaria donde sus hijos y los hijos de sus hijos .....  son todas las expresiones binarias creadas.
        }

    }
}