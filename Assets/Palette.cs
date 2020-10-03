using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palette : MonoBehaviour
{
    [System.Serializable]
    public struct colourList
    {
        public string id;
        public Color background;
        public Color hero;
        public Color heroOtherSens;
        public Color ennemi;
        public Color obstacle;
        public Color wall;
    }

    public List<colourList> paletteList = new List<colourList>();
    public int currentPalette = 0;


    [Button]
}
