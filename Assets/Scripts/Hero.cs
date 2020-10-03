using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public bool leftDirection = false;
    public bool bounceBack = false;
    [Range(0.3f,5f)]
    public float size = 1f;

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

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        MovementManagement();

        predictParticle = predict.GetComponent<ParticleSystem>();
        shapeMod = predictParticle.shape;
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

            //predictSprite.localScale = Vector3.one * size;

            //every X step ? better?
            predictParticle.Play();
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

    public void MovementManagement()
    {
        float newAccumulateur = accumulateur + Time.deltaTime * (leftDirection ? -1 : 1) * (bounceBack ? -1 : 1) * rotationSpeed * (1/size);
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
