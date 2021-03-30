﻿using UnityEngine;
using UnityEngine.UI;

public class Knight : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("KN_Piece");
    }

    // Creació del path de la peça
    private void CreatePath(int flipper)
    {
        // Es guarda la posició actual en variable auxiliars
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // Esquerra
        getCellState(currentX - 2, currentY + (1 * flipper));

        // A d'alt a l'esquerra
        getCellState(currentX - 1, currentY + (2 * flipper));

        // A d'alt a la dreta
        getCellState(currentX + 1, currentY + (2 * flipper));

        // Dreta
        getCellState(currentX + 2, currentY + (1 * flipper));
    }

    // Funció per comprovar el path de la peça
    // Degut als moviment no simetrics del cavall, la funció s'executa en dues parts
    protected override void CheckPathing()
    {
        // Meitat dels moviments
        CreatePath(1);

        // L'altra meitat dels moviments
        CreatePath(-1);
    }

    // Agafa l'estat de la cel·la
    private void getCellState(int targetX, int targetY)
    {
        // Es comprova l'estat de la cel·la
        CellState cellState = CellState.None;
        cellState = mCurrentCell.mBoard.StateCell(targetX, targetY, this);

        // Si hi ha un enemic o està lliure
        if (cellState == CellState.Enemy || cellState == CellState.Free)

            // S'afegeix la cel·la a les possible opcions de moviment de a peça
            mPossiblePathCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
    }
}
