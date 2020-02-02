using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggCrackScript : MonoBehaviour
{
    public GameObject wholeEgg;
    public GameObject halfEgg;
    public GameObject eggContents;

    public int numberOfHitsToCrack = 3;
    bool hasCracked = false;

    // Start is called before the first frame update
    void Start()
    {
        wholeEgg.SetActive(true);
        halfEgg.SetActive(false);

        hasCracked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CrackEgg()
    {
        Instantiate(eggContents, transform.position, Quaternion.identity);
        wholeEgg.SetActive(false);
        halfEgg.SetActive(true);
		GetComponent<AudioSource>().Play();
		GameObject particle = SmithysKitchen.EmitParticleImpact( transform.position );
		particle.transform.localScale = Vector3.one * 0.25f;

        hasCracked = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasCracked)
            return;

        numberOfHitsToCrack -= 1;

        if (numberOfHitsToCrack <= 0)
        {
            CrackEgg();
        }
    }
}
