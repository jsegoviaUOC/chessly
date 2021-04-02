using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Definició del tauler
    public Board mBoard;

    // Definició del gestor de peces
    public PieceManager mPieceManager;

    // Proporcions del tauler
    public int xAxis;
    public int yAxis;
    public static int xMargin;

    // Llistat de peces
    private string[] mTypePieces = new string[5]
    {
        "P","T", "KN", "B", "Q"
    };

    // Array de l'ordre de les peces en el joc (pendent de fer el tamany dinàmic)
    public static string[] mPieces = new string[16];

    // S'executa al iniciar l'escena
    void Start()
    {
        // Definició del tipus de joc
        typeGame();

        // Creació del tauler
        mBoard.Create(xAxis, yAxis);

        // Creació de les peces
        mPieceManager.Setup(mBoard);
    }

    // Funció per definir el tipus de joc
    public void typeGame()
    {
        // Joc clàssic
        switch (GameButton.typeGame)
        {
            case 1:

                xMargin = Random.Range(0, 3);
                xAxis = 8 + xMargin * 2;
                yAxis = Random.Range(5, 8);

                // Array de l'ordre de les peces en el joc random
                int pieceRand = Random.Range(0, 5);

                for (int i = 0; i < 16; i++)
                {
                    mPieces[i] = mTypePieces[pieceRand];
                    pieceRand = Random.Range(0, 5);
                }

                // Es força el rei en la cel·la estàndar
                mPieces[12] = "K";

                break;
            default:

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                // Array de l'ordre de les peces en el joc classic
                mPieces = new string[16]
                {
                    "P", "P", "P", "P", "P", "P", "P", "P",
                    "T", "KN", "B", "Q", "K", "B", "KN", "T"
                };

                break;
        }
        
    }

}
