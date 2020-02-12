using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpiceReactScript : MonoBehaviour
{
    List<ParticleCollisionEvent> collisionEvents;


    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        ParticleSystem ps = other.GetComponent<ParticleSystem>();

        if (ps)
        {
            int numCollisionEvents = ps.GetCollisionEvents(this.gameObject, collisionEvents);
            Color particleColour = ps.main.startColor.color;

            foreach (ParticleCollisionEvent colEvent in collisionEvents)
            {
                if ((colEvent.colliderComponent != null) && (colEvent.colliderComponent.gameObject != null))
                {
                    if (colEvent.colliderComponent.gameObject.GetComponentInParent<InteractableBladeTag>())
                    {
                        GameObject cubeSide = colEvent.colliderComponent.gameObject;
                        MeshRenderer cubeRenderer = cubeSide.transform.parent.GetComponentInChildren<MeshRenderer>( false );

						for ( int mat = 0; mat < cubeRenderer.materials.Length; mat++ )
						{
							Color col = particleColour;
							if ( cubeRenderer.materials[mat].name.Contains( "Light" ) )
							{
								cubeRenderer.materials[mat].color = Color.Lerp( cubeRenderer.materials[mat].color, col, 0.1f );
							}
							else
							{
								col *= Color.grey;
								cubeRenderer.materials[mat].color = Color.Lerp( cubeRenderer.materials[mat].color, col, 0.1f );
							}
						}
                    }
                }
            }
        }
    }
}
