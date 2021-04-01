using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    // Retorn al menu principal
    public void BackMenu()
    {
        // Carrega la escena del menu principal
        SceneManager.LoadScene("Main Menu");
    }

    // Funció per definir el tipus de joc
    public void typeGame()
    {
        if (GameButton.typeGame == 0)
        {
            xAxis = 8;
            yAxis = 8;
        }
        else
        {
            xAxis = Random.Range(8, 10);
            yAxis = Random.Range(6, 9);
        }
    }

}
