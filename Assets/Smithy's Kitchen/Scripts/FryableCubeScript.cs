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


    List<ParticleCollisionEvent> collisionEvents;

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

        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    //----------------------------------------------------------------
    public override void StartFry()
    {
        base.StartFry();
        scaleDir = Vector3.zero;
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

        float upResult = Vector3.Dot(Vector3.up, transform.up);
        float downResult = Vector3.Dot(Vector3.up, -transform.up);
        float forwardResult = Vector3.Dot(Vector3.up, transform.forward);
        float backResult = Vector3.Dot(Vector3.up, -transform.forward);
        float rightResult = Vector3.Dot(Vector3.up, transform.right);
        float leftResult = Vector3.Dot(Vector3.up, -transform.right);

        if ((upResult >= 0.9f) || (downResult >= 0.9f))
        {
            //Debug.Log("updir is Y");
            scaleDir.x = 1.0f * maxFrySize;
            scaleDir.y = 0.25f * minFrySize;
            scaleDir.z = 1.0f * maxFrySize;
        }
        else if ((forwardResult >= 0.9f) || (backResult >= 0.9f))
        {
            //Debug.Log("updir is Z");
            scaleDir.x = 1.0f * maxFrySize;
            scaleDir.y = 1.0f * maxFrySize;
            scaleDir.z = 0.25f * minFrySize;
        }
        else if ((rightResult >= 0.9f) || (leftResult >= 0.9f))
        {
            //Debug.Log("updir is X");
            scaleDir.x = 0.25f * minFrySize;
            scaleDir.y = 1.0f * maxFrySize;
            scaleDir.z = 1.0f * maxFrySize;
        }

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

    public Color GetAverageColour()
    {
        Color mixedColor = Color.white;

        MeshRenderer topRenderer = top.GetComponent<MeshRenderer>();
        mixedColor = mixedColor * topRenderer.material.color;

        MeshRenderer bottomRenderer = bottom.GetComponent<MeshRenderer>();
        mixedColor = mixedColor * bottomRenderer.material.color;

        MeshRenderer northRenderer = north.GetComponent<MeshRenderer>();
        mixedColor = mixedColor * northRenderer.material.color;

        MeshRenderer southRenderer = south.GetComponent<MeshRenderer>();
        mixedColor = mixedColor * southRenderer.material.color;

        MeshRenderer eastRenderer = east.GetComponent<MeshRenderer>();
        mixedColor = mixedColor * eastRenderer.material.color;

        MeshRenderer westRenderer = west.GetComponent<MeshRenderer>();
        mixedColor = mixedColor * westRenderer.material.color;

        return mixedColor;
    }

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponent<ParticleSystem>();

        if (ps)
        {
            int numCollisionEvents = ps.GetCollisionEvents(this.gameObject, collisionEvents);
            Color particleColour = ps.main.startColor.color;

            foreach(ParticleCollisionEvent colEvent in collisionEvents)
            {
                if ((colEvent.colliderComponent != null) && (colEvent.colliderComponent.gameObject != null))
                {
                    if (colEvent.colliderComponent.gameObject.GetComponentInParent<FryableCubeScript>())
                    {
                        GameObject cubeSide = colEvent.colliderComponent.gameObject;
                        MeshRenderer cubeRenderer = cubeSide.GetComponent<MeshRenderer>();


                        float colourSpeed = 0.001f;
                        if (IsFrying())
                        {
                            colourSpeed = 0.01f;
                        }

                        cubeRenderer.material.color = Color.Lerp(cubeRenderer.material.color, particleColour, colourSpeed);
                    }
                }
            }
        }
    }
}
