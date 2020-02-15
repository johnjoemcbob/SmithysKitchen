using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarScript : MonoBehaviour
{
    public Transform ingredientsParent_Large;
    public float lerpSpeed = 5.0f;

    List<Transform> possibleIngredients = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {

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

        Debug.Log("crush the food with the pestle.");
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
            ingredientsParent_Large.GetComponent<AudioSource>().pitch = Mathf.Lerp(2, 3, parent.GetSiblingIndex() / ingredientsParent_Large.childCount);
            ingredientsParent_Large.GetComponent<AudioSource>().Play();

            possibleIngredients.Add(parent);
        }

        // Remove old grabbable objects
        Transform oldroot = other.attachedRigidbody.transform;
        Destroy(oldroot.gameObject);
    }

}
