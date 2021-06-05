using UnityEngine;
using UnityEngine.UI;

public class MakrukKhon : BasePiece
{

    // Valor assignat per matar aquesta peça
    new public int price = 3;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject newSoundMove, GameObject newSoundDead)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, newSoundMove, newSoundDead);

        // Moviment de l'alfil
        mMovement = new Vector3Int(0, 0, 1);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("M_Piece");
    }

    // Funció per comprovar el path de la peça
    protected override void CheckPathing()
    {
        // Posició actual
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // Diagonals
        getCellState(currentX + 1, currentY + 1);
        getCellState(currentX + 1, currentY - 1);
        getCellState(currentX - 1, currentY + 1);
        getCellState(currentX - 1, currentY - 1);

        // Cel·la de davant
        int yMov = mColor == Color.white ? 1 : -1;
        getCellState(currentX, currentY + yMov);
    }

    // Agafa l'estat de la cel·la
    private bool getCellState(int targetX, int targetY)
    {
        // Es comprova l'estat de la cel·la
        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.StateCell(targetX, targetY, this);

        //Si es compleix que l'estat de la cel·la és l'esperat, el moviment és possible
        if (cellState == CellState.Free || cellState == CellState.Enemy)
        {
            mPossiblePathCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);

            // S'evalua si la casella es bona o no per fer un atac al enemic
            if (nonPlayerTurnOn)
            {
                if (cellState == CellState.Enemy)
                {
                    mCurrentCell.mBoard.mAllCells[targetX, targetY].score = 100 + mCurrentCell.mCurrentPiece.price * 10 + 7;
                }
                else
                {
                    mCurrentCell.mBoard.mAllCells[targetX, targetY].score = 0;
                }
            }
            return true;
        }

        return false;
    }
}
