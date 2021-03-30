using UnityEngine;
using UnityEngine.UI;

public class Bishop : BasePiece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviment de l'alfil
        mMovement = new Vector3Int(0, 0, 7);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("B_Piece");
    }
}
