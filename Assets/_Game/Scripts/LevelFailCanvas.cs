using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class LevelFailCanvas : MonoBehaviour {
   [ SerializeField ] private Canvas canvas;
   private bool rewindRequestedRecently;
   private float requestCooldown = 1f;
   private IGameEvents gameEvents;
   [ Inject ]
   public void Construct( IGameEvents gameEvents ) {
      this.gameEvents = gameEvents;
      this.gameEvents.OnLevelFailed += ShowCanvas;
      this.gameEvents.OnLevelRewind += HideCanvas;
   }

   private void OnDestroy() {
      if(gameEvents != null){
         this.gameEvents.OnLevelFailed -= ShowCanvas;
         this.gameEvents.OnLevelRewind -= HideCanvas;
      }
   }


   private void ShowCanvas() {
      canvas.enabled = true;
   }

   private void HideCanvas() {
      canvas.enabled = false;
   }

   private void Update() {
      if ( rewindRequestedRecently ) {
         return;
      }
      if ( Input.GetKeyDown( KeyCode.R )  && canvas.enabled) {
         RequestNextLevel();
      }
   }

   private async void RequestNextLevel() {
      if ( rewindRequestedRecently ) {
         return;
      }
      rewindRequestedRecently = true;
      this.gameEvents.OnRewindRequested?.Invoke();
      await UniTask.Delay( TimeSpan.FromSeconds( requestCooldown ), ignoreTimeScale: false );
      rewindRequestedRecently = false;
   }

}
