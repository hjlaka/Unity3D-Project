using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : SingleTon<DisplayManager>
{

    public void ShowOnBoard(Piece piece)
    {
        IMarkable markable = piece.place?.board as IMarkable;
        if (markable != null)
        {
            markable.PreShow(piece);
        }
    }

    public void FadeOnBoard(Piece piece)
    {
        IMarkable markable = piece.place?.board as IMarkable;
        if (markable != null)
        {
            markable.PreShowEnd(piece);
        }
    }

    public void ColorChange(Piece piece)
    {
        piece.ChangeColor();
    }

}
