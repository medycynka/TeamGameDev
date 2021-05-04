using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SzymonPeszek.Npc.DialogueSystem.Runtime;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;


namespace SzymonPeszek.Npc.DialogueSystem.DialogueGraph
{
    public class GraphSaveUtility
    {
        private DialogueGraphView _targetGraphView;
        private DialogueContainer _dialogueContainer;
        private List<Edge> edges => _targetGraphView.edges.ToList();
        private List<DialogueNode> nodes => _targetGraphView.nodes.ToList().Cast<DialogueNode>().ToList();
        private List<Group> commentBlocks =>
            _targetGraphView.graphElements.ToList().Where(x => x is Group).Cast<Group>().ToList();

        public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
        {
            return new GraphSaveUtility
            {
                _targetGraphView = targetGraphView
            };
        }

        public void SaveGraph(string fileName)
        {
            var dialogueContainerObject = ScriptableObject.CreateInstance<DialogueContainer>();
            if (!SaveNodes(fileName, dialogueContainerObject)) return;
            SaveExposedProperties(dialogueContainerObject);
            SaveCommentBlocks(dialogueContainerObject);

            if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/Resources/DialogueGraphFiles"))
            {
                AssetDatabase.CreateFolder("Assets/Resources", "DialogueGraphFiles");
            }
            
            if (!AssetDatabase.IsValidFolder("Assets/Resources/DialogueGraphFiles/DialoguePrefabs"))
            {
                AssetDatabase.CreateFolder("Assets/Resources/DialogueGraphFiles", "DialoguePrefabs");
            }

            UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/DialogueGraphFiles/DialoguePrefabs/{fileName}.asset", typeof(DialogueContainer));

            if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
			{
                AssetDatabase.CreateAsset(dialogueContainerObject, $"Assets/Resources/DialogueGraphFiles/DialoguePrefabs/{fileName}.asset");
            }
            else 
			{
                DialogueContainer container = loadedAsset as DialogueContainer;
                container.nodeLinks = dialogueContainerObject.nodeLinks;
                container.dialogueNodeData = dialogueContainerObject.dialogueNodeData;
                container.exposedProperties = dialogueContainerObject.exposedProperties;
                container.commentBlockData = dialogueContainerObject.commentBlockData;
                EditorUtility.SetDirty(container);
            }

            AssetDatabase.SaveAssets();
        }

        private bool SaveNodes(string fileName, DialogueContainer dialogueContainerObject)
        {
            if (!edges.Any())
            {
                return false;
            }

            Edge[] connectedSockets = edges.Where(x => x.input.node != null).ToArray();
            
            for (var i = 0; i < connectedSockets.Count(); i++)
            {
                DialogueNode outputNode = (connectedSockets[i].output.node as DialogueNode);
                DialogueNode inputNode = (connectedSockets[i].input.node as DialogueNode);
                dialogueContainerObject.nodeLinks.Add(new NodeLinkData
                {
                    baseNodeGuid = outputNode.guID,
                    portName = connectedSockets[i].output.portName,
                    targetNodeGuid = inputNode.guID
                });
            }

            foreach (var node in nodes.Where(node => !node.entryPoint))
            {
                dialogueContainerObject.dialogueNodeData.Add(new DialogueNodeData
                {
                    nodeGuid = node.guID,
                    dialogueText = node.dialogueText,
                    nodePosition = node.GetPosition().position,
                    isQuestGiver = node.isQuestGiver,
                    isQuestCompleter = node.isQuestCompleter
                });
            }

            return true;
        }

        private void SaveExposedProperties(DialogueContainer dialogueContainer)
        {
            dialogueContainer.exposedProperties.Clear();
            dialogueContainer.exposedProperties.AddRange(_targetGraphView.exposedProperties);
        }

        private void SaveCommentBlocks(DialogueContainer dialogueContainer)
        {
            foreach (var block in commentBlocks)
            {
                List<string> nodes = block.containedElements.Where(x => x is DialogueNode).Cast<DialogueNode>().Select(x => x.guID)
                    .ToList();

                dialogueContainer.commentBlockData.Add(new CommentBlockData
                {
                    childNodes = nodes,
                    title = block.title,
                    position = block.GetPosition().position
                });
            }
        }

        public void LoadGraph(string fileName)
        {
            _dialogueContainer = Resources.Load<DialogueContainer>($"DialogueGraphFiles/DialoguePrefabs/{fileName}");
            
            if (_dialogueContainer == null)
            {
                EditorUtility.DisplayDialog("File Not Found", "Target Narrative Data does not exist!", "OK");
                
                return;
            }

            ClearGraph();
            GenerateDialogueNodes();
            ConnectDialogueNodes();
            AddExposedProperties();
            GenerateCommentBlocks();
        }

        /// <summary>
        /// Set Entry point GUID then Get All Nodes, remove all and their edges. Leave only the entrypoint node. (Remove its edge too)
        /// </summary>
        private void ClearGraph()
        {
            nodes.Find(x => x.entryPoint).guID = _dialogueContainer.nodeLinks[0].baseNodeGuid;
            
            foreach (var perNode in nodes)
            {
                if (perNode.entryPoint)
                {
                    continue;
                }

                edges.Where(x => x.input.node == perNode).ToList()
                    .ForEach(edge => _targetGraphView.RemoveElement(edge));
                _targetGraphView.RemoveElement(perNode);
            }
        }

        /// <summary>
        /// Create All serialized nodes and assign their guid and dialogue text to them
        /// </summary>
        private void GenerateDialogueNodes()
        {
            foreach (var perNode in _dialogueContainer.dialogueNodeData)
            {
                var tempNode = _targetGraphView.CreateNode(perNode.dialogueText, Vector2.zero, perNode.isQuestGiver, 
                    perNode.isQuestCompleter);
                tempNode.guID = perNode.nodeGuid;
                _targetGraphView.AddElement(tempNode);

                var nodePorts = _dialogueContainer.nodeLinks.Where(x => x.baseNodeGuid == perNode.nodeGuid).ToList();
                nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.portName));
            }
        }

        private void ConnectDialogueNodes()
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                int k = i; //Prevent access to modified closure
                List<NodeLinkData> connections = _dialogueContainer.nodeLinks.Where(x => x.baseNodeGuid == nodes[k].guID).ToList();
                for (var j = 0; j < connections.Count(); j++)
                {
                    string targetNodeGUID = connections[j].targetNodeGuid;
                    DialogueNode targetNode = nodes.First(x => x.guID == targetNodeGUID);
                    
                    LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);

                    targetNode.SetPosition(new Rect(
                        _dialogueContainer.dialogueNodeData.First(x => x.nodeGuid == targetNodeGUID).nodePosition,
                        _targetGraphView.defaultNodeSize));
                }
            }
        }

        private void LinkNodesTogether(Port outputSocket, Port inputSocket)
        {
            var tempEdge = new Edge()
            {
                output = outputSocket,
                input = inputSocket
            };
            
            tempEdge?.input.Connect(tempEdge);
            tempEdge?.output.Connect(tempEdge);
            _targetGraphView.Add(tempEdge);
        }

        private void AddExposedProperties()
        {
            _targetGraphView.ClearBlackBoardAndExposedProperties();
            
            foreach (ExposedProperty exposedProperty in _dialogueContainer.exposedProperties)
            {
                _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
            }
        }

        private void GenerateCommentBlocks()
        {
            foreach (Group commentBlock in commentBlocks)
            {
                _targetGraphView.RemoveElement(commentBlock);
            }

            foreach (CommentBlockData commentBlockData in _dialogueContainer.commentBlockData)
            {
               Group block = _targetGraphView.CreateCommentBlock(new Rect(commentBlockData.position, _targetGraphView.defaultCommentBlockSize),
                    commentBlockData);
               block.AddElements(nodes.Where(x=>commentBlockData.childNodes.Contains(x.guID)));
            }
        }
    }
}