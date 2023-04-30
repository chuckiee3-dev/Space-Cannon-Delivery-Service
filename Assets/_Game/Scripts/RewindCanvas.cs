using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class RewindCanvas : MonoBehaviour
{
    [ SerializeField ] private Canvas canvas;
    private float requestCooldown = 1f;
    private IGameEvents gameEvents;
    private bool rewindRequestedRecently;

    [ Inject ]
    public void Construct( IGameEvents gameEvents ) {
        this.gameEvents = gameEvents;
    }

    public void Show() {
        canvas.enabled = true;
    }
    public void Hide() {
        canvas.enabled = false;
    }

    private void Update() {
        if ( rewindRequestedRecently ) {
            return;
        }
        if ( Input.GetKeyDown( KeyCode.R )  && canvas.enabled) {
            RequestRewind();
        }
    }

    private async void RequestRewind() {
        if ( rewindRequestedRecently ) {
            return;
        }
        rewindRequestedRecently = true;
        this.gameEvents.OnRewindRequested?.Invoke();
        await UniTask.Delay( TimeSpan.FromSeconds( requestCooldown ), ignoreTimeScale: false );
        rewindRequestedRecently = false;
    }
}
