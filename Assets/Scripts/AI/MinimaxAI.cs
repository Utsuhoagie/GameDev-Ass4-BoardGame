using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UType = UnitController.UType;
using Side = GameController.Side;
using State = UnitController.State;

public class MinimaxAI : MonoBehaviour
{
    public GameObject gameController;
    public GameObject combatController;

    private Unit[,] stimulateBoard;

    private Side AISide = Side.RED;
    private Side PlayerSide = Side.BLUE;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void playAITurn()
    {
        this.stimulateBoard = new Unit[8, 8];

        GameController gC = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        GameObject[,] board = gC.getBoardPos();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != null)
                {
                    UnitController uC = board[i, j].GetComponent<UnitController>();

                    stimulateBoard[i, j] = new Unit(uC.GetSide(), uC.GetState(), uC.GetUType(), uC.GetHP(), uC.GetMove(), uC.GetAtkRange(), i, j);
                }
            }
        }

        // stimulate one step of minimax
        List<Tuple<int, Move>> scores = new List<Tuple<int, Move>>();

        int alpha = int.MinValue;
        int beta = int.MaxValue;
        int depth = 2;
        int maxEval = -int.MaxValue;
        List<Move> moves = generateMoves(this.stimulateBoard, this.AISide);
        foreach (Move move in moves)
        {
            Unit[,] newBoard = this.cloneBoard(this.stimulateBoard);

            this.updateBoardWithMove(newBoard, move);

            int eval = minimax(newBoard, depth - 1, alpha, beta, false);

            // add to scores
            scores.Add(new Tuple<int, Move>(eval, move));

            maxEval = Mathf.Max(maxEval, eval);
            alpha = Mathf.Max(alpha, eval);

            if (beta <= alpha)
                break;
        }

        //TODO: fix error while scores null
        if (scores.Count == 0)
        {
            Debug.Log("AI: scores is null");
            return;
        }

        // choose the best move
        scores.Sort((x, y) => y.First.CompareTo(x.First));
        Move bestMove = scores[0].Second;

        // move the unit
        UnitController unitToMove = this.gameController.GetComponent<GameController>().getBoardPos()[bestMove.from[0], bestMove.from[1]].GetComponent<UnitController>();
        unitToMove.onAIMouseUp();
        unitToMove.clickMoveTile(bestMove.to[0], bestMove.to[1]);
        Debug.Log("AI move: " + bestMove.from[0] + ", " + bestMove.from[1] + " to " + bestMove.to[0] + ", " + bestMove.to[1]);

        // attack if needed
        if (bestMove.attackedUnit != null)
        {
            unitToMove.clickAtkTile(bestMove.attackedUnit.x, bestMove.attackedUnit.y);
            Debug.Log("AI attack: " + bestMove.attackedUnit.x + ", " + bestMove.attackedUnit.y);
        }

    }

    private int minimax(Unit[,] board, int depth, int alpha, int beta, bool isMaxAI)
    {
        if (depth == 0) // or is game over 
        {
            return evaluateBoard(board);
        }

        if (isMaxAI)
        {
            int maxEval = int.MinValue;
            List<Move> moves = generateMoves(board, this.AISide);
            foreach (Move move in moves)
            {
                Unit[,] newBoard = this.cloneBoard(board);

                this.updateBoardWithMove(newBoard, move);


                int eval = minimax(newBoard, depth - 1, alpha, beta, false);
                maxEval = Mathf.Max(maxEval, eval);
                alpha = Mathf.Max(alpha, eval);

                if (beta <= alpha)
                    break;
            }
            return maxEval;
        }
        else
        {
            int minEval = int.MaxValue;
            List<Move> moves = generateMoves(board, this.PlayerSide);
            foreach (Move move in moves)
            {
                Unit[,] newBoard = this.cloneBoard(board);

                this.updateBoardWithMove(newBoard, move);

                int eval = minimax(newBoard, depth - 1, alpha, beta, true);
                minEval = Mathf.Min(minEval, eval);
                beta = Mathf.Min(beta, eval);

                if (beta <= alpha)
                    break;
            }
            return minEval;
        }
    }

    private Unit[,] cloneBoard(Unit[,] board)
    {
        Unit[,] newBoard = new Unit[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != null)
                {
                    newBoard[i, j] = (Unit)board[i, j].clone();
                }
            }
        }
        return newBoard;
    }

    private void updateBoardWithMove(Unit[,] newBoard, Move move)
    {
        // // print board
        // for (int i = 0; i < 8; i++)
        // {
        //     string log = "";
        //     for (int j = 0; j < 8; j++)
        //     {
        //         if (newBoard[i, j] != null)
        //         {
        //             if (newBoard[i, j].side == Side.RED)
        //                 log += "R";
        //             else
        //                 log += "B";
        //         }
        //         else
        //         {
        //             log = log + "|0";
        //         }
        //     }
        //     Debug.Log(log);
        // }

        newBoard[move.to[0], move.to[1]] = newBoard[move.from[0], move.from[1]];
        if (!(move.to[0] == move.from[0] && move.to[1] == move.from[1]))
        {
            newBoard[move.from[0], move.from[1]] = null;
        }
        // Debug.Log("move: " + move.from[0] + ", " + move.from[1] + " to " + move.to[0] + ", " + move.to[1]);

        // // print board
        // for (int i = 0; i < 8; i++)
        // {
        //     string log = "";
        //     for (int j = 0; j < 8; j++)
        //     {
        //         if (newBoard[i, j] != null)
        //         {
        //             if (newBoard[i, j].side == Side.RED)
        //                 log += "R";
        //             else
        //                 log += "B";
        //         }
        //         else
        //         {
        //             log = log + "|0";
        //         }
        //     }
        //     Debug.Log(log);
        // }

        newBoard[move.to[0], move.to[1]].x = move.to[0];
        newBoard[move.to[0], move.to[1]].y = move.to[1];

        // attack if needed
        if (move.attackedUnit != null)
        {
            // UnitController attacker = new UnitController();
            // attacker.SetX(move.from[0]);
            // attacker.SetY(move.from[1]);
            // attacker.SetSide(newBoard[move.from[0], move.from[1]].side);
            // attacker.SetHP(newBoard[move.to[0], move.to[1]].HP);
            // attacker.SetRange(newBoard[move.to[0], move.to[1]].range);
            // attacker.SetMove(newBoard[move.to[0], move.to[1]].move);

            // UnitController defender = new UnitController();
            // defender.SetX(move.attackedUnit.x);
            // defender.SetY(move.attackedUnit.y);
            // defender.SetSide(newBoard[move.attackedUnit.x, move.attackedUnit.y].side);
            // defender.SetHP(newBoard[move.attackedUnit.x, move.attackedUnit.y].HP);
            // defender.SetRange(newBoard[move.attackedUnit.x, move.attackedUnit.y].range);
            // defender.SetMove(newBoard[move.attackedUnit.x, move.attackedUnit.y].move);

            // this.combatController.GetComponent<CombatController>().Attack(attacker, defender);

            // newBoard[move.to[0], move.to[1]].HP = attacker.GetHP();
            // newBoard[move.attackedUnit.x, move.attackedUnit.y].HP = defender.GetHP();

            this.combatController.GetComponent<CombatController>().attackStimulate(newBoard[move.to[0], move.to[1]], newBoard[move.attackedUnit.x, move.attackedUnit.y]);
            if (newBoard[move.attackedUnit.x, move.attackedUnit.y].HP <= 0)
            {
                newBoard[move.attackedUnit.x, move.attackedUnit.y] = null;
            }
            if (newBoard[move.to[0], move.to[1]].HP <= 0)
            {
                newBoard[move.to[0], move.to[1]] = null;
            }
        }
    }

    private int evaluateBoard(Unit[,] board)
    {
        int point = 0;

        // calculate point for each side
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != null)
                {
                    if (board[i, j].side == this.PlayerSide)
                    {
                        point -= board[i, j].HP;
                    }
                    else
                    {
                        point += board[i, j].HP;
                    }
                }
            }
        }

        return point;
    }

    private List<Move> generateMoves(Unit[,] board, Side side)
    {
        List<Move> moves = new List<Move>();

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board[i, j] != null && board[i, j].side == side && board[i, j].state != State.MOVED && board[i, j].state != State.END)
                {
                    if (board[i, j].move > 0)
                    {
                        List<int[]> moveList = generateMoveList(board, i, j);

                        foreach (int[] move in moveList)
                        {
                            Move moveObj = new Move(new int[] { i, j }, move, board[i, j], null);

                            // after move, what unit can attack ?
                            List<Unit> canAttackList = generateCanAttackList(board, moveObj);

                            // add all possible move to moves
                            moves.Add(moveObj);
                            for (int k = 0; k < canAttackList.Count; k++)
                            {
                                moves.Add(new Move(new int[] { i, j }, move, board[i, j], canAttackList[k]));
                            }
                        }
                    }
                }
            }
        }

        return moves;
    }

    private List<int[]> generateMoveList(Unit[,] board, int x, int y)
    {
        List<int[]> moveList = new List<int[]>();

        moveList.Add(new int[] { x, y });
        for (int i = 1; i <= board[x, y].move; i++)
        {
            // draw left
            if (this.isValidPos(board, x - i, y) && board[x - i, y] == null)
                moveList.Add(new int[] { x - i, y });
            // draw up
            if (this.isValidPos(board, x + i, y) && board[x + i, y] == null)
                moveList.Add(new int[] { x + i, y });
            // draw right
            if (this.isValidPos(board, x, y + i) && board[x, y + i] == null)
                moveList.Add(new int[] { x, y + i });
            // draw down
            if (this.isValidPos(board, x, y - i) && board[x, y - i] == null)
                moveList.Add(new int[] { x, y - i });

            // draw diagonal left top
            if (i >= 2 && this.isValidPos(board, x - (i - 1), y + (i - 1)) && board[x - (i - 1), y + (i - 1)] == null)
                moveList.Add(new int[] { x - (i - 1), y + (i - 1) });
            // draw diagonal right top
            if (i >= 2 && this.isValidPos(board, x + (i - 1), y + (i - 1)) && board[x + (i - 1), y + (i - 1)] == null)
                moveList.Add(new int[] { x + (i - 1), y + (i - 1) });
            // draw diagonal left bot
            if (i >= 2 && this.isValidPos(board, x - (i - 1), y - (i - 1)) && board[x - (i - 1), y - (i - 1)] == null)
                moveList.Add(new int[] { x - (i - 1), y - (i - 1) });
            // draw diagonal right bot
            if (i >= 2 && this.isValidPos(board, x + (i - 1), y - (i - 1)) && board[x + (i - 1), y - (i - 1)] == null)
                moveList.Add(new int[] { x + (i - 1), y - (i - 1) });
        }

        return moveList;
    }

    private bool isValidPos(Unit[,] board, int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return false;

        return true;
    }

    private List<Unit> generateCanAttackList(Unit[,] board, Move move)
    {
        List<Unit> canAttackList = new List<Unit>();
        int attackRange = move.moveUnit.range;
        int x = move.to[0];
        int y = move.to[1];
        Unit moveUnit = move.moveUnit;

        // draw left
        if (this.isValidPos(board, x - attackRange, y) &&
            board[x - attackRange, y] != null &&
            !moveUnit.isSameSide(board[x - attackRange, y]))
            canAttackList.Add(board[x - attackRange, y]);
        // draw up
        if (this.isValidPos(board, x + attackRange, y) &&
            board[x + attackRange, y] != null &&
            !moveUnit.isSameSide(board[x + attackRange, y]))
            canAttackList.Add(board[x + attackRange, y]);
        // draw right
        if (this.isValidPos(board, x, y + attackRange) &&
            board[x, y + attackRange] != null &&
            !moveUnit.isSameSide(board[x, y + attackRange]))
            canAttackList.Add(board[x, y + attackRange]);
        // draw down
        if (this.isValidPos(board, x, y - attackRange) &&
            board[x, y - attackRange] != null &&
            !moveUnit.isSameSide(board[x, y - attackRange]))
            canAttackList.Add(board[x, y - attackRange]);

        if (attackRange == 2)
        {
            // draw diagonal left top
            if (this.isValidPos(board, x - 1, y + 1) &&
                board[x - 1, y + 1] != null &&
                !moveUnit.isSameSide(board[x - 1, y + 1]))
                canAttackList.Add(board[x - 1, y + 1]);
            // draw diagonal right top
            if (this.isValidPos(board, x + 1, y + 1) &&
                board[x + 1, y + 1] != null &&
                !moveUnit.isSameSide(board[x + 1, y + 1]))
                canAttackList.Add(board[x + 1, y + 1]);
            // draw diagonal left bot
            if (this.isValidPos(board, x - 1, y - 1) &&
                board[x - 1, y - 1] != null &&
                !moveUnit.isSameSide(board[x - 1, y - 1]))
                canAttackList.Add(board[x - 1, y - 1]);
            // draw diagonal right bot
            if (this.isValidPos(board, x + 1, y - 1) &&
                board[x + 1, y - 1] != null &&
                !moveUnit.isSameSide(board[x + 1, y - 1]))
                canAttackList.Add(board[x + 1, y - 1]);
        }

        return canAttackList;
    }

    public class Unit
    {
        public Side side;
        public State state;
        public UType unitType;
        public int HP;
        public int move;
        public int range;
        public int x;
        public int y;

        public Unit(Side side, State state, UType unitType, int HP, int move, int range, int x, int y)
        {
            this.side = side;
            this.state = state;
            this.unitType = unitType;
            this.HP = HP;
            this.move = move;
            this.range = range;
            this.x = x;
            this.y = y;
        }

        public bool isSameSide(Unit other)
        {
            return this.side == other.side;
        }

        public object clone()
        {
            return this.MemberwiseClone();
        }

        // public bool Equals(Unit other)
        // {
        //     return side == other.side && unitType == other.unitType && HP == other.HP && move == other.move && range == other.range && x == other.x && y == other.y;
        // }

        // public static bool operator ==(Unit lhs, Unit rhs)
        // {
        //     return lhs.Equals(rhs);
        // }

        // public static bool operator !=(Unit lhs, Unit rhs)
        // {
        //     return !lhs.Equals(rhs);
        // }
    }
    struct Move
    {
        public int[] from;
        public int[] to;
        public Unit moveUnit;

        public Unit attackedUnit;

        public Move(int[] from, int[] to, Unit moveUnit, Unit attackedUnit)
        {
            this.from = from;
            this.to = to;
            this.moveUnit = moveUnit;
            this.attackedUnit = attackedUnit;
        }

    }
}

public class Tuple<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Tuple(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}

public static class Tuple
{
    public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
    {
        var tuple = new Tuple<T1, T2>(first, second);
        return tuple;
    }
}