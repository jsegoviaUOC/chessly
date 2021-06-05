using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;

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

    public int price;

    protected bool nonPlayerTurnOn = false;

    protected GameObject soundMove;
    protected GameObject soundDead;

    Vector3 textInitialPosition;
    Vector3 piecesInitialPosition;
    Vector3 boardInitialPosition;
    public float shakeMagnitude = 0.7f, shakeTime = 0.8f;

    // Inicialització de la peça
    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject newSoundMove, GameObject newSoundDead)
    {
        // Gestor de peces
        mPieceManager = newPieceManager;

        // Color i sprite associat
        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();

        soundMove = newSoundMove;
        soundDead = newSoundDead;
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
        ShakeIt();

        Instantiate(soundDead);

        // Es borra la informació sobre la cel·la actual que ocupa
        mCurrentCell.mCurrentPiece = null;

        // S'elimina l'objecte
        gameObject.SetActive(false);
    }

    // Fucnió per eliminar la peça
    public virtual void SilenceKill()
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

                // S'evalua si la casella es bona o no per fer un atac al enemic
                if (nonPlayerTurnOn)
                {
                    if (cellState == CellState.Enemy)
                    {
                        mCurrentCell.mBoard.mAllCells[currentX, currentY].score = 100 + mCurrentCell.mCurrentPiece.price*10 + (10 - this.price);
                    }
                    else
                    {
                        mCurrentCell.mBoard.mAllCells[currentX, currentY].score = 0;
                    }
                }
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
        Instantiate(soundMove);

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

        if (GameManager.isOnline)
        {
            StartCoroutine(PostMove(mCurrentCell, mTargetCell));
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

    /* FUNCIONS SINGLE PLAYER */

    // Moviment al atzar del non-player, el jugador controlat per l'ordinador
    public Cell NonPlayerMoves()
    {
        nonPlayerTurnOn = true;
        if (HasMove())
        {
            Cell bestCell = null;
            int score = 0;

            foreach (Cell cell in mPossiblePathCells)
            {
                if(cell.score >= score)
                {
                    bestCell = cell;
                    score = cell.score;
                    Debug.Log(cell.mBoardPosition);
                }
            }

            // S'esborr la informació sobre el path
            ClearCells();

            return bestCell;
        }

        return null;
    }

    public void NonPlayerDoMove(Cell target)
    {
        mTargetCell = target;

        Move();

        nonPlayerTurnOn = false;
    }

    /* FUNCIÓ SHAKE */
    public void ShakeIt()
    {
        textInitialPosition = GameObject.Find("textDisplayerCanvas").transform.position;
        piecesInitialPosition = GameObject.Find("PrefPieceManager").transform.position;
        boardInitialPosition = GameObject.Find("PrefBoard").transform.position;
        InvokeRepeating("StartShaking", 0f, 0.005f);
        Invoke("StopShaking", shakeTime);
    }

    void StartShaking()
    {
        float piecesShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float piecesShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float boardShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float boardShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float textShakingOffsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float textShakingOffsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;

        Vector3 textIntermadiatePosition = GameObject.Find("textDisplayerCanvas").transform.position;
        Vector3 piecesIntermadiatePosition = GameObject.Find("PrefPieceManager").transform.position;
        Vector3 boardIntermadiatePosition = GameObject.Find("PrefBoard").transform.position;
        textIntermadiatePosition.x += textShakingOffsetX;
        textIntermadiatePosition.y += textShakingOffsetY;
        piecesIntermadiatePosition.x += piecesShakingOffsetX;
        piecesIntermadiatePosition.y += piecesShakingOffsetY;
        boardIntermadiatePosition.x += boardShakingOffsetX;
        boardIntermadiatePosition.y += boardShakingOffsetY;
        GameObject.Find("textDisplayerCanvas").transform.position = textIntermadiatePosition;
        GameObject.Find("PrefPieceManager").transform.position = piecesIntermadiatePosition;
        GameObject.Find("PrefBoard").transform.position = boardIntermadiatePosition;
    }

    void StopShaking()
    {
        CancelInvoke("StartShaking");
        GameObject.Find("textDisplayerCanvas").transform.position = textInitialPosition;
        GameObject.Find("PrefPieceManager").transform.position = piecesInitialPosition;
        GameObject.Find("PrefBoard").transform.position = boardInitialPosition;
    }

    /* FUNCIONS ONLINE GAME */

    public void OtherPlayerMove(Cell target)
    {
        mTargetCell = target;

        Move();
    }
    
    // funció per a crear moviment
    public IEnumerator PostMove(Cell current, Cell target)
    {
        WWWForm form = new WWWForm();
        form.AddField("x_current", current.mBoardPosition.x);
        form.AddField("y_current", current.mBoardPosition.y);
        form.AddField("x_target", target.mBoardPosition.x);
        form.AddField("y_target", target.mBoardPosition.y);
        form.AddField("game_id", GameManager.idGame);
        form.AddField("player_id", Login.idPlayer);

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/game/" + GameManager.idGame + "/move", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            int id = int.Parse(www.downloadHandler.text);
        }
    }
}
