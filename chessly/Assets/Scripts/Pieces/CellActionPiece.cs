using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class CellActionPiece : EventTrigger
{ 
    // col·locació inicial i actual
    protected CellEditor mOriginalCell = null;
    protected CellEditor mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceEditorManager mPieceEditorManager;
    
    // Inicialització de la peça
    public virtual void Setup( Color32 newSpriteColor, PieceEditorManager newPieceEditorManager)
    {
        // Gestor de peces
        mPieceEditorManager = newPieceEditorManager;

        // Color i sprite associat
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    // Lloc de la peça
    public virtual void Place(CellEditor newCell)
    {
        // Iniciaització de la cel·la inicial i actual
        mCurrentCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        // Transform de l'objecte
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        ChangeCell();
    }

    public void ChangeCell()
    {
        bool samePiece = false;
        bool sameColor = false;

        if (mCurrentCell.mCurrentPiece.GetComponent<Image>().sprite == Resources.Load<Sprite>(EditorManager.selectedPiece+"_Piece"))
        {
            samePiece = true;
        }

        if ( (EditorManager.selectedColor == "W" && mCurrentCell.mCurrentPiece.GetComponent<Image>().color == GameButton.getColor(EditorManager.optionsData.colors.whitePiecesColor) ) ||
             (EditorManager.selectedColor == "B" && mCurrentCell.mCurrentPiece.GetComponent<Image>().color == GameButton.getColor(EditorManager.optionsData.colors.blackPiecesColor) ) )
        {
            sameColor = true;
        }

        if(samePiece && sameColor)
        {
            mCurrentCell.mCurrentPiece.GetComponent<Image>().sprite = Resources.Load<Sprite>("Void");
            mCurrentCell.mCurrentPiece.GetComponent<Image>().color = new Color32(255, 255, 255, 0);

            EditorManager.matrixPiecesEditor[mCurrentCell.mBoardPosition.x, mCurrentCell.mBoardPosition.y] = null;
        }
        else
        {
            mCurrentCell.mCurrentPiece.GetComponent<Image>().sprite = Resources.Load<Sprite>(EditorManager.selectedPiece + "_Piece");
            if (EditorManager.selectedColor == "W")
            {
                mCurrentCell.mCurrentPiece.GetComponent<Image>().color = GameButton.getColor(EditorManager.optionsData.colors.whitePiecesColor);
            }
            else
            {
                mCurrentCell.mCurrentPiece.GetComponent<Image>().color = GameButton.getColor(EditorManager.optionsData.colors.blackPiecesColor);
            }

            VoidPiece vPF = new VoidPiece();
            vPF.Setup(EditorManager.selectedPiece, EditorManager.selectedColor);

            EditorManager.matrixPiecesEditor[mCurrentCell.mBoardPosition.x, mCurrentCell.mBoardPosition.y] = vPF;
        }
                
    }
}
