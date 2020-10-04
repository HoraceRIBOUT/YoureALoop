using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource pianoRain;
    public AudioSource movieBoom;
    public AudioSource tibetanSing_grave;
    public AudioSource tibetanSing_light;
    public AudioSource drone_aigue;
    public AudioSource drone_grave;
    public AudioSource drone_tension;
    public AudioSource rainWindow;
    public List<AudioSource> listTaiko;

    [Header("Mixage (volume)")]
    public AnimationCurve ratioSizeVolume;
    private float sizeOfTheHero = 1;
    public float drone_tension_volumeMAX = 0.583f;

    private float tensionVolume = 0;
    public AnimationCurve tensionCurve;
    public AnimationCurve aigueCurve;
    private float sizeVolume = 0;
    public AnimationCurve sizeVolumeCurve;

    [Header("Music")]
    public float timerForTensionRising = 0.1f;
    public float timerForTibetanFalling = 1f;
    public float fallingSpeedForTension = 3f;

    public int indexForBell = 0;
    public int indexForTaiko = 0;

    [Header("Progression")]
    public float distanceMade = 0;


    public static SoundManager instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Update()
    {
        if (tensionVolume < drone_tension_volumeMAX) 
        {
            tensionVolume += Time.deltaTime * timerForTensionRising;
            if (tensionVolume > drone_tension_volumeMAX)
                tensionVolume = drone_tension_volumeMAX;
            drone_tension.volume = tensionCurve.Evaluate(tensionVolume);
            drone_aigue.volume = aigueCurve.Evaluate(tensionVolume);
        }

        if(drone_grave.volume > 0)
        {
            sizeVolume -= Time.deltaTime * timerForTibetanFalling;
            drone_grave.volume = sizeVolumeCurve.Evaluate(sizeVolume);
        }


        ProgressManagement();

    }
    public void ProgressManagement()
    {
        float ratio = distanceMade / 30;
        distanceMade += Time.deltaTime * 0.1f;
        if (distanceMade > 30)
            distanceMade = 30;

        float volume = ratio * (sizeVolumeCurve.Evaluate(sizeOfTheHero));
        float otherVolume = ratio * (1 - sizeVolumeCurve.Evaluate(sizeOfTheHero));

        //Zero then tibet then rain and piano then the end

        //First step = 
        if(level == 2)
        {
            //Second step = 
            float offset = 0.3f;
            volume = ratio * (sizeVolumeCurve.Evaluate(sizeOfTheHero) + offset);
            //Debug.Log("Volume (2) " + volume);
            pianoRain.volume = volume;
            rainWindow.volume = otherVolume;


            tibetanSing_grave.volume -= Time.deltaTime * 0.1f;
            tibetanSing_light.volume -= Time.deltaTime * 0.1f;
        }
        else if(level == 1)
        {
            //Debug.Log("Volume (1) " + volume);
            tibetanSing_grave.volume = volume;
            tibetanSing_light.volume = otherVolume;
        }

    }
    public int level = 0;
    public void GoToNextLevel()
    {
        Debug.Log("Next level !" + level);
        level++; 
        distanceMade = 0;


        if(level == 4)
        {
            StartCoroutine(DisplayEndText());
        }

    }

    public UnityEngine.UI.Text textEnd;
    public UnityEngine.UI.Text textThanks;
    public IEnumerator DisplayEndText()
    {
        while (textEnd.color.a < 1)
        {
            textEnd.color += Color.black * Time.deltaTime * 0.3f;
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(2f);
        while (textThanks.color.a < 1)
        {
            textThanks.color += Color.black * Time.deltaTime * 0.3f;
            yield return new WaitForSeconds(0.005f);
        }
    }


    public void SwitchSound(bool left)
    {        
        StartCoroutine(makeDroneVolumeToZeroVeryQuick());
    }

    public void WallSound(bool left)
    {
        indexForTaiko++;
        if (indexForTaiko == listTaiko.Count)
            indexForTaiko = 0;

        if (listTaiko[indexForTaiko].isPlaying)
        {
            listTaiko[indexForTaiko].Stop();
            listTaiko[indexForTaiko].time = 0;
        }
        listTaiko[indexForTaiko].Play();

        StartCoroutine(makeDroneVolumeToZeroVeryQuick());
    }


    public void AddSizeChange(float size, float distanceMade)
    {
        sizeVolume += distanceMade * 0.5f;
        if (sizeVolume > 1)
            sizeVolume = 1;

        drone_grave.volume = sizeVolumeCurve.Evaluate(sizeVolume);
        sizeOfTheHero = size;
    }



    IEnumerator makeDroneVolumeToZeroVeryQuick()
    {
        while (tensionVolume > 0)
        {
            tensionVolume -= Time.deltaTime * fallingSpeedForTension;
            drone_tension.volume = tensionCurve.Evaluate(tensionVolume);
            yield return new WaitForSeconds(1f / 60f);
        }

        tensionVolume = 0;
        drone_tension.volume = 0;
    }
}
