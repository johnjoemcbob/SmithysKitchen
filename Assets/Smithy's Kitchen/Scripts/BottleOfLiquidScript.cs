using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfLiquidScript : MonoBehaviour
{
    public GameObject liquidStreamObj;
    LineRenderer liquidLineRenderer;

    VRTK.Prefabs.Interactions.Interactables.InteractableFacade facade;

    bool isOpen = true; //Leave it true for now.

    // Start is called before the first frame update
    void Start()
    {
        liquidLineRenderer = liquidStreamObj.GetComponent<LineRenderer>();

        facade = GetComponent<VRTK.Prefabs.Interactions.Interactables.InteractableFacade>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.up.y <= 0.0f)
        {
            if (facade.IsGrabbed)
                StartPouring();
        }
        else
        {
            StopPouring();
        }

        if (!facade.IsGrabbed)
            StopPouring();
    }

    void FixedUpdate()
    {
        if (!liquidStreamObj.activeSelf)
            return;

        Vector3 dir = Vector3.up;
        RaycastHit hit;

        if (Physics.Raycast(liquidStreamObj.transform.position, -dir, out hit, 50))
        {
            Debug.DrawLine(liquidStreamObj.transform.position, hit.point);
            liquidLineRenderer.SetPosition(0, liquidStreamObj.transform.position);
            liquidLineRenderer.SetPosition(1, hit.point);

            //Make sure we're only hitting soup
            SoupPotScript soupScript = hit.collider.gameObject.GetComponentInParent<SoupPotScript>();
            if (soupScript != null)
            {
                soupScript.AddBase();
                soupScript.LerpBaseMaterial(liquidLineRenderer.material);
            }
        }
    }

    void StartPouring()
    {
        liquidStreamObj.SetActive(true);
    }

    void StopPouring()
    {
        liquidStreamObj.SetActive(false);
    }
}
