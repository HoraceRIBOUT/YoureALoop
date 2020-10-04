using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LDManager : MonoBehaviour
{
    public static LDManager instance = null;

    private void Awake()
    {
        instance = this;
        ResetLevel();
    }

    [System.Serializable]
    public struct level
    {
        public string id;
        public List<GameObject> part;
    }

    public List<level> listLevel = new List<level>();


    public int indexCurrent = 0;
    private int memeoryIndex = 0;

    public void ResetLevel()
    {
        foreach(level lvl in listLevel)
        {
            foreach (GameObject gO in lvl.part)
            {
                gO.SetActive(false);
            }
        }
    }

    public void Update()
    {
        if(indexCurrent != memeoryIndex)
        {
            if (indexCurrent >= listLevel.Count || indexCurrent < 0)
            {
                indexCurrent = memeoryIndex;
                return;
            }
            ResetLevel();


            for (int i = 0; i < indexCurrent + 1; i++)
            {
                foreach (GameObject gO in listLevel[i].part)
                {
                    gO.SetActive(true);
                }
            }

            
            memeoryIndex = indexCurrent;
        }
    }

    public void LoadNextLevel()
    {
        indexCurrent++;
        memeoryIndex = indexCurrent;


        StartCoroutine(LoadNextLevel(indexCurrent));
    }

    IEnumerator LoadNextLevel(int indexLevel)
    {
        foreach (GameObject gO in listLevel[indexCurrent].part)
        {
            gO.SetActive(true);
            float randomWait = Random.Range(0.001f, 0.02f);
            yield return new WaitForSeconds(randomWait);
        }
    }

    

}
