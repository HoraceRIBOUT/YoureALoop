using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public bool leftDirection = false;
    public bool bounceBack = false;
    [Range(0.1f,5f)]
    public float size = 1f;
    public Vector2 minMaxSize = new Vector2(0.2f, 2f);

    public Vector2 currentTangent = new Vector2(0, 1);
    public float accumulateur = 0;

    public bool moveAuthorize = true;
    public float rotationSpeed = 1f;
    //public List<GameObject> debugObj = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        InputManagement();
        MovementManagement();
    }

    public void InputManagement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            leftDirection = !leftDirection;
        }

        if (Input.GetMouseButtonDown(1))
        {

        }
    }

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

        Debug.DrawLine(transform.position, (Vector2)this.transform.position + newObjectif, Color.green);
        Debug.DrawLine((Vector2)this.transform.position + newObjectif, (Vector2)this.transform.position + centerPosition, Color.green);
        Debug.DrawLine((Vector2)this.transform.position + centerPosition, transform.position, Color.green);

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
        //particule ?
    }
}
