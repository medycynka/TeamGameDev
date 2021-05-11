using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace SzymonPeszek.Npc.DialogueSystem.DialogueGraph
{
    public class DialogueGraph : EditorWindow
    {
        private DialogueGraphView _graphView;
        private DialogueContainer _dialogueContainer;
        private string _fileName = "New Narrative";
        
        [MenuItem("Dialogue/Dialogue Graph")]
        public static void OpenDialogueGraphWindow()
        {
            DialogueGraph window = GetWindow<DialogueGraph>();
            window.titleContent = new GUIContent("Dialogue Graph");
        }

        private void OnEnable()
        {
            ConstructGraphView();
            GenerateToolbar();
            GenerateMiniMap();
            GenerateBlackBoard();
        }
        
        private void OnDisable()
        {
            rootVisualElement.Remove(_graphView);
        }
        
        private void ConstructGraphView()
        {
            _graphView = new DialogueGraphView(this)
            {
                name = "Dialogue Graph"
            };
            
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void GenerateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            
            TextField fileNameTextField = new TextField("File Name:");
            fileNameTextField.SetValueWithoutNotify(_fileName);
            fileNameTextField.MarkDirtyRepaint();
            fileNameTextField.RegisterValueChangedCallback(evt => _fileName = evt.newValue);
            toolbar.Add(fileNameTextField);
            
            toolbar.Add(new Button(() => RequestDataOperation(true)){text = "Save Data"});
            toolbar.Add(new Button(() => RequestDataOperation(false)){text = "Load Data"});
            
            // Button nodeCreateButton = new Button(() =>
            // {
            //     _graphView.CreateNewDialogueNode("DialogueNode");
            // });
            // nodeCreateButton.text = "Create Node";
            // toolbar.Add(nodeCreateButton);
            
            rootVisualElement.Add(toolbar);
        }

        private void GenerateMiniMap()
        {
            MiniMap miniMap = new MiniMap
            {
                anchored = true
            };
            
            miniMap.SetPosition(new Rect(10, 30, 200, 140));
            _graphView.Add(miniMap);
        }

        private void RequestDataOperation(bool save)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
                
                return;
            }
            
            GraphSaveUtility operationUtility = GraphSaveUtility.GetInstance(_graphView);

            if (save)
            {
                operationUtility.SaveGraph(_fileName);
            }
            else
            {
                operationUtility.LoadGraph(_fileName);
            }
        }
        
        private void GenerateBlackBoard()
        {
            Blackboard blackboard = new Blackboard(_graphView);
            blackboard.Add(new BlackboardSection {title = "Exposed Variables"});
            blackboard.addItemRequested = _blackboard =>
            {
                _graphView.AddPropertyToBlackBoard(ExposedProperty.CreateInstance(), false);
            };
            
            blackboard.editTextRequested = (_blackboard, element, newValue) =>
            {
                string oldPropertyName = ((BlackboardField) element).text;
                
                if (_graphView.exposedProperties.Any(x => x.propertyName == newValue))
                {
                    EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one.",
                        "OK");
                    
                    return;
                }

                int targetIndex = _graphView.exposedProperties.FindIndex(x => x.propertyName == oldPropertyName);
                _graphView.exposedProperties[targetIndex].propertyName = newValue;
                ((BlackboardField) element).text = newValue;
            };
            
            blackboard.SetPosition(new Rect(10,200,200,300));
            _graphView.Add(blackboard);
            _graphView.blackboard = blackboard;
        }
    }
}