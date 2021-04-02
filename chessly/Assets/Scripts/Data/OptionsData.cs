using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public Colors colors;

    public string language;

}

[System.Serializable]
public class Colors
{
    public string whitePiecesColor;
    public string blackPiecesColor;
    public string boardColor;
}