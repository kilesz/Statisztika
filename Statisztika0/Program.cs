using Statisztika1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Statisztika1 {
class DancingLinksSudokuSolver
{
    private int[,] board;
    private int size = 9;
    private int boxSize = 3;

    public DancingLinksSudokuSolver(int[,] board)
    {
        this.board = (int[,])board.Clone();
    }

    public bool Solve()
    {
        return SolveSudoku(0, 0);
    }

    private bool SolveSudoku(int row, int col)
    {
        if (row == size)
            return true;

        if (col == size)
            return SolveSudoku(row + 1, 0);

        if (board[row, col] != 0)
            return SolveSudoku(row, col + 1);

        for (int num = 1; num <= size; num++)
        {
            if (IsValidMove(row, col, num))
            {
                board[row, col] = num;
                if (SolveSudoku(row, col + 1))
                    return true;
                board[row, col] = 0;
            }
        }
        return false;
    }

    private bool IsValidMove(int row, int col, int num)
    {
        for (int i = 0; i < size; i++)
        {
            if (board[row, i] == num || board[i, col] == num)
                return false;
        }

        int startRow = (row / boxSize) * boxSize;
        int startCol = (col / boxSize) * boxSize;
        for (int i = startRow; i < startRow + boxSize; i++)
        {
            for (int j = startCol; j < startCol + boxSize; j++)
            {
                if (board[i, j] == num)
                    return false;
            }
        }
        return true;
    }

    public int[,] GetSolvedBoard()
    {
        return board;
    }
}
class Sudoku
{
    private int[,] board;

    public Sudoku()
    {
        board = new int[9, 9];
    }
    public Sudoku(int[,] initialBoard)
    {
        board = initialBoard;
    }

    public int[,] GetBoard()
    {
        return board;
    }

    public void DisplayBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            if (i % 3 == 0 && i != 0)
            {
                Console.WriteLine("---------------------------------");
            }
            for (int j = 0; j < 9; j++)
            {
                Console.Write("|" + (board[i, j] == 0 ? "." : board[i, j].ToString()) + "|");
                if ((j == 2 || j == 5) && j != 0)
                {
                    Console.Write(" | ");
                }
            }
            Console.WriteLine();
        }
    }

    public bool AddNumber(int row, int col, int num)
    {
        if (row < 0 || row >= 9 || col < 0 || col >= 9 || num < 1 || num > 9)
        {
            return false;
        }
        if (!IsValidMove(row, col, num))
        {
            return false;
        }

        board[row, col] = num;
        return true;
    }

    private bool IsValidMove(int row, int col, int num)
    {
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == num || board[i, col] == num)
                return false;
        }

        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;
        for (int i = startRow; i < startRow + 3; i++)
        {
            for (int j = startCol; j < startCol + 3; j++)
            {
                if (board[i, j] == num)
                    return false;
            }
        }

        return true;


    }

    public bool IsCompleted()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (board[i, j] == 0)
                    return false;
            }
        }
        return true;
    }

    public bool GenerateSudoku()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == 0)
                {
                    List<int> numbers = Enumerable.Range(1, 9).OrderBy(n => new Random().Next()).ToList();

                    foreach (int num in numbers)
                    {
                        if (IsValidMove(row, col, num))
                        {
                            board[row, col] = num;

                            if (GenerateSudoku())
                            {
                                return true;
                            }

                            board[row, col] = 0;
                        }
                    }

                    return false;
                }
            }
        }

        return true;
    }
    public bool SolveWithBacktracking()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == 0)
                {
                    for (int num = 1; num <= 9; num++)
                    {
                        if (IsValidMove(row, col, num))
                        {
                            board[row, col] = num;

                            if (SolveWithBacktracking())
                                return true;

                            board[row, col] = 0;
                        }
                    }
                    return false;
                }
            }
        }
        return true;
    }

    public void GenerateDifficulty(int diff)
    {
        Random rnd = new Random();
        int count = 0;

        while (count < diff)
        {
            int row = rnd.Next(0, 9);
            int col = rnd.Next(0, 9);

            if (board[row, col] != 0)
            {
                board[row, col] = 0;
                count++;
            }
        }
    }

    public void SolveWithConstraints()
    {
        while (ApplyConstraints()) { }

        if (!IsCompleted())
        {
            Console.WriteLine("A Sudoku nem oldható meg csak kényszerszórással.");
        }
    }

    private bool ApplyConstraints()
    {
        bool changed = false;
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (board[row, col] == 0)
                {
                    var possibilities = GetPossibleValues(row, col);
                    if (possibilities.Count == 1)
                    {
                        board[row, col] = possibilities.First();
                        changed = true;
                    }
                }
            }
        }
        return changed;
    }

    private HashSet<int> GetPossibleValues(int row, int col)
    {
        HashSet<int> possible = new HashSet<int>(Enumerable.Range(1, 9));

        for (int i = 0; i < 9; i++)
        {
            possible.Remove(board[row, i]);
            possible.Remove(board[i, col]);
        }

        int startRow = (row / 3) * 3;
        int startCol = (col / 3) * 3;
        for (int i = startRow; i < startRow + 3; i++)
        {
            for (int j = startCol; j < startCol + 3; j++)
            {
                possible.Remove(board[i, j]);
            }
        }

        return possible;
    }
}


class Program
{
    static void Main(string[] args)
    {

        Sudoku sudoku = new Sudoku();
        bool playing = true;
        List<SolutionTime> solutionTimes = new List<SolutionTime>();

        while (playing)
        {

            Console.Clear();
            sudoku.DisplayBoard();

            Console.WriteLine("\nVálassz egy opciót:");
            Console.WriteLine("1 - Szám beírása");
            Console.WriteLine("2 - Constraint propagation (Kényszerszórás) Megoldás");
            Console.WriteLine("3 - Dancing Links (DLX) Megoldás");
            Console.WriteLine("4 - Visszalépéses (Backtracking) Megoldás");
            Console.WriteLine("5 - Kilépés");

            int choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                Console.WriteLine("Add meg a nehézséget (1-80): ");
                int diff = Convert.ToInt32(Console.ReadLine());
                sudoku.GenerateSudoku();          
                sudoku.GenerateDifficulty(diff);
                Stopwatch stopwatchH = Stopwatch.StartNew();
                while (!sudoku.IsCompleted())
                {
                    Console.Clear();
                    sudoku.DisplayBoard();

                    Console.WriteLine("Add meg a sor számát (0-8): ");
                    int row = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Add meg az oszlop számát (0-8): ");
                    int col = Convert.ToInt32(Console.ReadLine());

                    Console.WriteLine("Add meg a számot (1-9): ");
                    int num = Convert.ToInt32(Console.ReadLine());

                    if (!sudoku.AddNumber(row, col, num))
                    {
                        Console.WriteLine("Érvénytelen lépés! Próbáld újra.");
                    }

                    if (sudoku.IsCompleted())
                    {
                        Console.Clear();
                        sudoku.DisplayBoard();
                        Console.WriteLine("Gratulálok, megnyerted a játékot!");
                        stopwatchH.Stop();
                        Console.WriteLine("Emberi megoldással, ideje a következő:"+stopwatchH.Elapsed);
                        playing = false;
                    }
                }

            }
            else if (choice == 2)
            {
                Console.WriteLine("Hanyszor akarod lefuttatni a kényszerszórásos megoldást?");
                int db = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Add meg a nehézséget (1-80): ");
                int diff = Convert.ToInt32(Console.ReadLine());
                for(int i = 0; i < db; i++)
                {
                    sudoku.GenerateSudoku();          
                    sudoku.GenerateDifficulty(diff);
                Stopwatch stopwatchC = Stopwatch.StartNew();
                sudoku.SolveWithConstraints();
                sudoku.DisplayBoard();
                stopwatchC.Stop();
                Console.WriteLine("\nA kényszerszórás ideje a következő:"+stopwatchC.Elapsed);
                solutionTimes.Add(new SolutionTime("Constraint Propagation", stopwatchC.Elapsed));
                }
                playing = false;
                }
            else if (choice == 3)
            {
                Console.WriteLine("Hanyszor akarod lefuttatni a DancingLinks megoldást?");
                int db = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Add meg a nehézséget (1-80): ");
                int diff = Convert.ToInt32(Console.ReadLine());
                for(int i = 0; i < db; i++)
                {
                sudoku.GenerateSudoku();          
                sudoku.GenerateDifficulty(diff);
                Stopwatch stopwatchDL = Stopwatch.StartNew();
                DancingLinksSudokuSolver solver = new DancingLinksSudokuSolver(sudoku.GetBoard());
                if (solver.Solve())
                {
                    
                    Sudoku solvedSudoku = new Sudoku(solver.GetSolvedBoard());
                    solvedSudoku.DisplayBoard();
                    stopwatchDL.Stop();
                    Console.WriteLine("A DLX ideje a következő:"+stopwatchDL.Elapsed);
                    solutionTimes.Add(new SolutionTime("DancingLinks", stopwatchDL.Elapsed));
                    }
                else
                {
                    Console.WriteLine("\nNem sikerült megoldani a Sudokut DLX-szel.");
                }
                }
                playing = false;
            }
            else if (choice == 4)
            {
                Console.WriteLine("Hanyszor akarod lefuttatni a visszalépéses megoldást?");
                int db = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Add meg a nehézséget (1-80): ");
                int diff = Convert.ToInt32(Console.ReadLine());
                for(int i = 0; i < db; i++)
                {
                    sudoku.GenerateSudoku();          
                    sudoku.GenerateDifficulty(diff);

                Stopwatch stopwatchBT = Stopwatch.StartNew();
                if (sudoku.SolveWithBacktracking())
                {
                    sudoku.DisplayBoard();
                    stopwatchBT.Stop();
                    Console.WriteLine("A BackTracking ideje a következő:"+stopwatchBT.Elapsed);
                    solutionTimes.Add(new SolutionTime("Backtracking", stopwatchBT.Elapsed));
                    }
                else
                {
                    Console.WriteLine("Nem található megoldás.");
                }
                }
                playing = false;
            }
            else if (choice == 5)
            {
                playing = false;
            }
            
        }
        // Calculate and display average times
        var groupedTimes = solutionTimes.GroupBy(st => st.Method);
        foreach (var group in groupedTimes)
        {
            var averageTime = new TimeSpan((long)group.Average(st => st.Time.Ticks));
            Console.WriteLine($"Átlagos idő a {group.Key} módszerrel: {averageTime}");
        }
    }
}
class SolutionTime
{
    public string Method { get; set; }
    public TimeSpan Time { get; set; }

    public SolutionTime(string method, TimeSpan time)
    {
        Method = method;
        Time = time;
    }
}
}
