using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnerScript : MonoBehaviour
{
    bool isHeated = false;
    public GameObject snappedItemList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSnapped()
    {
        HeatedLogic(isHeated);
    }

    public void SetHeated(bool val)
    {
        if (isHeated == val)
            return;

        isHeated = val;
        HeatedLogic(val);
    }

    void HeatedLogic(bool val)
    {
        //Heat fry zone
        if (snappedItemList)
        {
            Zinnia.Data.Collection.List.GameObjectObservableList list = snappedItemList.GetComponent<Zinnia.Data.Collection.List.GameObjectObservableList>();

            if (list && (list.SubscribableElements.Count > 0))
            {
                GameObject fryZone = list.SubscribableElements[0];

                FryZoneScript fryZoneScript = fryZone.GetComponentInChildren<FryZoneScript>();
                if (fryZoneScript != null)
                {
                    fryZoneScript.SetHeated(val);
                }
            }
        }
    }
}
