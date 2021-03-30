using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviment. En funció del color és un o l'altre doncs el peó no pot retrocedir
        mMovement = mColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("P_Piece");
    }

    // Funció per moure el peó
    // El peó pot promoures a una altra peça si es compleixen certes condicions
    protected override void Move()
    {
        base.Move();

        // Comprovació de la promoció del peó
        CheckForPromotion();
    }

    // Agafa l'estat de la cel·la
    private bool getCellState(int targetX, int targetY, CellState targetState)
    {
        // Es comprova l'estat de la cel·la
        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.StateCell(targetX, targetY, this);

        //Si es compleix que l'estat de la cel·la és l'esperat, el moviment és possible
        if (cellState == targetState)
        {
            mPossiblePathCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }

        return false;
    }

    // Funció per comprovar a la promoció de la peça
    private void CheckForPromotion()
    {
        // Posició actual
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // S'agafa l'estat dela cel·la de davant del peó
        CellState cellState = mCurrentCell.mBoard.StateCell(currentX, currentY + mMovement.y, this);

        // Si la cel·la del davant del peó está fora dels marges del tauler, el peó pot promocionar-se
        if (cellState == CellState.OutOfBounds)
        {
            Color spriteColor = GetComponent<Image>().color;

            // Es promociona la peça
            mPieceManager.PromotePiece(this, mCurrentCell, mColor, spriteColor);
        }
    }

    // Funció per comprovar el path de la peça
    // El peó només pot avançar. Si és el primer moviment pot moure dos cel·les. I pot moure en diagonal una cel3la si té un enemic
    protected override void CheckPathing()
    {
        // Posició actual
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // Comprovació dels enemics en diagonal
        getCellState(currentX - mMovement.z, currentY + mMovement.z, CellState.Enemy);
        getCellState(currentX + mMovement.z, currentY + mMovement.z, CellState.Enemy);

        // Cel·la de davant
        if (getCellState(currentX, currentY + mMovement.y, CellState.Free))
        {
            // Si és el primer moviment, es comprova la segona cel·la
            if (mIsFirstMove)
            {
                getCellState(currentX, currentY + (mMovement.y * 2), CellState.Free);
            }
        }
    }

}
