using ChessChallenge.API;

public class SimpleAlphaBeta : IChessBot
{
    // Piece values: null, pawn, knight, bishop, rook, queen
    int[] PieceValues = { 0, 100, 300, 300, 500, 900 };
    int Checkmate = 9999;
    int Depth = 6;

    public Move Think(Board board, Timer timer)
    {
        Move bestMove = default;
        int alpha = -10000;
        int beta = 10000;
        foreach (Move move in board.GetLegalMoves())
        {
            board.MakeMove(move);
            int score = -NegaAlphaBeta(-beta, -alpha, 1, board);
            board.UndoMove(move);

            if (score > alpha)
            {
                bestMove = move;
                alpha = score;
            }
        }
        return bestMove;
    }

    private int NegaAlphaBeta(int alpha, int beta, int depth, Board board)
    {
        if (depth == Depth)
            return Eval(board);

        if (board.IsDraw())
            return 0;

        if (board.IsInCheckmate())
            return -Checkmate;

        foreach (Move move in board.GetLegalMoves())
        {
            board.MakeMove(move);
            int score = -NegaAlphaBeta(-beta, -alpha, depth + 1, board);
            board.UndoMove(move);

            if (score > alpha)
            {
                alpha = score;
                if (score >= beta)
                    return beta;
            }
        }
        return alpha;
    }

    private int Eval(Board board)
    {
        int eval = 0;
        for (int pieceType = 1; pieceType < 5; pieceType++)
        {
            var white = board.GetPieceList((PieceType)pieceType, true);
            var black = board.GetPieceList((PieceType)pieceType, false);
            eval += (white.Count - black.Count) * PieceValues[pieceType];
        }
        return board.IsWhiteToMove ? eval : -eval;
    }
}