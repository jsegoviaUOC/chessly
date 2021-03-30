using UnityEngine;
using UnityEngine.UI;

public class Tower : BasePiece
{
    // Definició de les cel·les per gestionar l'enderroc (castling en angles)
    public Cell mCastlingTriggerCell = null;
    private Cell mCastlingCell = null;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviment
        mMovement = new Vector3Int(7, 7, 0);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Piece");
    }

    // funció per recol·locar la torre un cop fet l'enderroc
    public override void Place(Cell newCell)
    {
        // Nova posició
        base.Place(newCell);

        // Cel·la del trigger
        int triggerOffset = mCurrentCell.mBoardPosition.x < 4 ? 2 : -1;
        mCastlingTriggerCell = SetCell(triggerOffset);

        // Cel·la del enderroc
        int castlingOffset = mCurrentCell.mBoardPosition.x < 4 ? 3 : -2;
        mCastlingCell = SetCell(castlingOffset);
    }

    // Funció del enderroc
    public void Castling()
    {
        // Cel·la objectiu de l'enderroc
        mTargetCell = mCastlingCell;

        // Moviment de la torre
        Move();
    }

    private Cell SetCell(int offset)
    {
        // Nova posició
        Vector2Int newPosition = mCurrentCell.mBoardPosition;
        newPosition.x += offset;

        // Return de la nova posició de la peça
        return mCurrentCell.mBoard.mAllCells[newPosition.x, newPosition.y];
    }
}
