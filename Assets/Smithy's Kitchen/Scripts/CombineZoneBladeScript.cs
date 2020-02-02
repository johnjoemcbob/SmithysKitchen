using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineZoneBladeScript : MonoBehaviour
{
    public GameObject highlightMesh;
    public GameObject snappedItemList;

    bool isComplete = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Snapped()
    {
        highlightMesh.SetActive(false);

        //Change hilt colour before activating
        if (snappedItemList)
        {
            Zinnia.Data.Collection.List.GameObjectObservableList list = snappedItemList.GetComponent<Zinnia.Data.Collection.List.GameObjectObservableList>();

            if (list)
            {
                GameObject cube = list.SubscribableElements[0];

                GameObject grabLogic = cube.transform.Find("InteractionLogic").gameObject;
                grabLogic.SetActive(false);
            }
        }

        isComplete = true;
    }

    public bool IsComplete()
    {
        return isComplete;
    }
}
