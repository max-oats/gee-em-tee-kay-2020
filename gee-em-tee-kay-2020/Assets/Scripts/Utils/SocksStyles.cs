using UnityEngine;
using UnityEditor;

namespace Socks
{
#if UNITY_EDITOR
    public static class Styles
    {
        private static GUIStyle _node;
        public static GUIStyle Node
        {
            get
            {
                if (_node == null)
                {
                    _node = new GUIStyle(EditorStyles.helpBox);
                    _node.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SockEditor/Textures/background.png", typeof(Texture2D));
                    _node.border = new RectOffset(5, 5, 5, 5);
                }
                return _node;
            }
        }

        private static GUIStyle _commandLine;
        public static GUIStyle CommandLine
        {
            get
            {
                if (_commandLine == null)
                {
                    _commandLine = new GUIStyle(EditorStyles.helpBox);
                    _commandLine.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/Editor/DebugSelect.png", typeof(Texture2D));
                    _commandLine.border = new RectOffset(5, 5, 5, 5);
                    _commandLine.padding = new RectOffset(5, 0, 0, 0);
                }
                return _commandLine;
            }
        }

        private static GUIStyle _commandLineUnselected;
        public static GUIStyle CommandLineUnselected
        {
            get
            {
                if (_commandLineUnselected == null)
                {
                    _commandLineUnselected = new GUIStyle(EditorStyles.helpBox);
                    _commandLineUnselected.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Textures/Editor/Debug.png", typeof(Texture2D));
                    _commandLineUnselected.border = new RectOffset(5, 5, 5, 5);
                    _commandLineUnselected.padding = new RectOffset(5, 0, 0, 0);
                }
                return _commandLineUnselected;
            }
        }

        private static GUIStyle _selectedNode;
        public static GUIStyle SelectedNode
        {
            get
            {
                if (_selectedNode == null)
                {
                    _selectedNode = new GUIStyle(EditorStyles.helpBox);
                    _selectedNode.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SockEditor/Textures/selected.png", typeof(Texture2D));
                    _selectedNode.border = new RectOffset(5, 5, 5, 5);
                }
                return _selectedNode;
            }
        }

        private static GUIStyle _bold;
        public static GUIStyle Bold
        {
            get
            {
                if (_bold == null)
                {
                    _bold = new GUIStyle(EditorStyles.boldLabel);
                }
                return _bold;
            }
        }

        private static GUIStyle _helpbox;
        public static GUIStyle Helpbox
        {
            get
            {
                if (_helpbox == null)
                {
                    _helpbox = new GUIStyle(EditorStyles.helpBox);
                    _helpbox.margin = new RectOffset(5, 5, 5, 5);
                    _helpbox.padding = new RectOffset(5, 5, 5, 5);
                }
                return _helpbox;
            }
        }

        private static GUIStyle _boldout;
        public static GUIStyle Boldout
        {
            get
            {
                if (_boldout == null)
                {
                    _boldout = new GUIStyle(EditorStyles.foldout);
                    _boldout.fontStyle = FontStyle.Bold;
                }

                return _boldout;
            }
        }

        private static GUIStyle _input;
        public static GUIStyle Input
        {
            get
            {
                if (_input == null)
                {
                    _input = new GUIStyle(EditorStyles.helpBox);
                }
                return _input;
            }
        }

        private static GUIStyle _output;
        public static GUIStyle Output
        {
            get
            {
                if (_output == null)
                {
                    _output = new GUIStyle(EditorStyles.helpBox);
                }
                return _output;
            }
        }

        private static GUIStyle _outputText;
        public static GUIStyle OutputText
        {
            get
            {
                if (_outputText == null)
                {
                    _outputText = new GUIStyle(EditorStyles.label);
                    _outputText.alignment = TextAnchor.MiddleCenter;
                }
                return _outputText;
            }
        }

        private static GUIStyle _horizontalLine;
        public static GUIStyle HorizontalLine
        {
            get
            {
                if (_horizontalLine == null)
                {
                    _horizontalLine = new GUIStyle();
                    _horizontalLine.normal.background = EditorGUIUtility.whiteTexture;
                    _horizontalLine.margin = new RectOffset( 0, 0, 4, 4 );
                    _horizontalLine.fixedHeight = 1;
                }

                return _horizontalLine;
            }
        }

        private static GUIStyle _verticalLine;
        public static GUIStyle VerticalLine
        {
            get
            {
                if (_verticalLine == null)
                {
                    _verticalLine = new GUIStyle();
                    _verticalLine.normal.background = EditorGUIUtility.whiteTexture;
                    _verticalLine.margin = new RectOffset( 0, 0, 4, 4 );
                    _verticalLine.fixedWidth = 3;
                }

                return _verticalLine;
            }
        }

        private static GUIStyle _sidePanel;
        public static GUIStyle SidePanel
        {
            get
            {
                if (_sidePanel == null)
                {
                    _sidePanel = new GUIStyle();
                    _sidePanel.padding = new RectOffset(10, 10, 10, 10);
                }

                return _sidePanel;
            }
        }

        private static GUIStyle _selectBox;
        public static GUIStyle SelectBox
        {
            get
            {
                if (_selectBox == null)
                {
                    _selectBox = new GUIStyle(EditorStyles.helpBox);

                    _selectBox.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/SockEditor/Textures/select.png", typeof(Texture2D));
                    _selectBox.border = new RectOffset(3, 3, 3, 3);
                }

                return _selectBox;
            }
        }
    }
#endif
}