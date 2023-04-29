using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class LevelCompleteCanvas : MonoBehaviour {

    [ SerializeField ] private Canvas winCanvas;
    private bool nextLevelRequestedRecently;
    private float requestCooldown = 1f;
    private IGameEvents gameEvents;
    [ Inject ]
    public void Construct( IGameEvents gameEvents ) {
        this.gameEvents = gameEvents;
        this.gameEvents.OnLevelCompleted += ShowCanvas;
        this.gameEvents.OnNextLevelRequested += HideCanvas;
    }

    private void OnDestroy() {
        if(gameEvents != null){
            this.gameEvents.OnLevelCompleted -= ShowCanvas;
            this.gameEvents.OnNextLevelRequested -= HideCanvas;
        }
    }


    private void ShowCanvas() {
        winCanvas.enabled = true;
    }

    private void HideCanvas() {
        winCanvas.enabled = false;
    }

    private void Update() {
        if ( nextLevelRequestedRecently ) {
            return;
        }
        if ( Input.GetKeyDown( KeyCode.Space )  && winCanvas.enabled) {
            RequestNextLevel();
        }
    }

    private async void RequestNextLevel() {
        if ( nextLevelRequestedRecently ) {
            return;
        }
        nextLevelRequestedRecently = true;
        this.gameEvents.OnNextLevelRequested?.Invoke();
        await UniTask.Delay( TimeSpan.FromSeconds( requestCooldown ), ignoreTimeScale: false );
        nextLevelRequestedRecently = false;
    }

}
