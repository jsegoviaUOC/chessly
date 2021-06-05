using UnityEngine;
using UnityEngine.UI;

public class XiangqiXiang : BasePiece
{

    // Valor assignat per matar aquesta peça
    new public int price = 3;

    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager, GameObject newSoundMove, GameObject newSoundDead)
    {
        // Inicialització de la peça
        base.Setup(newTeamColor, newSpriteColor, newPieceManager, newSoundMove, newSoundDead);

        // Moviment de l'elefant
        mMovement = new Vector3Int(0, 0, 1);

        // Sprite
        GetComponent<Image>().sprite = Resources.Load<Sprite>("X_Piece");
    }
}
