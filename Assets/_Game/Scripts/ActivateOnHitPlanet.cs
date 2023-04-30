using UnityEngine;
using VContainer;

public class ActivateOnHitPlanet : MonoBehaviour {

    [ SerializeField ] private GameObject[] thingsToActivate;
    private bool activated;
    private LayerMasks layerMasks;
    
    [ Inject ]
    public void Construct( LayerMasks layerMasks ) {
        this.layerMasks = layerMasks;
    }

    private void OnCollisionEnter2D( Collision2D other ) {
        if ( activated ) {
            return;
        }
        if ( other.gameObject.layer == layerMasks.defaultPackageLayer ) {
            activated = true;
            foreach ( GameObject thing in thingsToActivate ) {
                thing.SetActive( true );
            }
        }
    }

}
