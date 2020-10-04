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
    public AnimationCurve speedAdaptator = AnimationCurve.Linear(0,0,1,1);

    public Vector2 currentTangent = new Vector2(0, 1);
    public float accumulateur = 0;

    public bool moveAuthorize = true;
    public float rotationSpeed = 1f;


    [Header("Body visual")]
    public Transform body;

    [Header("Predict part")]
    public Transform predict;
    //public Transform predictSprite;
    //public Animator predictAnimator;
    public ParticleSystem predictParticle;
    public ParticleSystem.ShapeModule shapeMod;
    public ParticleSystem.Burst emisMod;

    public AnimationCurve rateAdjusteur = AnimationCurve.Linear(1, 300, 5, 600);
    public GameObject sizeParticule;

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        MovementManagement();

        predictParticle = predict.GetComponent<ParticleSystem>();
        shapeMod = predictParticle.shape;
        emisMod = predictParticle.emission.GetBurst(0);
    }

    public void InputManagement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            leftDirection = !leftDirection;
            predictParticle.Play();
        }

        if (Input.GetMouseButton(1))
        {
            size += Time.deltaTime * 4f * (goingUp ? 1 : -1);
            if (size > 5f)
                goingUp = false;
            else if (size < 0.3f)
                goingUp = true;

            shapeMod.scale = Vector3.one * size;
            emisMod.count = rateAdjusteur.Evaluate(size);
            predictParticle.emission.SetBurst(0, emisMod);

            if (Mathf.Abs(memorieSize -size) > sizeDelta )
            {
                ParticleSystem pS = Instantiate(sizeParticule, predict.position, Quaternion.identity, null).GetComponent<ParticleSystem>();

                ParticleSystem.ShapeModule shape = pS.shape;
                shape.scale = Vector3.one * size;
                ParticleSystem.Burst emis = pS.emission.GetBurst(0);
                emis.count = rateAdjusteur.Evaluate(size);
                pS.emission.SetBurst(0, emis);
                //Color too
                memorieSize = size;
            }
            //predictSprite.localScale = Vector3.one * size;
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            //predictAnimator.SetBool("Change", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //predictAnimator.SetBool("Change", false);
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

        Vector2 falseObjectif =Vector3.Project(centerPosition, newTangent);
        
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
        Vector3 target = (Vector2)this.transform.position + currentTangent * (bounceBack ? -1 : 1);
        float dot = Vector3.Dot(Vector3.up, (target - this.transform.position).normalized);
        float sign = Mathf.Sign(Vector3.Cross(Vector3.forward, (target - this.transform.position)).y);
        body.transform.localEulerAngles = Vector3.forward * (dot - 1) * 90 * sign;
    }
}
