using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using SzymonPeszek.PlayerScripts.Controller;


namespace SzymonPeszek.Misc
{
    public class InputRebinder : MonoBehaviour
    {
        public List<InputActionReference> inputsToRebind = new List<InputActionReference>();
        public List<TextMeshProUGUI> bindingNameText = new List<TextMeshProUGUI>();
        public List<GameObject> startBinding = new List<GameObject>();
        public List<GameObject> waitForBinding = new List<GameObject>();
        public InputHandler inputHandler;
        
        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;

        private const string RebindKey = "InputRebind";

        private void Start()
        {
            string rebind = PlayerPrefs.GetString(RebindKey, string.Empty);

            if (string.IsNullOrEmpty(rebind))
            {
                return;
            }
            
            inputHandler.playerControls.asset.LoadFromJson(rebind);

            for (int i = 0; i < inputsToRebind.Count; i++)
            {
                int bindingId = inputsToRebind[i].action.GetBindingIndexForControl(inputsToRebind[i].action.controls[0]);

                bindingNameText[i].text = InputControlPath.ToHumanReadableString(
                    inputsToRebind[i].action.bindings[bindingId].effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
        }

        public void Save()
        {
            string rebind = inputHandler.playerControls.asset.ToJson();
            PlayerPrefs.SetString(RebindKey, rebind);
        }

        public void StartRebinding(int id)
        {
            if (!ResolveBindingAction(out InputAction action, out int bindingIndex, id))
            {
                return;
            }
            
            _rebindingOperation?.Cancel();
            
            for (int i = 0; i < inputsToRebind.Count; i++)
            {
                if (i != id)
                {
                    startBinding[i].SetActive(false);
                }    
            }
            
            startBinding[id].SetActive(false);
            bindingNameText[id].gameObject.SetActive(false);
            waitForBinding[id].SetActive(true);

            string debugTxt = "Start rebinding action " + action.name + " with binding: " + action.bindings[0] +
                              " and paths:\n" +
                              "path: " + action.bindings[0].path + "\n" +
                              "effective path: " + action.bindings[0].effectivePath + "\n" +
                              "override path: " + action.bindings[0].overridePath + "\n";
            Debug.Log(debugTxt);

            _rebindingOperation = action.PerformInteractiveRebinding(bindingIndex).WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f).OnComplete(op => CompleteRebind(id));
            _rebindingOperation.Start();
        }

        private bool ResolveBindingAction(out InputAction action, out int bindingIndex, int id)
        {
            bindingIndex = -1;

            action = inputsToRebind[id]?.action;
            
            if (action == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(inputsToRebind[id].action.bindings[0].id.ToString()))
            {
                return false;
            }

            Guid bindingId = new Guid(inputsToRebind[id].action.bindings[0].id.ToString());
            bindingIndex = action.bindings.IndexOf(x => x.id == bindingId);
            
            if (bindingIndex == -1)
            {
                Debug.LogError($"Cannot find binding with ID '{bindingId}' on '{action}'", this);
                return false;
            }

            return true;
        }

        private void CompleteRebind(int id)
        {
            _rebindingOperation.Dispose();
            _rebindingOperation = null;

            string debugTxt = "finished rebinding action " + inputsToRebind[id].action.name + " with binding: " +
                              inputsToRebind[id].action.bindings[0] +
                              " and paths:\n" +
                              "path: " + inputsToRebind[id].action.bindings[0].path + "\n" +
                              "effective path: " + inputsToRebind[id].action.bindings[0].effectivePath + "\n" +
                              "override path: " + inputsToRebind[id].action.bindings[0].overridePath + "\n";
            Debug.Log(debugTxt);
            
            int bindingId = inputsToRebind[id].action.GetBindingIndexForControl(inputsToRebind[id].action.controls[0]);

            bindingNameText[id].text = InputControlPath.ToHumanReadableString(
                inputsToRebind[id].action.bindings[bindingId].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);

            startBinding[id].SetActive(true);
            bindingNameText[id].gameObject.SetActive(true);
            waitForBinding[id].SetActive(false);
            
            for (int i = 0; i < inputsToRebind.Count; i++)
            {
                if (i != id)
                {
                    startBinding[i].SetActive(true);
                }    
            }
        }
    }
}