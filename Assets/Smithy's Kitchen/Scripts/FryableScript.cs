using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FryableScript : MonoBehaviour
{
    bool isFrying = false;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public bool IsFrying()
    {
        return isFrying;
    }

    public virtual void StartFry()
    {
        isFrying = true;
    }

    public virtual void StopFry()
    {
        isFrying = false;
    }

    public virtual void Fry()
    {

    }
}
