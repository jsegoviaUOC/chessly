﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    private Board mBoard;

    // Control per saber si ha acabat la partida
    public bool isEndGame = false;

    // Prefab de les peces
    public GameObject mPiecePrefab;

    // Recuadre de text amb informació de la partida
    public GameObject textDisplayCanvas;

    // Controla el color del jugador actual
    public Color currentColor;

    // Color del jugador controlat per l'ordinador (Non-Player)
    public Color NPColor;

    // Creació llista de peces
    private List<BasePiece> mWhitePieces = new List<BasePiece>();
    private List<BasePiece> mBlackPieces = new List<BasePiece>();
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();

    // Array de l'ordre de les peces en el joc (pendent de fer el tamany dinàmic)
    private List<string> mWhitePiecesOrder;
    private List<string> mBlackPiecesOrder;
    private List<string> mAuxiliarPiecesOrder;

    private int gameStatus = 0;

    // Creació d'un diccionari per relacionar l'array de peces amb el tipus de peça
    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        // Peces estandards
        {"P",  typeof(Pawn)},//peó
        {"T",  typeof(Tower)},//torre
        {"KN", typeof(Knight)},//caball
        {"B",  typeof(Bishop)},//alfil
        {"K",  typeof(King)},//rei
        {"Q",  typeof(Queen)},//reina

        //peces no estandards
        {"M",  typeof(MakrukKhon)},// Khon (alfil) del joc Makruk
        {"X",  typeof(XiangqiXiang)},// Xiang (elefant) dels escacs chinesos
        {"D",  typeof(Dabbabah)}// Dabbāba (canyó) en algunes varians dels escacs
    };

    // Objecte per gestionar les traduccions
    public static LanguagesData languageData;

    // Boolean per a gestionar el torn online
    private bool yourTurn;

    public void Setup(Board board)
    {
        // S'assigna el tauler
        mBoard = board;

        // Recull els textos traduits
        languageData = LanguageManager.getLanguageText();

        // Llistat de peces incials de cada color
        for (int i = 0; i < Board.xLimit; i++)
        {
            for (int j = 0; j < Board.yLimit; j++)
            {
                VoidPiece vPiece = GameManager.matrixPieces[i, j];
                if (vPiece != null)
                {
                    Color teamColor;
                    Color spriteColor;
                    Type pieceType = mPieceLibrary[vPiece.type];

                    // Es crea la peça
                    BasePiece newPiece = CreatePiece(pieceType);

                    if (vPiece.color == "W")
                    {
                        teamColor = Color.white;
                        spriteColor = GameManager.getColor(GameManager.optionsData.colors.whitePiecesColor);

                        mWhitePieces.Add(newPiece);
                    }
                    else
                    {
                        teamColor = Color.black;
                        spriteColor = GameManager.getColor(GameManager.optionsData.colors.blackPiecesColor);

                        mBlackPieces.Add(newPiece);
                    }

                    // S'inicialitza la peça amb el seu color i l'equip al que pertany
                    newPiece.Setup(teamColor, spriteColor, this);

                    newPiece.Place(board.mAllCells[i, j]);
                }
            }
        }

        if (GameManager.isOnline)
        {
            currentColor = Color.white;
            StartCoroutine(GetGameStatus(GameManager.idGame));
            SetOnlineGame();
        }
        else
        {
            // Inici del torn de les blanques
            currentColor = Color.black;
            GameObject.Find("CanvasExitButton").GetComponent<Canvas>().enabled = false;
            SwitchPlayer();
        }
    }

    // Funció per crear una peça
    private BasePiece CreatePiece(Type pieceType)
    {
        // Es genera una nova peça a partir el prefab
        GameObject newPieceObject = Instantiate(mPiecePrefab);
        newPieceObject.transform.SetParent(transform);

        // S'aplica l'escala i rotació del objete
        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;

        // Es guarda la peça creada
        BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);

        //retorn de la peça a la funció anterior
        return newPiece;
    }

    // Funció que inicialitza les peces com actives
    // Permet el control del torn no permentent moure el color que no té torn
    private void ChangeStateActivePieces(List<BasePiece> allPieces, bool value)
    {
        // Per a cada peça del color, aquesta s'activa o no
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }

    // Funció per canviar de torn
    public void SwitchPlayer()
    {
        // Si la partida ha acabat
        if (isEndGame)
        {
            // Es reseteixen les peces
            ResetPieces();

            // Es canvia el flag per marcar que la partida no ha acabat
            isEndGame = false;

            // Es canvia el color del jugador en torn per asegurar que el blanc comença la següent partida
            currentColor = Color.black;

            // Es posa el text per defecte del botó del recuadre d'informació
            GameObject.Find("QuitTextButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.game.buttons.OkButton;

            // Si era una partida contra l'ordinador, es torna a calcular al atzar el color assignat a aquest
            if (GameManager.NPColor == Color.white || GameManager.NPColor == Color.black)
            {
                int NPColorRand = Random.Range(0, 2);
                GameManager.NPColor = NPColorRand == 1 ? Color.white : Color.black;
            }
        }

        // Es comprova de qui es el torn i s'ativen les peces d'aquest color i es desactiven les de l'altre
        if (currentColor != Color.white)
        {
            ChangeStateActivePieces(mWhitePieces, true);
            ChangeStateActivePieces(mBlackPieces, false);

            foreach (BasePiece piece in mPromotedPieces)
            {
                if (piece.mColor == Color.white)
                {
                    piece.enabled = true;
                }
                else
                {
                    piece.enabled = false;
                }
            }

            // S'actualitza el color del jugador actual
            currentColor = Color.white;
        }
        else
        {
            ChangeStateActivePieces(mWhitePieces, false);
            ChangeStateActivePieces(mBlackPieces, true);

            foreach (BasePiece piece in mPromotedPieces)
            {
                if (piece.mColor == Color.black)
                {
                    piece.enabled = true;
                }
                else
                {
                    piece.enabled = false;
                }
            }

            // S'actualitza el color del jugador actual
            currentColor = Color.black;
        }

        // Si el joc és contra l'ordinador, s'executen la funció del torn de l'ordinador i el canvi de torn consecutivament
        if (currentColor == GameManager.NPColor)
        {
            NonPlayerTurn();
            SwitchPlayer();
        }

    }

    // Funció per a resetejar de peces al final de la partida
    public void ResetPieces()
    {
        // S'eliminen totes les peces
        foreach (BasePiece piece in mPromotedPieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);
        }

        // Es buida la matriu de peces promocionades
        mPromotedPieces.Clear();

        // Es posicionen les dues matrius de peces
        foreach (BasePiece piece in mWhitePieces)
            piece.Reset();

        foreach (BasePiece piece in mBlackPieces)
            piece.Reset();
    }

    // Promoció d'un peó
    public void PromotePiece(Pawn pawn, Cell cell, Color teamColor, Color spriteColor)
    {
        // Primer s'elimina l'objecte peó
        pawn.Kill();

        // Es crea la peça promocionada
        // Per simplificar l'algoritme, de moment només es promociona a Reina
        BasePiece promotedPiece = CreatePiece(typeof(Queen));
        promotedPiece.Setup(teamColor, spriteColor, this);

        // Es posiciona en el tauler la peça
        promotedPiece.Place(cell);

        // S'afegeix la peça promocionada a la matriu corresponent
        mPromotedPieces.Add(promotedPiece);
    }

    // Funció per activar el recuadre de text amb informació de la partida
    public void ShowTextInfo()
    {
        // Es dehabiliten els moviments de les peces i el botó per sortir de la partida
        ChangeStateActivePieces(mWhitePieces, false);
        ChangeStateActivePieces(mBlackPieces, false);
        GameObject.Find("ButtonSpace").GetComponent<Canvas>().enabled = false;

        // Si el joc ha acabat el text que es mostra és diferent
        if (isEndGame)
        {
            string winText = currentColor == Color.white ? languageData.game.info.WinWhiteText : languageData.game.info.WinBlackText;
            textDisplayCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = winText;
            GameObject.Find("QuitTextButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.game.buttons.NewGameButton;
        }
        else
        {
            string nextTurnText = currentColor != Color.white ? languageData.game.info.TurnWhiteText : languageData.game.info.TurnBlackText;
            textDisplayCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = nextTurnText;
        }

        if (GameManager.isOnline)
        {
            yourTurn = false;
            ToggleOnlinePieces(false);
        }

        // Es mostra el recuadre d'informació
        textDisplayCanvas.GetComponent<Canvas>().enabled = true;
    }

    // Funció per tancar el recuadre de text
    public void CloseTextInfo()
    {
        // Finalitza el turn del color actual
        if (!GameManager.isOnline)
        {
            SwitchPlayer();
        }

        // S'habiliten i deshabiliten els botons i recuadres de text pertinents
        textDisplayCanvas.GetComponent<Canvas>().enabled = false;
        GameObject.Find("ButtonSpace").GetComponent<Canvas>().enabled = true;
    }

    /* FUNCIONS SINGLE PLAYER GAME */

    // Funció del torn del NP (Non-Player)
    private void NonPlayerTurn()
    {
        List<Cell> nonPlayerMoveOptions = new List<Cell>();
        List<BasePiece> nonPlayerPiecesOptions = new List<BasePiece>();
        List<BasePiece> auxPieces = null;

        int bestScore = -1;

        if (currentColor == Color.white)
        {
            auxPieces = mWhitePieces;
        }
        else
        {
            auxPieces = mBlackPieces;
        }

        foreach (BasePiece p in auxPieces)
        {
            if (p.gameObject.activeInHierarchy)
            {
                Cell cell = null;
                cell = p.NonPlayerMoves();
                if (cell != null)
                {
                    nonPlayerMoveOptions.Add(cell);
                    nonPlayerPiecesOptions.Add(p);
                }
            }

        }

        int index = 0;
        int indexBestPiece = 0;

        //Random rng = new Random();

        int n = nonPlayerMoveOptions.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            Cell cellValue = nonPlayerMoveOptions[k];
            nonPlayerMoveOptions[k] = nonPlayerMoveOptions[n];
            nonPlayerMoveOptions[n] = cellValue;

            BasePiece pieceValue = nonPlayerPiecesOptions[k];
            nonPlayerPiecesOptions[k] = nonPlayerPiecesOptions[n];
            nonPlayerPiecesOptions[n] = pieceValue;
        }

        foreach (Cell bestCell in nonPlayerMoveOptions)
        {

            if (bestCell.score > bestScore)
            {
                indexBestPiece = index;
                bestScore = bestCell.score;
            }
            index++;
        }

        nonPlayerPiecesOptions[indexBestPiece].NonPlayerDoMove(nonPlayerMoveOptions[indexBestPiece]);

    }

    /* FUNCIONS ONLINE GAME */

    public void SetOnlineGame()
    {
        if (GameManager.yourColor == Color.white)
        {
            yourTurn = true;
            ChangeStateActivePieces(mWhitePieces, true);
        }
        else
        {
            yourTurn = false;
            ChangeStateActivePieces(mWhitePieces, false);
        }
        ChangeStateActivePieces(mBlackPieces, false);
    }

    public void ToggleOnlinePieces(bool state)
    {
        List<BasePiece> pieces = GameManager.yourColor == Color.white ? mWhitePieces : mBlackPieces;

        ChangeStateActivePieces(pieces, state);
    }

    public void ExitButton()
    {
        StartCoroutine(GameManager.ExitGame());
    }

    // Funció que controla el flux del joc online
    public IEnumerator GetGameStatus(int idGame)
    {
        // Es deshabiliten els botons
        GameObject.Find("ButtonSpace").GetComponent<Canvas>().enabled = false;

        // Es mostra un cartell d'esperant contrincant
        textDisplayCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "waiting other player";
        textDisplayCanvas.GetComponent<Canvas>().enabled = true;
        GameObject.Find("CanvasQuitButton").GetComponent<Canvas>().enabled = false;
        GameObject.Find("ExitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.game.buttons.ReturnButton;
        GameObject.Find("CanvasExitButton").GetComponent<Canvas>().enabled = true;

        while (gameStatus != 1) // Mentre no hi hagi un contrincant
        {
            yield return new WaitForSeconds(8);
            StartCoroutine(GetStatus(idGame));
        }

        // Es treu el cartell d'espra de contrincant i s'habiliten els botons
        GameObject.Find("ButtonSpace").GetComponent<Canvas>().enabled = true;
        textDisplayCanvas.GetComponent<Canvas>().enabled = false;
        GameObject.Find("CanvasExitButton").GetComponent<Canvas>().enabled = false;
        GameObject.Find("CanvasQuitButton").GetComponent<Canvas>().enabled = true;

        while (gameStatus != 2) // Mentre no hi hagi un guanyador
        {
            yield return new WaitForSeconds(10);
            StartCoroutine(GetStatus(idGame));

            if (!yourTurn)
            {
                StartCoroutine(GetLastMove(idGame));
            }
        }

        StartCoroutine(GetWinner(idGame));
    }

    public IEnumerator GetStatus(int idGame)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://18.116.223.113/api/game-status/" + idGame);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
            gameStatus = int.Parse(www.downloadHandler.text);
        }
    }

    public IEnumerator GetLastMove(int idGame)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://18.116.223.113/api/game/" + idGame + "/last-move");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text != "")
            {
                var jsonMove = JsonUtility.FromJson<OnlineMoveData>(www.downloadHandler.text);

                //if (jsonMove.data.playerId != Login.idPlayer && !yourTurn)
                if (jsonMove.data.playerId != 1 && !yourTurn)
                {
                    // Moviment del contrincant online
                    Cell currentCell = mBoard.mAllCells[jsonMove.data.xCurrent, jsonMove.data.yCurrent];
                    Cell targetCell = mBoard.mAllCells[jsonMove.data.xTarget, jsonMove.data.yTarget];
                    currentCell.mCurrentPiece.OtherPlayerMove(targetCell);

                    ToggleOnlinePieces(true);
                    yourTurn = true;
                }
            }
        }

    }

    public IEnumerator GetWinner(int idGame)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://18.116.223.113/api/game/" + idGame);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var json = JsonUtility.FromJson<OnlineGameData>(www.downloadHandler.text);

            Debug.Log("get winner");
            Debug.Log(www.downloadHandler.text);

            string winnerText = "";

            if (json.data.winnerId != Login.idPlayer)
            {
                winnerText = "Sorry, You lose :(";
            }
            else
            {
                winnerText = "Congratulations! You win!";
            }

            // Es deshabiliten els botons
            GameObject.Find("ButtonSpace").GetComponent<Canvas>().enabled = false;

            // Es mostra un cartell del guanyador
            textDisplayCanvas.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = winnerText;
            textDisplayCanvas.GetComponent<Canvas>().enabled = true;
        }
    }

}