using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryableCubeScript : FryableScript
{
    public GameObject cubeMesh;

    GameObject top;
    GameObject bottom;
    GameObject north;
    GameObject south;
    GameObject east;
    GameObject west;

    Vector3 scaleDir = Vector3.zero;
    float maxFrySize = 0.025f;
    float minFrySize = 0.01f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        top = cubeMesh.transform.Find("Top").gameObject;
        bottom = cubeMesh.transform.Find("Bottom").gameObject;
        north = cubeMesh.transform.Find("Side_North").gameObject;
        south = cubeMesh.transform.Find("Side_South").gameObject;
        east = cubeMesh.transform.Find("Side_East").gameObject;
        west = cubeMesh.transform.Find("Side_West").gameObject;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    //----------------------------------------------------------------
    public override void StopFry()
    {
        base.StopFry();
    }

    //----------------------------------------------------------------
    public override void Fry()
    {
        base.Fry();


        scaleDir = cubeMesh.transform.localScale;
        Vector3 upVector = transform.up;

        Mathf.Abs(upVector.x);
        Mathf.Abs(upVector.y);
        Mathf.Abs(upVector.z);

        float upDir = Mathf.Max(upVector.x, upVector.y, upVector.z);

        if (upDir == upVector.x)
        {
            scaleDir.x = 0.25f * minFrySize;
            scaleDir.y = 1.0f * maxFrySize;
            scaleDir.z = 1.0f * maxFrySize;
        }
        else if (upDir == upVector.y)
        {
            scaleDir.x = 1.0f * maxFrySize;
            scaleDir.y = 0.25f * minFrySize;
            scaleDir.z = 1.0f * maxFrySize;
        }
        else if (upDir == upVector.z)
        {
            scaleDir.x = 1.0f * maxFrySize;
            scaleDir.y = 1.0f * maxFrySize;
            scaleDir.z = 0.25f * minFrySize;
        }

        //Debug.Log(transform.up + "   " + scaleDir.ToString("F4"));

        cubeMesh.transform.localScale = Vector3.Lerp(cubeMesh.transform.localScale, scaleDir, 0.001f);
    }

    //----------------------------------------------------------------
    public void OnCollisionEnter(Collision collision)
    {
        if (!IsFrying())
            return;
    }

    //----------------------------------------------------------------
    public GameObject GetContactSide(ContactPoint contact)
    {
        if (contact.thisCollider.gameObject == top)
            return top;

        if (contact.thisCollider.gameObject == bottom)
            return bottom;

        if (contact.thisCollider.gameObject == north)
            return north;

        if (contact.thisCollider.gameObject == south)
            return south;

        if (contact.thisCollider.gameObject == east)
            return east;

        if (contact.thisCollider.gameObject == west)
            return west;

        return null;
    }

    //----------------------------------------------------------------
    public GameObject GetOppositeSide(GameObject side)
    {
        if (side == top)
            return bottom;

        if (side == bottom)
            return top;

        if (side == north)
            return south;

        if (side == south)
            return north;

        if (side == east)
            return west;

        if (side == west)
            return east;

        return null;
    }
}
