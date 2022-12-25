namespace MiniCompiler;
interface IExpr //creo esta interfaz para solamente llamar .Evaluate a cada tipo distinto de expresion,tanto binaria,como no binaria.
{
    public int Evaluate();
}
abstract class BinaryExpr : IExpr//clase d la cual heredaran todas las expresiones binarias .
{
    public IExpr left;
    public IExpr right;
    public BinaryExpr(IExpr left, IExpr right)
    {
        this.left = left;
        this.right = right;
    }
    public abstract int Evaluate();
}

class Plus : BinaryExpr//operacion binaria q suma el resultado de evaluar la expresion de la izq mas el resultado de hacer lo mismo en la derecha,devuelve  el resultado de la suma
{
    public Plus(IExpr left, IExpr right) : base(left, right) { }
    public override int Evaluate()
    {
        return left.Evaluate() + right.Evaluate();
    }
}

class Minus : BinaryExpr
{
    public Minus(IExpr left, IExpr right) : base(left, right) { }
    public override int Evaluate()
    {
        return left.Evaluate() - right.Evaluate();
    }
}

class Mult : BinaryExpr
{
    public Mult(IExpr left, IExpr right) : base(left, right) { }
    public override int Evaluate()
    {
        return left.Evaluate() * right.Evaluate();
    }
}

class Div : BinaryExpr
{
    public Div(IExpr left, IExpr right) : base(left, right) { }
    public override int Evaluate()
    {
        if (right.Evaluate() == 0)
            throw new Exception("Division by 0");

        return left.Evaluate() / right.Evaluate();
    }
}

class Number : IExpr//expresion basica no binaria,devuelve el valor de la expresion
{
    public int value;
    public Number(int value)
    {
        this.value = value;
    }
    public int Evaluate()
    {
        return value;
    }
}

class Identifier : IExpr//expresion basica no binaria q representa el valor actual en el juego,de alguna estadistica del juego ,este valor es extraido del diccionario de estadisticas de cartas.
{
    string key;
    public Identifier(string key) => this.key = key;

    public int Evaluate() => GameData.CardStats[key];
}

class Higher : BinaryExpr//operacion binaria q verifica si el resultado de evaluar la expresion de la isq es MAYOR QUE el resultado de hacer lo mismo en la derecha,devuelve true si se cumple,false si no.
{
    public Higher(IExpr left, IExpr right) : base(left, right) { }

    public override int Evaluate()
    {
        if (left.Evaluate() > right.Evaluate())
            return 1;
        else
            return 0;
    }
}

class Minor : BinaryExpr//operacion binaria q verifica si el resultado de evaluar la expresion de la isq es MENOR QUE el resultado de hacer lo mismo en la derecha,devuelve true si se cumple,false si no.
{
    public Minor(IExpr left, IExpr right) : base(left, right) { }

    public override int Evaluate()
    {
        if (left.Evaluate() < right.Evaluate())
            return 1;
        else
            return 0;
    }
}

class Same : BinaryExpr//operacion binaria q verifica si el resultado de evaluar la expresion de la isq es IGUAL QUE el resultado de hacer lo mismo en la derecha,devuelve 1(true) si se cumple,0(false) si no.
{
    public Same(IExpr left, IExpr right) : base(left, right) { }

    public override int Evaluate()
    {
        if (left.Evaluate() == right.Evaluate())
            return 1;
        else
            return 0;
    }
}

class AND : BinaryExpr//operacion binaria q verifica si el resultado de evaluar la expresion de la isq Y  el resultado de hacer lo mismo en la derecha devuelven 1(true)los dos,en dicho caso devolveria 1,sino se cumple dicha condicion devuelve 0(false).
{
    public AND(IExpr left, IExpr right) : base(left, right) { }

    public override int Evaluate()
    {
        if (left.Evaluate() == 1 && right.Evaluate() == 1)
            return 1;
        else
            return 0;
    }
}

class OR : BinaryExpr//operacion binaria q devuelve 1 si el resultado de evaluar la exp isq devuelve 1 o si pasa lo mismo con la de la derecha.Si no se cumple devuelve 0.
{
    public OR(IExpr left, IExpr right) : base(left, right) { }

    public override int Evaluate()
    {
        if (left.Evaluate() == 1 || right.Evaluate() == 1)
            return 1;
        else
            return 0;
    }
}


public interface Iinstruction//forma de una instruccion,una asignacion de un valor a un identificadr por ejemplo,un If,el nodo raiz,etc.
{
    void Execute();
}

class IF : Iinstruction//verifica la condicion ,si devuelve 1,entonces ejecuta la instruccion de la lista de instrucciones,sino no hace nada.
{
    IExpr condition;
    List<Iinstruction> ifInstructions = new List<Iinstruction>();

    public IF(IExpr condition, List<Iinstruction> ifInstructions)
    {
        this.condition = condition;
        this.ifInstructions = ifInstructions;
    }
    public void Execute()
    {
        if (condition.Evaluate() == 1)
        {
            for (int position = 0; position < ifInstructions.Count; position++)
            {
                ifInstructions[position].Execute();
            }
        }
    }
}

class Assignment : Iinstruction//asigna el valor de la expr de la erecha al identificador de la isq ,esto modifica el valor de la carta en el diccionario,luego al actualizarse las estadisticas a partir del diccionario,el valor en el juego de la carta sera alterado.
{
    Token leftIdentifier;
    IExpr rightExpr;

    public Assignment(Token left, IExpr right)
    {
        leftIdentifier = left;
        rightExpr = right;
    }
    public void Execute()
    {
        GameData.CardStats[leftIdentifier.value] = rightExpr.Evaluate();
    }
}

class Action : Iinstruction//accion del juego ,ejecuta la accion del diccionario de acciones de tipo del delegado void Actions().
{
    Token stringAction;

    public Action(Token stringAction)
    {
        this.stringAction = stringAction;
    }
    public void Execute()
    {
        GameData.GameActions[stringAction.value]();
    }
}
class ClientEffectInstructionsAST : Iinstruction//nodo principal y raiz donde contiene uuna lista de instrucciones donde cada instruccion va a ser ejecutada y va a hacer lo q le toca,evaluandose recursivamente el metodo Evaluate() y Execute() hasta q se altere el juego de manera satisfactoria.
{
    public List<Iinstruction> instructions = new List<Iinstruction>();

    public void Execute()
    {
        for (int position = 0; position < instructions.Count; position++)
        {
            instructions[position].Execute();
        }
    }
}