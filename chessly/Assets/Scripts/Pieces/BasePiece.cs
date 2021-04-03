using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{
    // Es declara el color
    public Color mColor = Color.clear;

    // Com algunes peces tenen propietats especials si és el seu primer moviment, es guarda aquesta informació
    public bool mIsFirstMove = true;

    // col·locació inicial i actual
    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    // Cel·la objectiu
    protected Cell mTargetCell = null;

    // Moviment
    protected Vector3Int mMovement = Vector3Int.one;

    // Cel·les resaltades
    protected List<Cell> mPossiblePathCells = new List<Cell>();

    // Inicialització de la peça
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Gestor de peces
        mPieceManager = newPieceManager;

        // Color i sprite associat
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    // Lloc de la peça
    public virtual void Place(Cell newCell)
    {
        // Iniciaització de la cel·la inicial i actual
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        // Transform de l'objecte
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }

    // Reset de la peça
    public void Reset()
    {
        // S'elimina la peça
        Kill();

        // S'indica que es tracta del seu primer moviment en la pròxima partida
        mIsFirstMove = true;

        // Es col·loca a la seva posició inicial
        Place(mOriginalCell);
    }

    // Fucnió per eliminar la peça
    public virtual void Kill()
    {
        // Es borra la informació sobre la cel·la actual que ocupa
        mCurrentCell.mCurrentPiece = null;

        // S'elimina l'objecte
        gameObject.SetActive(false);
    }

    // Funció per saber si es pot moure
    public bool HasMove()
    {
        // Es crida la funció per comprovar el pathing
        CheckPathing();

        // Si no té moviments retorna false
        if (mPossiblePathCells.Count == 0)
            return false;

        // si té moviments disponibles, retorna true
        return true;
    }

    // Creació del path de la peça
    private void CreatePath(int x, int y, int mov)
    {
        // Es guarda la posició actual en variable auxiliars
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        // Per cada casella del moviment
        for (int i = 1; i <= mov; i++)
        {
            // S'atualitza la posició de X i Y
            currentX += x;
            currentY += y;

            // Es mira l'estat de la cel·la
            CellState cellState = CellState.None;
            cellState = mCurrentCell.mBoard.StateCell(currentX, currentY, this);

            // Si hi ha un enemic o està lliure
            if (cellState == CellState.Enemy || cellState == CellState.Free)
            {
                // S'afegeix la cel·la a les possible opcions de moviment de a peça
                mPossiblePathCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            }

            // Si s'ha trobat un enemic o una cel·la ocupada, no es pot seguir amb aquest moviment
            if (cellState == CellState.Enemy || cellState != CellState.Free)
            {
                break;
            }
        }
    }

    // Funció per comprovar el path de la peça
    // Si la peça está rodejada de peces o de bordes del tauler, no es pot moure.
    // El cavall sobreescriu aquesta funció perque pot saltar per sobre d'altres peces
    protected virtual void CheckPathing()
    {
        // Horizontal
        CreatePath(1, 0, mMovement.x);
        CreatePath(-1, 0, mMovement.x);

        // Vertical 
        CreatePath(0, 1, mMovement.y);
        CreatePath(0, -1, mMovement.y);

        // Diagonal superior
        CreatePath(1, 1, mMovement.z);
        CreatePath(-1, 1, mMovement.z);

        // Diagonal inferior
        CreatePath(-1, -1, mMovement.z);
        CreatePath(1, -1, mMovement.z);
    }

    // Es mostren les possibles cel·les on pot moure la peça
    protected void ShowCells()
    {
        foreach (Cell cell in mPossiblePathCells)
            cell.mBorderImage.enabled = true;
    }

    // Es deixen de mostrar les possibles cel·les on pot moure la peça
    protected void ClearCells()
    {
        foreach (Cell cell in mPossiblePathCells)
            cell.mBorderImage.enabled = false;

        // S'esborra la informació sobre el path
        mPossiblePathCells.Clear();
    }

    // Funció per moure la peça
    protected virtual void Move()
    {
        // Es canvia l'estat del primer moviment a false
        mIsFirstMove = false;

        // Si hi ha un enemic en la cel·la objectiu, s'elimina
        mTargetCell.RemovePiece();

        // Es borra la peça de la cel·la actual
        mCurrentCell.mCurrentPiece = null;

        // S'actualitzen les dades de la cel·la de la peça
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        // Es col·loca la peça a la nova posició
        transform.position = mCurrentCell.transform.position;

        // S'esborra la informació sobre la cel·la objectiu
        mTargetCell = null;
    }

    // Funcions per realitzar el drag and drop de la peça

    // Inici del drag
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);

        // Calcul del path
        CheckPathing();

        // Mostra les possibles cel·les
        ShowCells();
    }

    // Durant el drag
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        // Moviment de la peça en funció del ratolí
        transform.position += (Vector3)eventData.delta;

        // Per a cada cel·la en el llistat de cel·les possibles del path
        foreach (Cell cell in mPossiblePathCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                // Si la peça está sobre una de les possibles cel·les, la cel·la objectiu és vàlida i es guarda la seva informació
                mTargetCell = cell;
                break;
            }

            // Si la peça no está sobre una de les possibles cel·les, la cel·la objectiu no és vàlida
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        // S'esborr la informació sobre el path
        ClearCells();

        // Si la cel·la objectiu no és vàlida, es torna la peça al seu lloc inicial i acaba aquesta funció
        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }

        // Es mou la peça
        Move();

        // Si es juga contra l'ordinador, es fa un canvi de turn
        if (GameManager.NPColor == Color.white || GameManager.NPColor == Color.black)
        {
            mPieceManager.SwitchPlayer();
        }
        else// Mostra el missatge de finalització del turn del jugador
        {
            mPieceManager.ShowTextInfo();
        }
    }

    // Moviment al atzar del non-player, el jugador controlat per l'ordinador
    public void NonPlayerMove()
    {
        // S'escull una cel·la a l'atzar de les possibles
        int index = Random.Range(0, mPossiblePathCells.Count);
        mTargetCell = mPossiblePathCells[index];

        // Es mou la peça
        Move();
    }

}
