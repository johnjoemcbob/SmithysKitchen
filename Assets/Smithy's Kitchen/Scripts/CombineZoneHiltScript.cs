using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineZoneHiltScript : MonoBehaviour
{
    public GameObject highlightMesh;
    public GameObject hiltMesh;
    public GameObject snappedItemList;

    MeshRenderer hiltRenderer;

    bool isComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        hiltRenderer = hiltMesh.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Snapped()
    {
        //Change hilt colour before activating
        if (snappedItemList)
        {
            Zinnia.Data.Collection.List.GameObjectObservableList list = snappedItemList.GetComponent<Zinnia.Data.Collection.List.GameObjectObservableList>();

            if (list)
            {
                GameObject cube = list.SubscribableElements[0];

                FryableCubeScript fryableCubeScript = cube.GetComponent<FryableCubeScript>();
                if (fryableCubeScript != null)
                {
                    hiltRenderer.material.color = fryableCubeScript.GetAverageColour();
                }

                Destroy(cube);
                list.Clear();
            }
        }

        highlightMesh.SetActive(false);
        hiltMesh.SetActive(true);

		// Matthew play party
		FindObjectOfType<CombineZoneScript>().Complete();

		isComplete = true;
    }

    public bool IsComplete()
    {
        return isComplete;
    }
}
