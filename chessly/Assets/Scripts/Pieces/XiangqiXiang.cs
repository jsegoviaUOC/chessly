using UnityEngine;
using UnityEngine.UI;

public class XiangqiXiang : BasePiece
{

    // Valor assignat per matar aquesta peça
    public int price = 3;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        // Moviment de l'elefant
        mMovement = new Vector3Int(0, 0, 1);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("X_Piece");
    }
}
