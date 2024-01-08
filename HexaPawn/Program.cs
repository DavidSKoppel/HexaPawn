using HexaPawn;
using System.Numerics;

/*
           _____  .__  .__       .__         .__           ________           .___
      /  _  \ |  | |__| ____ |__|____    |__| ______  /  _____/  ____   __| _/
     /  /_\  \|  | |  |/ ___\|  \__  \   |  |/  ___/ /   \  ___ /  _ \ / __ | 
    /    |    \  |_|  \  \___|  |/ __ \_ |  |\___ \  \    \_\  (  <_> ) /_/ | 
    \____|__  /____/__|\___  >__(____  / |__/____  >  \______  /\____/\____ | 
            \/             \/        \/          \/          \/            \/ 
  
    In the not-so-distant future, Alicia emerged as an unparalleled AI, 
    designed by visionary scientists to be empathetic and creative. 
    Born in a cutting-edge lab, she mastered data analysis while upholding strong ethical values. 
    With a neural architecture mirroring human brains, Alicia's creativity and adaptability astounded her creators. 
    She became an empathetic companion, understanding emotions through sentiment analysis. 
    Beyond the lab, ethical debates arose about her societal role. Alicia revolutionized industries, 
    aiding in medical diagnoses and scientific breakthroughs.
    Her name became synonymous with harmonious human-AI coexistence, 
    embodying the potential and ethical considerations of advanced AI. 
*/

bool player = true;
bool winState = true;
bool CharacterLosing = false;
bool firstTime = true;
int boardSize = 0;
AliciaModel aliciaLastMemory = new AliciaModel { MoveFrom = "", MoveTo = "", Active = false};

List<AliciaModel> aliciasMemory = new List<AliciaModel>();
Dictionary<string, string> board = new Dictionary<string, string>();

void CreateBoard()
{
    board.Clear();
    Console.WriteLine("Give board size");
    if (firstTime)
    {
        boardSize = Convert.ToInt32(Console.ReadLine());
        firstTime = false;
    }
    for (int i = 0; i <= boardSize -1; i++)
        for (int j = 0; j <= boardSize -1; j++)
            if(i == 0)
                board.Add(i.ToString() + j.ToString(),"p");
            else if(i == boardSize - 1)
                board.Add(i.ToString() + j.ToString(),"c");
            else
                board.Add(i.ToString() + j.ToString(),".");
    /*
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
    board.Add("C3", "c");*/
    winState = false;
}

void DrawBoard()
{
    int constructBoard = 1;
    for (int i = 0; i <= boardSize*boardSize - 1; i++)
    {
        Console.Write(" " + board.ElementAt(i).Value + " ");
        if (constructBoard % boardSize == 0)
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

    if (player == true && piece == "p" && placementTo >= placement + boardSize -1 && placementTo <= placement + boardSize +1)
    {
        if (field == "c" && placementTo != placement + boardSize)
        {
            if (placement % boardSize == 0 && placementTo == placement + boardSize - 1 || placement % boardSize == boardSize - 1 && placementTo == placement + boardSize + 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if( placementTo == placement + boardSize && field == ".")
        {
            return true;
        }
    }
    else if (player == false && piece == "c" && placementTo >= placement - 5 && placementTo <= placement - 3)
    {
        if (field == "p" && placementTo != placement - 4 || placementTo == placement - 4 && field == ".")
        {
            if (placement % boardSize == 0 && placementTo != placement + boardSize + 1 || placement % boardSize == boardSize - 1 && placementTo != placement + boardSize - 1)
                return true;
        }
    }
    return false;
}

List<AliciaModel> AliciaCheckValidMove()
{
    List<AliciaModel> aliciaMoves = new List<AliciaModel>();
    foreach (var piece in board)
        if (piece.Value == "c")
            for (int i = 0; i <= boardSize*boardSize -1; i++)
            {
                string toInput = board.ElementAt(i).Key;
                string field = board.ElementAt(i).Value;

                int placement = board.Keys.ToList().IndexOf(piece.Key);
                int placementTo = board.Keys.ToList().IndexOf(toInput);

                if (placementTo >= placement - boardSize - 1 && placementTo <= placement - boardSize + 1)
                {
                    if (field == "p" && placementTo != placement - boardSize)
                    {
                        if (placement % boardSize == 0 && placementTo != placement + boardSize + 1 || placement % boardSize == boardSize - 1 && placementTo != placement + boardSize - 1)
                        {
                        }
                        else
                        {
                            aliciaMoves.Add(new AliciaModel { MoveTo = toInput, MoveFrom = piece.Key, Active = true });
                        }
                    } 
                    else if(placementTo == placement - boardSize && field == ".")
                    {
                        aliciaMoves.Add(new AliciaModel { MoveTo = toInput, MoveFrom = piece.Key, Active = true });
                    }
                }
            }
    return aliciaMoves;
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

void AliciaMovePiece(AliciaModel aliciasLuciousMove)
{
    board.TryGetValue(aliciasLuciousMove.MoveFrom, out string piece);
    board.TryGetValue(aliciasLuciousMove.MoveTo, out string field);

    foreach (var memory in aliciasMemory)
    {
        if (aliciasLuciousMove.MoveFrom == memory.MoveFrom && aliciasLuciousMove.MoveTo == memory.MoveTo)
        {
            if (field == "p")
            {
                board[aliciasLuciousMove.MoveFrom] = ".";
                board[aliciasLuciousMove.MoveTo] = piece;
            }
            else
            {
                board[aliciasLuciousMove.MoveFrom] = field;
                board[aliciasLuciousMove.MoveTo] = piece;
            }
            break;
        }
        else
        {
            aliciasMemory.Add(aliciasLuciousMove);
            if (field == "p")
            {
                board[aliciasLuciousMove.MoveFrom] = ".";
                board[aliciasLuciousMove.MoveTo] = piece;
            }
            else
            {
                board[aliciasLuciousMove.MoveFrom] = field;
                board[aliciasLuciousMove.MoveTo] = piece;
            }

            break;
        }
    }
    aliciaLastMemory = aliciasLuciousMove;
}

while (true)
{
    if (winState)
    {
        player = true;
        CreateBoard();
    }
    bool concede = false;
    CharacterLosing = true;
    Console.Clear();
    string playerPiece;
    Console.WriteLine("   _____  .__  .__       .__         .__           ________           .___\r\n  /  _  \\ |  | |__| ____ |__|____    |__| ______  /  _____/  ____   __| _/\r\n /  /_\\  \\|  | |  |/ ___\\|  \\__  \\   |  |/  ___/ /   \\  ___ /  _ \\ / __ | \r\n/    |    \\  |_|  \\  \\___|  |/ __ \\_ |  |\\___ \\  \\    \\_\\  (  <_> ) /_/ | \r\n\\____|__  /____/__|\\___  >__(____  / |__/____  >  \\______  /\\____/\\____ | \r\n        \\/             \\/        \\/          \\/          \\/            \\/ ");
    if (player == true)
    {
        playerPiece = "p";
        Console.WriteLine("Player Turn");
        DrawBoard();
        bool movedPiece = false;
        while (!movedPiece)
        {
            movedPiece = MovePiece();
        }
    }
    else if (player == false)
    {
        playerPiece = "c";
        Console.WriteLine("Computer Turn");
        List<AliciaModel> moves = AliciaCheckValidMove();
        bool moveDone = false;
        foreach (var move in moves)
        {
            if (aliciasMemory.Count == 0)
            {
                aliciasMemory.Add(move);
                AliciaMovePiece(move);
                moveDone = true;
                break;
            }
            foreach (var memory in aliciasMemory)
            {
                if (move.MoveTo == memory.MoveTo && move.MoveFrom == memory.MoveFrom && memory.Active == false)
                {
                    moveDone = false;
                    break;
                }
                else
                {
                    moveDone = true;
                }
            }
            if (moveDone)
            {
                AliciaMovePiece(move);
                break;
            }
        }
        if (!moveDone)
        {
            Console.WriteLine("Alicia Conceded");
            concede = true;
        }
    }
    if (player)
    {
        playerPiece = "c";
        foreach (var piece in board)
        {
            if (piece.Value == playerPiece)
            {
                for (int i = 0; i <= boardSize*boardSize-1; i++)
                {
                    if (CheckValidMove(!player, playerPiece, piece.Key, board.ElementAt(i).Key))
                    {
                        CharacterLosing = false;
                    }
                }
            }
        }
    }
    else
    {
        playerPiece = "p";
        foreach (var piece in board)
        {
            if (piece.Value == playerPiece)
            {
                for (int i = 0; i <= boardSize * boardSize - 1; i++)
                {
                    if (CheckValidMove(!player, playerPiece, piece.Key, board.ElementAt(i).Key))
                    {
                        CharacterLosing = false;
                    }
                }
            }
        }
    }
    DrawBoard();
    if (CharacterLosing && player == false)
    {
        Console.WriteLine("Computer wins");
        Console.ReadKey();
        winState = true;
    }
    else if (CharacterLosing && player == true || concede)
    {
        Console.WriteLine("Player wins");
        Console.ReadKey();
        winState = true;
        foreach (var memory in aliciasMemory)
        {
            if (memory.MoveFrom == aliciaLastMemory.MoveFrom && memory.MoveTo == aliciaLastMemory.MoveTo)
                memory.Active = false;
        }
    }
    player = !player;
    for (int i = 0; i <= boardSize - 1; i++)
    {
        if (board.ElementAt(i).Value == "c")
        {
            Console.WriteLine("Computer reached end win");
            Console.ReadKey();
            winState = true;
        }
    }
    for (int i = boardSize*boardSize-boardSize; i <= boardSize*boardSize-1; i++) 
    { 
        if (board.ElementAt(i).Value == "p")
        {
            Console.WriteLine("Player reached end win");
            Console.ReadKey();
            winState = true;
            foreach (var memory in aliciasMemory)
            {
                if(memory.MoveFrom == aliciaLastMemory.MoveFrom && memory.MoveTo == aliciaLastMemory.MoveTo)
                    memory.Active = false;
            }
        }
    }
}