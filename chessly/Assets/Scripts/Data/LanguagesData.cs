using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguagesData
{
    public MenuData menu;
}

[System.Serializable]
public class MenuData
{
    public GeneralMenuData general;
    public OptionsMenuData options;
}

[System.Serializable]
public class OptionsMenuData
{
    public string ReturnMenuButton;
    public string LangEngButton;
    public string LangCatButton;

    public string SelectLanguageText;
    public string SelectWhitePiecesColorText;
    public string SelectBlackPiecesColorText;
    public string SelectBoardColorText;

    public WhitePiecesColors whitePiecesColor;
    public BlackPiecesColors blackPiecesColor;
    public BoardColors boardColor;
}

[System.Serializable]
public class GeneralMenuData
{
    public string ClassicButton;
    public string NonClassicButton;
    public string QuitButton;
    public string OptionsButton;
}

[System.Serializable]
public class WhitePiecesColors
{
    public string PW;
    public string IV;
    public string SB;
}

[System.Serializable]
public class BlackPiecesColors
{
    public string PB;
    public string EB;
    public string DR;
}

[System.Serializable]
public class BoardColors
{
    public string BW;
    public string WC;
    public string NE;
}
