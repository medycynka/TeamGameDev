using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace SzymonPeszek.Npc.DialogueSystem.DialogueGraph
{
    public class DialogueGraphView : GraphView
    {
        public readonly Vector2 defaultNodeSize = new Vector2(150, 200);
        public readonly Vector2 defaultCommentBlockSize = new Vector2(300, 200);
        public DialogueNode entryPointNode;
        public Blackboard blackboard = new Blackboard();
        public List<ExposedProperty> exposedProperties { get; private set; } = new List<ExposedProperty>();
        private NodeSearchWindow _searchWindow;
        
        public DialogueGraphView(DialogueGraph editorWindow)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraphFiles/DialogueGraph"));
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground grid = new GridBackground();
            Insert(0, grid);
            grid.StretchToParentSize();

            AddElement(GetEntryPointNodeInstance());
            AddSearchWindow(editorWindow);
        }
        
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (startPort != port && startPort.node != port.node)
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }
        
        private void AddSearchWindow(DialogueGraph editorWindow)
        {
            _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
            _searchWindow.Configure(editorWindow, this);
            nodeCreationRequest = context =>
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }


        public void ClearBlackBoardAndExposedProperties()
        {
            exposedProperties.Clear();
            blackboard.Clear();
        }

        public Group CreateCommentBlock(Rect rect, CommentBlockData commentBlockData = null)
        {
            if (commentBlockData == null)
            {
                commentBlockData = new CommentBlockData();
            }
            
            Group group = new Group
            {
                autoUpdateGeometry = true,
                title = commentBlockData.title
            };
            
            AddElement(group);
            group.SetPosition(rect);
            
            return group;
        }

        public void AddPropertyToBlackBoard(ExposedProperty property, bool loadMode = false)
        {
            var localPropertyName = property.propertyName;
            var localPropertyValue = property.propertyValue;
            
            if (!loadMode)
            {
                while (exposedProperties.Any(x => x.propertyName == localPropertyName))
                {
                    localPropertyName = $"{localPropertyName}(1)";
                }
            }

            var item = ExposedProperty.CreateInstance();
            item.propertyName = localPropertyName;
            item.propertyValue = localPropertyValue;
            exposedProperties.Add(item);

            VisualElement container = new VisualElement();
            BlackboardField field = new BlackboardField {text = localPropertyName, typeText = "string"};
            container.Add(field);

            TextField propertyValueTextField = new TextField("Value:")
            {
                value = localPropertyValue
            };
            
            propertyValueTextField.RegisterValueChangedCallback(evt =>
            {
                int index = exposedProperties.FindIndex(x => x.propertyName == item.propertyName);
                exposedProperties[index].propertyValue = evt.newValue;
            });
            
            BlackboardRow sa = new BlackboardRow(field, propertyValueTextField);
            container.Add(sa);
            blackboard.Add(container);
        }

        private Port GeneratePort(DialogueNode node, Direction portDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

        public void CreateNewDialogueNode(string nodeName, Vector2 mousePosition)
        {
            AddElement(CreateNode(nodeName, mousePosition));
        }

        public DialogueNode CreateNode(string nodeName, Vector2 mousePosition, bool questGiver = false, 
            bool questCompleter = false, bool isExit = false, bool isEnding = false, bool isItem = false)
        {
            DialogueNode newDialogueNode = new DialogueNode
            {
                title = nodeName,
                guID = Guid.NewGuid().ToString(),
                dialogueText = nodeName,
                isQuestGiver = questGiver,
                isQuestCompleter = questCompleter,
                isExitNode = isExit,
                isEndingDialogueNode = isEnding,
                isItemGiver = isItem
            };
            
            newDialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraphFiles/Node"));

            Port inputPort = GeneratePort(newDialogueNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            newDialogueNode.inputContainer.Add(inputPort);
            
            newDialogueNode.RefreshExpandedState();
            newDialogueNode.RefreshPorts();
            newDialogueNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

            TextField textField = new TextField("");
            textField.RegisterValueChangedCallback(evt =>
            {
                newDialogueNode.dialogueText = evt.newValue;
                newDialogueNode.title = evt.newValue;
            });
            textField.SetValueWithoutNotify(newDialogueNode.title);
            
            newDialogueNode.mainContainer.Add(textField);
            
            Toggle isExitNode = new Toggle();
            isExitNode.RegisterValueChangedCallback(evt => newDialogueNode.isExitNode = evt.newValue);
            isExitNode.SetValueWithoutNotify(newDialogueNode.isExitNode);
            isExitNode.text = "Exit";
            newDialogueNode.titleContainer.Add(isExitNode);
            
            Toggle isQuestGiver = new Toggle();
            isQuestGiver.RegisterValueChangedCallback(evt => newDialogueNode.isQuestGiver = evt.newValue);
            isQuestGiver.SetValueWithoutNotify(newDialogueNode.isQuestGiver);
            isQuestGiver.text = "Giver";
            newDialogueNode.titleContainer.Add(isQuestGiver);
                
            Toggle isQuestCompleter = new Toggle();
            isQuestCompleter.RegisterValueChangedCallback(evt => newDialogueNode.isQuestCompleter = evt.newValue);
            isQuestCompleter.SetValueWithoutNotify(newDialogueNode.isQuestCompleter);
            isQuestCompleter.text = "Completer";
            newDialogueNode.titleContainer.Add(isQuestCompleter);
            
            Toggle isDialogueEnder = new Toggle();
            isDialogueEnder.RegisterValueChangedCallback(evt => newDialogueNode.isEndingDialogueNode = evt.newValue);
            isDialogueEnder.SetValueWithoutNotify(newDialogueNode.isEndingDialogueNode);
            isDialogueEnder.text = "Ender";
            newDialogueNode.titleContainer.Add(isDialogueEnder);
            
            Toggle isItemGiver = new Toggle();
            isItemGiver.RegisterValueChangedCallback(evt => newDialogueNode.isItemGiver = evt.newValue);
            isItemGiver.SetValueWithoutNotify(newDialogueNode.isItemGiver);
            isItemGiver.text = "Item";
            newDialogueNode.titleContainer.Add(isItemGiver);

            Button button = new Button(() => { AddChoicePort(newDialogueNode); });
            button.text = "Add Choice";
            newDialogueNode.titleContainer.Add(button);
            
            return newDialogueNode;
        }

        public void AddChoicePort(DialogueNode dialogueNode, string overriddenPortName = "")
        {
            int outputPortCount = dialogueNode.outputContainer.Query("connector").ToList().Count;
            
            if (outputPortCount < 3)
            {
                Port generatedPort = GeneratePort(dialogueNode, Direction.Output);
                Label oldLabel = generatedPort.contentContainer.Q<Label>("type");
                generatedPort.contentContainer.Remove(oldLabel);
                string choicePortName = string.IsNullOrEmpty(overriddenPortName)
                    ? $"Choice {outputPortCount + 1}"
                    : overriddenPortName;
                
                generatedPort.contentContainer.Add(new Label("            "));

                TextField textField = new TextField
                {
                    name = string.Empty,
                    value = choicePortName
                };
                textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
                generatedPort.contentContainer.Add(textField);

                Button deleteButton = new Button(() => RemovePort(dialogueNode, generatedPort))
                {
                    text = "X"
                };
                generatedPort.contentContainer.Add(deleteButton);

                generatedPort.portName = choicePortName;
                dialogueNode.outputContainer.Add(generatedPort);
                dialogueNode.RefreshPorts();
                dialogueNode.RefreshExpandedState();
            }
        }

        private void RemovePort(DialogueNode node, Port socket)
        {
            var targetEdge = edges.ToList().Where(p =>
                p.output.portName == socket.portName && p.output.node == socket.node);

            if (targetEdge.Any())
            {
                Edge edge = targetEdge.First();
                edge.input.Disconnect(edge);
                RemoveElement(targetEdge.First());
            }

            node.outputContainer.Remove(socket);
            node.RefreshPorts();
            node.RefreshExpandedState();
        }
        
        private Port GetPortInstance(DialogueNode node, Direction nodeDirection,
            Port.Capacity capacity = Port.Capacity.Single)
        {
            return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
        }
        

        private DialogueNode GetEntryPointNodeInstance()
        {
            var nodeCache = new DialogueNode()
            {
                title = "START",
                guID = Guid.NewGuid().ToString(),
                dialogueText = "ENTRYPOINT",
                entryPoint = true
            };

            var generatedPort = GetPortInstance(nodeCache, Direction.Output);
            generatedPort.portName = "Next";
            nodeCache.outputContainer.Add(generatedPort);

            nodeCache.capabilities &= ~Capabilities.Movable;
            nodeCache.capabilities &= ~Capabilities.Deletable;

            nodeCache.RefreshExpandedState();
            nodeCache.RefreshPorts();
            nodeCache.SetPosition(new Rect(250, 200, 100, 150));
            
            return nodeCache;
        }
    }
}