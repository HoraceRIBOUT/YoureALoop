using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [Header("Movement")]
    public bool leftDirection = false;
    public bool bounceBack = false;
    [Range(0.3f,5f)]
    public float size = 1f;
    public Vector2 sizeMaxMin = new Vector2(0.3f, 5f);
    public AnimationCurve speedAdaptator = AnimationCurve.Linear(0,0,1,1);

    public Vector2 currentTangent = new Vector2(0, 1);
    public float accumulateur = 0;

    public bool moveAuthorize = true;
    public float rotationSpeed = 1f;


    [Header("Body visual")]
    public Transform body;
    public SpriteRenderer body_point;

    [Header("Predict part")]
    public Transform predict;
    //public Transform predictSprite;
    //public Animator predictAnimator;
    public ParticleSystem predictParticle;
    public ParticleSystem.ShapeModule shapeMod;
    public ParticleSystem.MainModule mainMod;
    public ParticleSystem.Burst burstMod;

    public AnimationCurve rateAdjusteur = AnimationCurve.Linear(1, 300, 5, 600);
    public GameObject sizeParticule;

    public GameObject impactGO;

    public void Impact(Collision2D collision)
    {
       bounceBack = !bounceBack;
       leftDirection = !leftDirection;

        //transform normal into quaternion
        Quaternion quat = Quaternion.FromToRotation(Vector3.up, -collision.contacts[0].normal);
        ParticleSystem.MainModule ps = Instantiate(impactGO, collision.contacts[0].point, quat, null).GetComponentInChildren<ParticleSystem>().main;

        ps.startColor = (leftDirection ^ bounceBack ? Palette.instance.GetCurrentColorPalette().particleLeft : Palette.instance.GetCurrentColorPalette().particleRight);

        SoundManager.instance.WallSound(leftDirection);
    }

    /// <summary>
    /// 
    /// </summary>
    private AlternatingGO[] allAlterningGOs;
    public void Start()
    {
        allAlterningGOs = FindObjectsOfType<AlternatingGO>(true);

        predictParticle = predict.GetComponent<ParticleSystem>();
        shapeMod = predictParticle.shape;
        mainMod = predictParticle.main;
        burstMod = predictParticle.emission.GetBurst(0);

        SwitchAllAlternating();
    }


    public void SwitchAllAlternating()
    {
        foreach (AlternatingGO alT in allAlterningGOs)
        {
            alT.SwitchSide(leftDirection ^ bounceBack);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        MovementManagement();
    }

    public void InputManagement()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();


        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            leftDirection = !leftDirection;
            body_point.color = (leftDirection ^ bounceBack ? Palette.instance.GetCurrentColorPalette().heroLeft : Palette.instance.GetCurrentColorPalette().heroRight);
            mainMod.startColor = (leftDirection ^ bounceBack ? Palette.instance.GetCurrentColorPalette().particleLeft : Palette.instance.GetCurrentColorPalette().particleRight);
            SwitchAllAlternating();
            predictParticle.Play();
            
            SoundManager.instance.SwitchSound(leftDirection);
        }


        float sizeBefore = size;
        if (Input.GetMouseButton(1))
        {
            size += Time.deltaTime * 4f * (goingUp ? 1 : -1);
            if (size > sizeMaxMin.y)
                goingUp = false;
            else if (size < sizeMaxMin.x)
                goingUp = true;
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            size += Time.deltaTime * 16f * Input.mouseScrollDelta.y;

            if (size >= sizeMaxMin.y)
                size = sizeMaxMin.y;
            else if (size <= sizeMaxMin.x)
                size = sizeMaxMin.x;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            size += Time.deltaTime * 4f;
            if (size >= sizeMaxMin.y)
                size = sizeMaxMin.y;

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            size -= Time.deltaTime * 4f;
            if (size <= sizeMaxMin.x)
                size = sizeMaxMin.x;
        }

        

        if(sizeBefore != size)
        {
            shapeMod.scale = Vector3.one * size;
            burstMod.count = rateAdjusteur.Evaluate(size);
            predictParticle.emission.SetBurst(0, burstMod);

            if (Mathf.Abs(memorieSize - size) > sizeDelta)
            {
                ParticleSystem pS = Instantiate(sizeParticule, predict.position, Quaternion.identity, null).GetComponent<ParticleSystem>();

                ParticleSystem.ShapeModule shape = pS.shape;
                shape.scale = Vector3.one * size;
                ParticleSystem.Burst emis = pS.emission.GetBurst(0);
                emis.count = rateAdjusteur.Evaluate(size);
                pS.emission.SetBurst(0, emis);
                ParticleSystem.MainModule main = pS.main;
                main.startColor = (leftDirection ^ bounceBack ? Palette.instance.GetCurrentColorPalette().particleLeft : Palette.instance.GetCurrentColorPalette().particleRight);

                memorieSize = size;
            }
            //predictSprite.localScale = Vector3.one * size;
            SoundManager.instance.AddSizeChange(size, Mathf.Abs(sizeBefore - size));
        }

    }
    private bool goingUp = true;
    public float sizeDelta = 0.6f;
    private float memorieSize = 1f;

    public void MovementManagement()
    {
        float speedAjusteur = (1 / (speedAdaptator.Evaluate(size)));
        float directionAjusteur = (leftDirection ? -1 : 1) * (bounceBack ? -1 : 1);
        float newAccumulateur = accumulateur + Time.deltaTime * directionAjusteur * rotationSpeed * speedAjusteur;
        Vector2 newTangent;
        newTangent.x = Mathf.Sin(newAccumulateur);
        newTangent.y = Mathf.Cos(newAccumulateur);
        newTangent.Normalize();

        Vector2 centerPosition;
        if (leftDirection) {
            centerPosition = new Vector2(-currentTangent.y, currentTangent.x).normalized * size;
        }
        else
        {
            centerPosition = new Vector2(currentTangent.y, -currentTangent.x).normalized * size;
        }


        //debugObj[1].transform.position = (Vector2)this.transform.position + centerPosition;

        Vector2 falseObjectif = Vector3.Project(centerPosition, newTangent);
        
        Vector2 directionFromCenter = falseObjectif - centerPosition;
        directionFromCenter = directionFromCenter.normalized * size;

        Vector2 newObjectif = centerPosition + directionFromCenter;

        //debugObj[0].transform.position = (Vector2)this.transform.position + newObjectif;

        //Debug.DrawLine(transform.position, (Vector2)this.transform.position + newObjectif, Color.green);
        Vector2 value = (Vector2)this.transform.position + centerPosition;
        predict.position = value;
        Debug.DrawLine((Vector2)this.transform.position + newObjectif, value, Color.green);
        Debug.DrawLine(value, transform.position, Color.green);

        if (moveAuthorize)
        {
            currentTangent = newTangent;
            accumulateur = newAccumulateur;

            this.transform.Translate(newObjectif);
        }

        PreviewManager();
    }


    public void PreviewManager()
    {
        body.transform.rotation = Quaternion.FromToRotation(Vector3.up, currentTangent * (bounceBack ? -1 : 1));
    }
}
