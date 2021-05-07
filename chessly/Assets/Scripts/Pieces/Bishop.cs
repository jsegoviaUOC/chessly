using UnityEngine;
using UnityEngine.UI;

public class Bishop : BasePiece
{

    // Valor assignat per matar aquesta peça
    public int price = 3;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviment de l'alfil
        mMovement = new Vector3Int(0, 0, 13);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("B_Piece");
    }
}
