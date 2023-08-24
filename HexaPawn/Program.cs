using System.Numerics;

bool player = true;
bool winState = false;
bool CharacterLosing = false;

Dictionary<string, string> board = new Dictionary<string, string>();

board.Add("0", " ");
board.Add("1", "A");
board.Add("2", "B");
board.Add("3", "C");
board.Add("4", "1");
board.Add("A1", "p");
board.Add("B1", "p");
board.Add("C1", "p");
board.Add("6", "2");
board.Add("A2", ".");
board.Add("B2", ".");
board.Add("C2", ".");
board.Add("7", "3");
board.Add("A3", "c");
board.Add("B3", "c");
board.Add("C3", "c");

void DrawBoard()
{
    int constructBoard = 1;
    for (int i = 0; i <= 15; i++)
    {
        Console.Write("  " + board.ElementAt(i).Value + "  ");
        if (constructBoard % 4 == 0)
        {
            Console.WriteLine();
            constructBoard = 0;
        }
        constructBoard++;
    }
}

bool CheckValidMove(bool player, string piece, string moveInput, string toInput)
{
    board.TryGetValue(toInput, out string field);

    int placement = board.Keys.ToList().IndexOf(moveInput);
    int placementTo = board.Keys.ToList().IndexOf(toInput);

    if (player == true && piece == "p" && placementTo >= placement + 3 && placementTo <= placement + 5)
    {
        if (field == "c" && placementTo != placement + 4 || placementTo == placement + 4 && field == ".")
        {
            return true;
        }
    }
    else if (player == false && piece == "c" && placementTo >= placement - 5 && placementTo <= placement - 3)
    {
        if (field == "p" && placementTo != placement - 4 || placementTo == placement - 4 && field == ".")
        {
            return true;
        }
    }
    return false;
}

Alica AiCheckValidMove()
{
    for (int i = 0; i <= 15; i++)
    {
        board.TryGetValue(toInput, out string field);

        int placement = board.Keys.ToList().IndexOf(moveInput);
        int placementTo = board.Keys.ToList().IndexOf(toInput);

        if (piece == "c" && placementTo >= placement - 5 && placementTo <= placement - 3)
        {
            if (field == "p" && placementTo != placement - 4 || placementTo == placement - 4 && field == ".")
            {
                return true;
            }
        }
        return false;
    }
}

bool MovePiece()
{
    Console.WriteLine("Move piece");
    string moveInput = Console.ReadLine().ToUpper();
    Console.WriteLine("To where?");
    string toInput = Console.ReadLine().ToUpper();

    board.TryGetValue(moveInput, out string piece);
    board.TryGetValue(toInput, out string field);

    if (CheckValidMove(player, piece, moveInput, toInput))
    {
        if (field == "c" || field == "p")
        {
            board[moveInput] = ".";
            board[toInput] = piece;
        }
        else
        {
            board[moveInput] = field;
            board[toInput] = piece;
        }
    }
    else
    {
        Console.WriteLine("Invalid");
        return false;
    }
    return true;
}

bool AliciaMovePiece()
{
    Console.WriteLine("Move piece");
    string moveInput = Console.ReadLine().ToUpper();
    Console.WriteLine("To where?");
    string toInput = Console.ReadLine().ToUpper();

    board.TryGetValue(moveInput, out string piece);
    board.TryGetValue(toInput, out string field);

    if (CheckValidMove(player, piece, moveInput, toInput))
    {
        if (field == "c" || field == "p")
        {
            board[moveInput] = ".";
            board[toInput] = piece;
        }
        else
        {
            board[moveInput] = field;
            board[toInput] = piece;
        }
    }
    else
    {
        Console.WriteLine("Invalid");
        return false;
    }
    return true;
}

while (!winState)
{
    Console.Clear();
    CharacterLosing = true;
    string playerPiece = "";
    if(player == true)
    {
        playerPiece = "p";
        Console.WriteLine("Player Turn");
    }
    else if(player == false)
    {
        playerPiece = "c";
        Console.WriteLine("Computer Turn");
    }
    foreach (var piece in board)
        if (piece.Value == playerPiece)
        {
            for (int i = 0; i <= 15; i++)
            {
                if (CheckValidMove(player, playerPiece, piece.Key, board.ElementAt(i).Key))
                {
                    CharacterLosing = false;
                }
            }
        }
    DrawBoard();
    if (CharacterLosing && player == false)
    {
        Console.WriteLine("Player wins");
        break;
    }
    else if (CharacterLosing && player == true)
    {
        Console.WriteLine("Computer turn");
        break;
    }
    bool movedPiece = false;
    while (!movedPiece) 
    {
        movedPiece = MovePiece();
    }
    player = !player;
    for (int i = 5; i < 7; i++)
    {
        if (board.ElementAt(i).Value == "c")
        {
            Console.WriteLine("Computer win");
            winState = true;
            break;
        }
    }
    for (int i = 13; i < 15; i++) 
    { 
        if (board.ElementAt(i).Value == "p")
        {
            Console.WriteLine("Player win");
            winState = true;
            break;
        }
    }
}