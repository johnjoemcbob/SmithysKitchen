using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarScript : MonoBehaviour
{
    public Transform ingredientsParent_Large;
    public Transform ingredientsParent_Small;
    public float lerpSpeed = 5.0f;

    public Transform pourOrigin;

    List<Transform> possibleIngredients = new List<Transform>();

    public AudioClip placeIngredientClip;
    public AudioClip mashIngredientsClip;

    VRTK.Prefabs.Interactions.Interactables.InteractableFacade facade;

    // Start is called before the first frame update
    void Start()
    {
        facade = GetComponent<VRTK.Prefabs.Interactions.Interactables.InteractableFacade>();
    }

    // Update is called once per frame
    void Update()
    {
        //[TODO] ---> Maybe fix this lol
        foreach (Transform ingredient in ingredientsParent_Large)
        {
            if (ingredient.gameObject.activeSelf)
            {
                ingredient.localScale = Vector3.Lerp(ingredient.localScale, new Vector3(0.1f, 0.05f, 0.1f), Time.deltaTime * lerpSpeed);
            }
        }

        //Pouring.
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
        if (!pourOrigin.gameObject.activeSelf)
            return;

        if (possibleIngredients.Count <= 0)
            return;

        Vector3 dir = Vector3.up;
        RaycastHit hit;

        if (Physics.Raycast(pourOrigin.position, -dir, out hit, 50))
        {
            Debug.DrawLine(pourOrigin.position, hit.point);

            //Make sure we're only hitting soup
            SoupPotScript soupScript = hit.collider.gameObject.GetComponentInParent<SoupPotScript>();
            if (soupScript != null)
            {
                //[TODO] ---> A better way of doing this hopefully??? ---- This is not very nice to look at.
                soupScript.AddChunks(possibleIngredients[possibleIngredients.Count - 1].GetComponent<MeshRenderer>().material);
                possibleIngredients.RemoveAt(possibleIngredients.Count - 1);

                //Turn off ingredient???
                foreach (Transform ingredient in ingredientsParent_Small)
                {
                    if (ingredient.gameObject.activeSelf)
                    {
                        ingredient.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pestle")
        {
            PulveriseIngredients();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ingredient")
        {
            TrackIngredient(other);
        }
    }

    void PulveriseIngredients()
    {
        if (possibleIngredients.Count <= 0)
            return;

        //Transfer info from large ingredient to small ingredient
        for (int i = 0; i < ingredientsParent_Large.childCount; i++)
        {
            Transform largeIngredient = ingredientsParent_Large.GetChild(i);

            if (largeIngredient.gameObject.activeSelf)
            {
                Transform smallIngredient = ingredientsParent_Small.GetChild(i);

                // Turn mesh renderer on for this "newly added" ingredient and colour correctly
                foreach(Transform child in smallIngredient)
                {
                    child.GetComponent<MeshRenderer>().material.color = largeIngredient.GetComponent<MeshRenderer>().material.color;
                    child.GetComponent<MeshRenderer>().enabled = true;
                }

                smallIngredient.gameObject.SetActive(true);
                largeIngredient.gameObject.SetActive(false);
                break;
            }
        }

        // Play crush sound
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = mashIngredientsClip;
        GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.25f);
        GetComponent<AudioSource>().Play();
    }


    void TrackIngredient(Collider other)
    {
        // Find any free spots to add to
        Transform parent = null;
        foreach (Transform ingredient in ingredientsParent_Large)
        {
            if (!ingredient.gameObject.activeSelf)
            {
                parent = ingredient;
                parent.gameObject.SetActive(true);
                break;
            }
        }

        // If free spot then add
        if (parent != null)
        {
            // Turn mesh renderer on for this "newly added" ingredient and colour correctly
            parent.GetComponent<MeshRenderer>().material.color = other.GetComponent<MeshRenderer>().material.color;
            parent.GetComponent<MeshRenderer>().enabled = true;
            parent.localScale = Vector3.zero;

            // Play rising pitch sound
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = placeIngredientClip;
            GetComponent<AudioSource>().pitch = Mathf.Lerp(2, 3, parent.GetSiblingIndex() / ingredientsParent_Large.childCount);
            GetComponent<AudioSource>().Play();

            possibleIngredients.Add(parent);
        }

        // Remove old grabbable objects
        Transform oldroot = other.attachedRigidbody.transform;
        Destroy(oldroot.gameObject);
    }


    void StartPouring()
    {
        pourOrigin.gameObject.SetActive(true);
    }

    void StopPouring()
    {
        pourOrigin.gameObject.SetActive(false);
    }

}
