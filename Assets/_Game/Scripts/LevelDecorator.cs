using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class LevelDecorator : MonoBehaviour {

    [ SerializeField ] private List<Sprite> planetSprites;
    [ SerializeField ] private List<GameObject> bgProps;
    [ SerializeField ] private List<Sprite> bgSprites;
    [ SerializeField ] private Transform maxs;
    [ SerializeField ] private Transform mins;
    private IGameEvents gameEvents;
    private bool shouldDecorate = true;
    private List<GameObject> props;
    [SerializeField] private Vector2Int bgPropsRange = new Vector2Int( 5, 20 );
    [ Inject ]
    public void Construct( IGameEvents gameEvents ) {
        this.gameEvents = gameEvents;
        gameEvents.OnLevelCompleted += LevelCompleted;
        gameEvents.OnLevelLoaded += Decorate;
    }

    private void OnDestroy() {
        if ( gameEvents != null ) {
            gameEvents.OnLevelCompleted -= LevelCompleted;
            gameEvents.OnLevelLoaded -= Decorate;
        }
    }

    private void Decorate() {
        if ( !shouldDecorate ) {
            return;
        }

        Planet[] planets = FindObjectsOfType<Planet>();

        foreach ( Planet planet in planets ) {
            planet.SetVisual( planetSprites[Random.Range( 0, planetSprites.Count )] );
        }
        PutProps();
        shouldDecorate = false;
    }

    private void LevelCompleted() {
        shouldDecorate = true;
    }

    private void PutProps() {
        if ( props == null ) {
            props = new List<GameObject>();
        }

        for ( int i = props.Count - 1; i >= 0; i-- ) {
            Destroy(props[i]);
            props.RemoveAt( i );
        }

        int propAmount = Random.Range( bgPropsRange.x, bgPropsRange.y );
        for ( int i = 0; i < propAmount; i++ ) {
            Vector2 pos = new Vector2( Random.Range( mins.position.x, maxs.position.x ), Random.Range( mins.position.y, maxs.position.y ) );
            GameObject newProp = Instantiate( bgProps[Random.Range( 0, bgProps.Count )], pos, quaternion.identity );
            newProp.transform.Rotate( 0,0, Random.Range( 0,45 ) );
            props.Add( newProp);
        }

    }
}
