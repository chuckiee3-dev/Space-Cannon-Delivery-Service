using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [ SerializeField ] private SpriteRenderer sr;

    public void SetVisual( Sprite sprite ) {
        sr.sprite = sprite;
    }

}
