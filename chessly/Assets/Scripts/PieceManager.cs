using System;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    // Control per saber si ha acabat la partida
    public bool isEndGame = false;

    // Prefab de les peces
    public GameObject mPiecePrefab;

    // Creació llista de peces
    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;
    private List<BasePiece> mPromotedPieces = new List<BasePiece>();

    // Array de l'ordre de les peces en el joc classic (pendent de canviar)
    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "T", "KN", "B", "Q", "K", "B", "KN", "T"
    };

    private string[] mWhitePiecesOrder = new string[16];
    private string[] mBlackPiecesOrder = new string[16];
    private string[] mAuxiliarPiecesOrder = new string[16];

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

        //peces no estandards (no desarroladas)
        //{"M",  typeof(Makruk)}
        //{"X",  typeof(Xiangqi)}
        //{"E",  typeof(Elefant)}
    };

    public void Setup(Board board)
    {
        // Llistat de peces incials de cada color (actualment per defecte)
        mWhitePiecesOrder = mPieceOrder;
        mBlackPiecesOrder = mPieceOrder;

        // Creació de les peces blanques i negres
        mWhitePieces = CreatePieces(Color.white, new Color32(213, 198, 125, 255));
        mBlackPieces = CreatePieces(Color.black, new Color32(31, 30, 24, 255));

        // Colocació inicial
        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(Board.yLimit - 2, Board.yLimit-1, mBlackPieces, board);

        // Inici del torn de les blanques
        SwitchPlayer(Color.black);
    }

    // Funció per crear les peces de tot un color
    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        if( teamColor == Color.white)
        {
            mAuxiliarPiecesOrder = mWhitePiecesOrder;
        }
        else
        {
            mAuxiliarPiecesOrder = mBlackPiecesOrder;
        }

        for (int i = 0; i < mAuxiliarPiecesOrder.Length; i++)
        {
            // S'agafa el tipus de peça del diccionari en funció de l'array
            string key = mAuxiliarPiecesOrder[i];
            Type pieceType = mPieceLibrary[key];

            // Es crea la peça
            BasePiece newPiece = CreatePiece(pieceType);
            newPieces.Add(newPiece);

            // S'inicialitza la peça amb el seu color i l'equip al que pertany
            newPiece.Setup(teamColor, spriteColor, this);
        }

        // Es retorna el llistat complert de peces a la funció anterior
        return newPieces;
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

    // Funció per colocar les peces d'un color en formació inicial (només acepta el tauler classic)
    private void PlacePieces(int frontRow, int backRow, List<BasePiece> pieces, Board board)
    {
        // Per a cada fila (del tauler estandar)
        for (int i = 0; i < 8; i++)
        {
            // Posició de les peces frontals  
            pieces[i].Place(board.mAllCells[i, frontRow]);

            // Posició de les peces traseres  
            pieces[i + 8].Place(board.mAllCells[i, backRow]);
        }
    }

    // Funció que inicialitza les peces com actives
    // Permet el control del torn no permentent moure el color que no té torn
    private void changeStateActivePieces(List<BasePiece> allPieces, bool value)
    {
        // Per a cada peça del color, aquesta s'activa o no
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }

    // Funció per canviar de torn
    public void SwitchPlayer(Color previousColor)
    {
        // Si la partida ha acabat
        if (isEndGame)
        {
            // Es reseteixen les peces
            ResetPieces();

            // Es canvia el flag per marcar que la partida no ha acabat
            isEndGame = false;

            // Es canvia el color del jugador en torn per asegurar que el blanc comença la següent partida
            previousColor = Color.black;
        }

        // Es comprova de qui es el torn i s'ativen les peces d'aquest color i es desactiven les de l'altre
        if (previousColor != Color.white)
        {
            changeStateActivePieces(mWhitePieces, true);
            changeStateActivePieces(mBlackPieces, false);

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
        }
        else
        {
            changeStateActivePieces(mWhitePieces, false);
            changeStateActivePieces(mBlackPieces, true);

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
}
