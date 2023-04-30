using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour {

    [ SerializeField ] private SpriteRenderer sr;
    [ SerializeField ] private bool shouldRotate;
    [ SerializeField ] private Vector2 range = new Vector2( -15,15 );

    private float rotationSpeed;
    public void SetVisual( Sprite sprite ) {
        sr.sprite = sprite;
        rotationSpeed = Random.Range( range.x, range.y );
    }

    private void Update() {
        if ( !shouldRotate ) {
            return;
        }
        sr.transform.Rotate( 0,0,rotationSpeed *Time.deltaTime  );
    }

}
