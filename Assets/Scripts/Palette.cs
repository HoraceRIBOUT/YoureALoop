using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Palette : MonoBehaviour
{
    public static Palette instance;
    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogError("Hooo stop la");
        }
    }

    [System.Serializable]
    public struct colourList
    {
        public string id;
        public Color background;
        public Color hero;//also, trail
        public Color heroLeft;
        public Color heroRight;
        public Gradient particleLeft;
        public Gradient particleRight;
        public Color ennemiLeftOn;
        public Color ennemiLeftOff;
        public Color ennemiRightOn;
        public Color ennemiRightOff;
        public Color obstacle;
    }

    public List<colourList> paletteList = new List<colourList>();
    public int currentPalette = 0;

    public colourList GetCurrentColorPalette()
    {
        return paletteList[currentPalette];
    }

    [MyBox.ButtonMethod]
    public void ReColorAll()
    {
        if (currentPalette < 0 || currentPalette >= paletteList.Count)
            return;

        colourList curre = paletteList[currentPalette];
        foreach (SpriteRenderer sR in FindObjectsOfType<SpriteRenderer>())
        {
            if (sR.name.Contains("colHero"))
            {
                sR.color = curre.hero;
            }
            if (sR.name.Contains("colLef"))
            {
                sR.color = curre.heroLeft;
            }
            if (sR.name.Contains("colRig"))
            {
                sR.color = curre.heroRight;
            }
            if (sR.name.Contains("oncolEnnL"))
            {
                sR.color = curre.ennemiLeftOn;
            }
            if (sR.name.Contains("offcolEnnL"))
            {
                sR.color = curre.ennemiLeftOff;
            }
            if (sR.name.Contains("oncolEnnR"))
            {
                sR.color = curre.ennemiRightOn;
            }
            if (sR.name.Contains("offcolEnnR"))
            {
                sR.color = curre.ennemiRightOff;
            }
            if (sR.name.Contains("colObs"))
            {
                sR.color = curre.obstacle;
            }
            if (sR.name.Contains("colWall"))
            {

            }
        }

        FindObjectOfType<Camera>().backgroundColor = curre.background;
    }


    [ButtonMethod()]
    public void SetAllTransparencyToOne()
    {
        for (int i = 0; i < paletteList.Count; i++)
        {
            colourList list = paletteList[i];
            list.background.a = 1;
            list.hero.a = 1;
            list.heroLeft.a = 1;
            list.heroRight.a = 1;
            list.ennemiLeftOn.a = 1;
            list.ennemiLeftOff.a = 1;
            list.ennemiRightOn.a = 1;
            list.ennemiRightOff.a = 1;
            list.obstacle.a = 1;

            paletteList[i] = list;
        }
    }
}
