using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

public class Palette : MonoBehaviour
{
    [System.Serializable]
    public struct colourList
    {
        public string id;
        public Color background;
        public Color hero;
        public Color heroOtherSens;
        public Color heroLeft;
        public Color heroRight;
        public Color ennemi;
        public Color obstacle;
        public Color wall;
    }

    public List<colourList> paletteList = new List<colourList>();
    public int currentPalette = 0;


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
            if (sR.name.Contains("colEnn"))
            {
                sR.color = curre.ennemi;
            }
            if (sR.name.Contains("colObs"))
            {
                sR.color = curre.obstacle;
            }
            if (sR.name.Contains("colWall"))
            {
                sR.color = curre.wall;
            }
        }

        FindObjectOfType<Camera>().backgroundColor = curre.background;
    }
}
