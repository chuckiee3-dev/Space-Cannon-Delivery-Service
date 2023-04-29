using DG.Tweening;
using UnityEngine;
using VContainer;

[RequireComponent(typeof(Rigidbody2D))]
public class SpacePackage : MonoBehaviour {

    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Collider2D c2d;

    private LayerMasks layerMasks;

    [ Inject ]
    public void Construct( LayerMasks layerMasks ) {
        this.layerMasks = layerMasks;
    }
    public void Send( Vector2 force ) {
        rb2d.AddForce( force, ForceMode2D.Impulse );
    }

    public void CollectedBy( Vector3 collectTransformPosition ) {
        gameObject.layer = layerMasks.collectedPackageLayer;
        rb2d.velocity = Vector2.zero;
        rb2d.angularVelocity = 0;
        rb2d.isKinematic = true;
        c2d.enabled = false;
        transform.DOMove( collectTransformPosition, .5f ).SetEase( Ease.OutCirc );
    }

}
