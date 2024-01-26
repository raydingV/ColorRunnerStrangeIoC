using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Rich.Base.Runtime.Concrete.Injectable.Mediator;
using Runtime.Controller;
using Runtime.Model.Player;
using Runtime.Signals;
using Runtime.Views.Player;
using Runtime.Views.Pool;
using UnityEngine;

namespace Runtime.Mediators.Player
{
    public class PlayerMediator : MediatorLite
    {
        [Inject] public PlayerView View { get; set; }
        [Inject] public IPlayerModel Model { get; set; }
        [Inject] public InputSignals InputSignals { get; set; }
        [Inject] public PlayerSignals PlayerSignals { get; set; }
        [Inject] public CoreGameSignals CoreGameSignals { get; set; }

        [Inject] public UISignals UISignals { get; set; }
        
        private List<GameObject> stackedObjects = new List<GameObject>();

        public override void OnRegister()
        {
            base.OnRegister();
            InputSignals.onInputDragged.AddListener(View.OnInputDragged);
            InputSignals.onInputReleased.AddListener(View.OnInputReleased);
            InputSignals.onInputTaken.AddListener(View.OnInputTaken);
            PlayerSignals.onStageAreaSuccessful.AddListener(StageAreaSuccessful);
            UISignals.onPlay.AddListener(OnPlay);

            View.onReset += OnReset;
            View.onStageAreaEntered += OnStageAreaEntered;
            View.onFinishAreaEntered += OnFinishAreaEntered;
            View.onColorAreaEntered += OnColorAreaEntered;
            View.onColorPlaneEntered += OnColorPlaneEntered;
            View.stackTrigger += stackTrigger;
        }


        private void OnPlay()
        {
            View.IsReadyToPlay(true);
        }

        private void OnStageAreaEntered(Transform view, Transform other)
        {
            PlayerSignals.onForceCommand.Dispatch(view, Model.PlayerData.PlayerData.ForceData);
            InputSignals.onDisableInput.Dispatch();

            DOVirtual.DelayedCall(3, () =>
            {
                bool result = other.GetComponentInChildren<PoolControllerView>().OnGetPoolResult(Model.StageValue);

                Debug.Log(result);
                if (result != null && (bool)result)
                {
                    Debug.Log("Result True");
                    PlayerSignals.onStageAreaSuccessful.Dispatch(Model.StageValue);
                    InputSignals.onEnableInput.Dispatch();
                }
                else
                {
                    CoreGameSignals.onLevelFailed.Dispatch();
                    Debug.Log("TRYTRY");
                }
            });
        }

        private void OnColorAreaEntered(Transform view, Collider other, Material material)
        {
            if (material != null)
            {
                Material otherMaterial = other.GetComponent<Renderer>().material;
                
                material.color = otherMaterial.color;
            }
        }
        
        private void stackTrigger(Transform view, Collider other)
        {
            stackedObjects.Add(other.gameObject);

            Vector3 stackedPosition = new Vector3(view.transform.position.x, view.transform.position.y - 0.58f, view.transform.position.z - 1.5f * stackedObjects.Count);

            other.transform.parent = view.transform;
            other.transform.position = stackedPosition;
            
            Debug.Log("Score: " + stackedObjects.Count);
        }

        private void OnColorPlaneEntered(Transform view, Collider other, Material material)
        {
            if (material != null)
            {
                Material otherMaterial = other.GetComponent<Renderer>().material;
                
                if (material.color != otherMaterial.color && stackedObjects.Count >= 1)
                {
                   GameObject.Destroy(stackedObjects.Last());

                   stackedObjects.Remove(stackedObjects.Last());
                   
                   Debug.Log("Score: " + stackedObjects.Count);
                }
            }
        }
        
        private void StageAreaSuccessful(byte obj)
        {
            Model.StageValue++;
            View.IsReadyToPlay(true);
            View.ShowUpText();
            View.PlayConfettiParticle();
            View.ScaleUpPlayer();
        }

        private void OnFinishAreaEntered()
        {
            CoreGameSignals.onLevelSuccessful?.Dispatch();
        }

        private void OnReset()
        {
            Model.StageValue = 0;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            InputSignals.onInputDragged.RemoveListener(View.OnInputDragged);
            InputSignals.onInputReleased.RemoveListener(View.OnInputReleased);
            InputSignals.onInputTaken.RemoveListener(View.OnInputTaken);
            PlayerSignals.onStageAreaSuccessful.RemoveListener(StageAreaSuccessful);
            UISignals.onPlay.RemoveListener(OnPlay);

            View.onReset -= OnReset;
            View.onStageAreaEntered -= OnStageAreaEntered;
            View.onFinishAreaEntered -= OnFinishAreaEntered;
            View.onColorAreaEntered -= OnColorAreaEntered;
            View.onColorPlaneEntered -= OnColorPlaneEntered;
            View.stackTrigger -= stackTrigger;
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            View.SetPlayerData(Model.PlayerData.PlayerData);
        }
    }
}