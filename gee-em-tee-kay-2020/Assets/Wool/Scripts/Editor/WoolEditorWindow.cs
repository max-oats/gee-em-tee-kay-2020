using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Wool;

/**
* WoolEditorWindow
* - Split into three sections- Raw text, parsed text, and node tree
*/
public class WoolEditorWindow : EditorWindow
{
    /** The actual wool database
     * - Stores all the node info and all the running node strings */
    WoolDatabase db = null;

    /** Text strings */
    const string titleText = "Wool Editor";
    const string woolDatabaseFilePath = "Assets/Wool/WoolDatabase.asset";

    /** Minimum screen width/height so we can't resize too far */
    const float minScreenWidth = 500f;
    const float minScreenHeight = 300f;

    /** Editor state */
    float zoomLevel = 1f;
    Vector2 nodeTreePos = Vector2.zero;
    bool isDraggingScreen = false;
    Vector2 textScrollAmount = Vector2.zero;

    /** The currently set up node (should never be null) */
    YarnNode currentlyLoadedNode = null;

    /** Have nodes been edited since the last save? */
    bool hasEditedSinceLastSave = false;

    /** Rects for the different "panels" in the window */
    Rect nodeTreeRect;
    Rect rawTextRect;
    Rect parsedTextRect;

    /** Colors */
    Color lightGrey = new Color(0.85f, 0.85f, 0.85f, 1f);
    Color lightGreybehindTextArea = new Color(0.89f, 0.89f, 0.89f, 1f);

    /** Resizing stuff */
    bool isResizingNodeTreePanel = false;
    static float currentResizedNodeTreeWidth = minScreenWidth;

    /** Mouse state */
    Vector2 mousePos = Vector2.zero;
    Vector2 prevMousePos = Vector2.zero;
    Vector2 mouseDelta = Vector2.zero;

    /** Bools for whether the window/objects needs updating or repainting */
    bool isDirty = false;
    bool requireRepaint = false;

    /** Wool tags + parsed text */
    WoolTagContainer woolTags = null;
    EditorTextObject parsedText = null;

    /** Show the window */
    [MenuItem("Socks/Wool Editor")]
    public static WoolEditorWindow ShowWindow()
    {
        WoolEditorWindow window = GetWindow<WoolEditorWindow>(false, "Wool Editor");
        window.minSize = new Vector2(minScreenWidth, minScreenHeight);

        return window;
    }

    /** On enable, reset the position state + initialize the tags + parsed text + the initial database */
    void OnEnable()
    {
        Init();
        
        ResetEditorPositionState();

        SetUpWoolDatabase();
    }

    /** Update */
    void OnGUI()
    {
        /** Reset states */
        isDirty = false;
        requireRepaint = false;

        UpdateInputs();

        DrawStage();

        UpdateStage();

        if (requireRepaint)
        {
            Repaint();
        }

        UpdateTitle();

        if (isDirty)
        {
            EditorUtility.SetDirty(db);
        }
    }

    void Init()
    {
        /** Grab the wool  */
        woolTags = new WoolTagContainer();
        parsedText = new EditorTextObject();

        if (currentlyLoadedNode != null)
        {
            
        }
    }

    void ResetEditorPositionState()
    {
        nodeTreePos = Vector2.zero;

        /** Set default node tree pos to exactly half way in the screen */
        currentResizedNodeTreeWidth = position.width / 2f;
    }

    void SetUpWoolDatabase()
    {
        /** Load database */
        string[] dbs = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(WoolDatabase).Name));
        if (dbs.Length == 0)
        {
            /** If we haven't found one, create one */
            Debug.LogFormat("Creating WoolDatabase at filepath '<i>{0}</i>'", woolDatabaseFilePath);
            db = (WoolDatabase)CreateInstance(typeof(WoolDatabase));
            AssetDatabase.CreateAsset(db, woolDatabaseFilePath);
        }
        else
        {
            /** if we do find one, load it! */
            db = (WoolDatabase)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(dbs[0]), typeof(WoolDatabase));
        }

        /** Load yarn nodes */
        db.LoadYarnNodes();
    }
    
    public void UpdateTitle()
    {
        string finalTitle = titleText;

        /** If the nodes need to be saved, add an asterisk to the title */
        if (hasEditedSinceLastSave)
        {
            finalTitle += " *";
        }
        titleContent = new GUIContent(finalTitle);
    }

    /** Update inputs */
    void UpdateInputs()
    {
        /** Update mouse state */
        mousePos = Event.current.mousePosition;
        mouseDelta = mousePos - prevMousePos;
        prevMousePos = mousePos;

        /** Check for save update */
        if (Event.current.type == EventType.KeyDown 
            && Event.current.keyCode == KeyCode.S)
        {
            SaveDatabase();

            Event.current.Use();
        }
    }

    /** Update database save */
    void SaveDatabase()
    {
        hasEditedSinceLastSave = false;

        db.Save();
    }

    /** Update stage
        * - Handles all the actual logic 
        */
    void UpdateStage()
    {
        DrawMenuBox();

        UpdateNodeTree();

        UpdateNodeTreePanelResizing();
    }

    /** Draw stage
        * - Handles the drawing of the editor, the node tree, the parsed text etc
        */
    void DrawStage()
    {
        DrawNodeTreePanel();

        DrawTextPanel();
    }

    void DrawNodeTreePanel()
    {
        DrawNodeTree();

        DrawTextPanelResizeLine();
    }

    void DrawTextPanel()
    {
        DrawRawText();

        DrawParsedText();

        DrawTextDividerLine();
    }

    void UpdateNodeTree()
    {
        GUILayout.BeginArea(nodeTreeRect);
        {
            /** Update nodes */
            foreach (YarnNode node in db.nodes)
            {
                UpdateNode(node);
            }

            /** Scroll wheel input */
            if (Event.current.type == EventType.MouseDown)
            {
                /** Check for right click */
                if (Event.current.button == 1
                    && nodeTreeRect.Contains(mousePos))
                {
                    // Do a thing, in this case a drop down menu
                    GenericMenu menu = new GenericMenu();
                
                    menu.AddItem(new GUIContent("Create New Node"), false, CreateNewNode);
                    menu.ShowAsContext();

                    Event.current.Use();
                }

                /** Check for left click */
                if (Event.current.button == 0  // Left mouse button
                    && nodeTreeRect.Contains(mousePos))
                {
                    // Focus control off of the text area
                    GUI.FocusControl(null);
                }
            }
            
            /** Check if the scroll wheel is being scrolled */
            if (Event.current.type == EventType.ScrollWheel)
            {
                /** Update zoom */
                UpdateZoom();
            }
        }
        GUILayout.EndArea();

        UpdateScreenDrag();
    }

    void CreateNewNode()
    {
        LoadNode(db.AddNode());

        hasEditedSinceLastSave = true;
    }

    void UpdateZoom()
    {
        float zoomDelta = -Event.current.delta.y*0.01f;
        float newZoom = Mathf.Clamp(zoomLevel+(zoomDelta), 0.4f, 2f);
        Vector2 ratio = new Vector2((mousePos.x - (nodeTreeRect.width / 2f)) / nodeTreeRect.width,
                                    (mousePos.y - (nodeTreeRect.height / 2f)) / nodeTreeRect.height);

        Vector2 rectSize = new Vector2(nodeTreeRect.width, nodeTreeRect.height);
        Vector2 pixels_difference = (rectSize / zoomLevel) - (rectSize / newZoom);

        Vector2 mousePosInRelationToBox = new Vector2(nodeTreeRect.width/2f - mousePos.x, (nodeTreeRect.height/2f - mousePos.y));
        zoomLevel = newZoom;

        nodeTreePos += pixels_difference * ratio;
    }

    void UpdateScreenDrag()
    { 
        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        if (!isDraggingScreen && Event.current.GetTypeForControl(controlId) == EventType.MouseDown
            && Event.current.button == 2  // Scroll wheel down
            //|| (Event.current.button == 0 && (Event.current.modifiers == EventModifiers.Alt))) // Alt+left mouse
        )
        {
            GUIUtility.hotControl = controlId;
            isDraggingScreen = true;
            Event.current.Use();
        }

        /** Check whether we want to stop dragging */
        if (isDraggingScreen && Event.current.GetTypeForControl(controlId) == EventType.MouseUp
            && (Event.current.button == 0   // Left mouse up
            || Event.current.button == 2))  // Scroll wheel up
        {
            GUIUtility.hotControl = 0;
            // Turn off screen dragging
            isDraggingScreen = false;
            Event.current.Use();
        }

        /** Screen dragging */
        if (isDraggingScreen)
        {
            nodeTreePos += mouseDelta;
            requireRepaint = true;
        }
    }

    void DrawTextPanelResizeLine()
    {
        Rect panelInbetween = new Rect(currentResizedNodeTreeWidth, 0f, 5f, position.height);
        
        /** Store colour to set to normal later */
        var tempColor = GUI.color;
        GUI.color = Colors.ResizeLine;

        /** Draw line */
        GUILayout.BeginArea(panelInbetween, Styles.VerticalLine);
        GUILayout.EndArea();
        
        /** Reset colour to prev */
        GUI.color = tempColor;
    }

    void DrawTextDividerLine()
    {
        Rect panelInbetween = new Rect(currentResizedNodeTreeWidth, position.height/2f, position.width - currentResizedNodeTreeWidth, 5f);
        
        /** Store colour to set to normal later */
        var c = GUI.color;
        GUI.color = Colors.ResizeLine;
        
        /** Draw line */
        GUILayout.BeginArea(panelInbetween, Styles.HorizontalLine);
        GUILayout.EndArea();
        
        /** Reset colour to prev */
        GUI.color = c;
    }

    void UpdateNodeTreePanelResizing()
    {
        /** Clamp to defaults */
        currentResizedNodeTreeWidth = Mathf.Clamp(currentResizedNodeTreeWidth, position.width * 0.1f, position.width * 0.9f);
        Rect panelInbetween = new Rect(currentResizedNodeTreeWidth, 0f, 5f, position.height);

        /** Update input */
        switch (Event.current.type)
        {
            case EventType.MouseDown:
                if (Event.current.button == 0 // Left mouse
                    && panelInbetween.Contains(mousePos)) 
                {
                    isResizingNodeTreePanel = true;
                    Event.current.Use();
                }
                break;
            case EventType.MouseUp:
                if (Event.current.button == 0)
                {
                    isResizingNodeTreePanel = false;
                    Event.current.Use();
                }
                break;
        }

        if (isResizingNodeTreePanel)
        {
            /** If resizing- update based on current mousepos */
            currentResizedNodeTreeWidth = Mathf.Clamp(mousePos.x, position.width * 0.1f, position.width * 0.9f);
            panelInbetween.Set(currentResizedNodeTreeWidth, panelInbetween.y, panelInbetween.width, panelInbetween.height);

            requireRepaint = true;
        }
    }

    int GetAmountOfLines(string label)
    {
        return label.Length - label.Replace("\n", "").Replace("\r", "").Length+1;
    }

    void DrawLineNumbers(string label)
    {
        string full = "";
        int count = GetAmountOfLines(label);
        for (int i = 1; i <= count; ++i)
        {
            full += i;
            full += "\n";
        }

        GUILayout.Label(full, Styles.LineNumber, GUILayout.MaxWidth(count.ToString().Length * 10f));
    }

    void DrawRawText()
    {
        // Grab font size
        int textHeight = 13;

        /** Update the raw text rect according to the current size */
        rawTextRect = new Rect(currentResizedNodeTreeWidth, 0f, Screen.width - currentResizedNodeTreeWidth, position.height/2f);
        GUILayout.BeginArea(rawTextRect, Styles.TextSidePanel);
        {
            EditorGUILayout.LabelField("Raw text:", Styles.Bold);
            EditorGUI.BeginChangeCheck();
            Vector2 newScrollAmount = GUILayout.BeginScrollView(textScrollAmount, false, true);
            if (EditorGUI.EndChangeCheck())
            {
                textScrollAmount = newScrollAmount;
            }
            
            int count = GetAmountOfLines(currentlyLoadedNode.fullString);
            float w = (count.ToString().Length * 10f) + 8f;

            GUILayout.BeginHorizontal();
            {
                DrawLineNumbers(currentlyLoadedNode.fullString);

                for (int i = 0; i < count; ++i)
                {
                    if (i%2==0)
                    {
                        EditorGUI.DrawRect(new Rect(w, (i*textHeight)+3f, (parsedTextRect.width - w - 32f), textHeight), lightGreybehindTextArea);
                    }
                }
                bool wasChanged = false;
                EditorGUI.BeginChangeCheck();

                /** Draw text area for raw text */
                GUI.SetNextControlName("RawTextEditor");
                string newText = GUI.TextArea(new Rect(w, 2f, parsedTextRect.width - w - 32f, position.height/2f + count*textHeight), 
                                                currentlyLoadedNode.fullString);

                TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                if (GUIUtility.keyboardControl == editor.controlID 
                    && Event.current.type == EventType.KeyDown 
                    && (Event.current.character == '\t'))
                {
                    wasChanged = true;

                    if (newText.Length > editor.cursorIndex) 
                    {
                        newText = newText.Insert(editor.cursorIndex, "    ");
                        editor.cursorIndex+=4;
                        editor.selectIndex = editor.cursorIndex;
                    }
                    
                    GUI.FocusControl("RawTextEditor");
                    Event.current.Use();
                }

                wasChanged |= EditorGUI.EndChangeCheck();

                if (wasChanged)
                {
                    Undo.RecordObject(db, "Updated node text");
                    hasEditedSinceLastSave = true;

                    currentlyLoadedNode.fullString = newText;

                    parsedText.UpdateContainer(rawTextRect.width, rawTextRect.height);
                    parsedText.ParseText(currentlyLoadedNode.fullString);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    void DrawParsedText()
    {
        // Grab font size
        int textHeight = 13;

        parsedTextRect = new Rect(currentResizedNodeTreeWidth, position.height/2f, Screen.width - currentResizedNodeTreeWidth, position.height/2f); 
        GUILayout.BeginArea(parsedTextRect, Styles.TextSidePanel);
        {
            EditorGUILayout.LabelField("Parsed text:", Styles.Bold);
            EditorGUI.BeginChangeCheck();
            Vector2 newScrollAmount = GUILayout.BeginScrollView(textScrollAmount, false, true);
            if (EditorGUI.EndChangeCheck())
            {
                textScrollAmount = newScrollAmount;
            }

            GUILayout.BeginHorizontal();
            {
                DrawLineNumbers(currentlyLoadedNode.fullString);
                EditorGUI.BeginDisabledGroup(true);
                GUIStyle areaStyle = new GUIStyle(EditorStyles.textArea);
                areaStyle.normal.textColor = new Color(0f, 0f, 0f, 0f);
                int count = GetAmountOfLines(currentlyLoadedNode.fullString);
                float w = (count.ToString().Length * 10f) + 3f;
                GUI.TextArea(new Rect(w+5f, 2f, parsedTextRect.width - w - 35f, position.height/2f + count*textHeight), "");
                EditorGUI.EndDisabledGroup();

                Rect newRect = new Rect(textScrollAmount.x, textScrollAmount.y, parsedTextRect.width, parsedTextRect.height);
                GUILayout.BeginArea(new Rect(6f, 2f, Screen.width, currentlyLoadedNode.fullString.Length*10));
                {
                    /** Draw alternating lines for cute row things */
                    for (int i = 0; i < count; ++i)
                    {
                        if (i%2==0)
                        {
                            EditorGUI.DrawRect(new Rect(w, (i*textHeight)+1f, (parsedTextRect.width - w - 37f), textHeight), lightGrey);
                        }
                    }

                    foreach (EditorLine line in parsedText.lines)
                    {
                        foreach (EditorLetter letter in line.letters)
                        {
                            letter.Update();
                        }

                        parsedText.DrawLineAndBatch(line, w, newRect);
                    }

                    if (currentlyLoadedNode.fullString.Length > 0)
                    {
                        Repaint();
                    }
                }
                GUILayout.EndArea();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }
        GUILayout.EndArea();
    }

    void DrawNodeTree()
    {
        /** Set current screen rect */
        nodeTreeRect = new Rect(0f, 0f, currentResizedNodeTreeWidth, position.height);      

        GUILayout.BeginArea(nodeTreeRect);
        {
            float pos = 0f;
            foreach (YarnNode node in db.nodes)
            {
                node.pos = new Vector2(pos, 50f);
                DrawNode(node);

                pos += 110f;
            }
        }
        GUILayout.EndArea();  
    }

    /** Load a new node into the raw text/parsed text */
    void LoadNode(YarnNode node)
    {
        // Set node
        currentlyLoadedNode = node;

        // Reset focus + reset scroll
        GUI.FocusControl(null);
        textScrollAmount = Vector2.zero;
        
        parsedText.UpdateContainer(rawTextRect.width, rawTextRect.height);
        parsedText.ParseText(currentlyLoadedNode.fullString);
    }

    void UpdateNode(YarnNode node)
    {
        Rect rect = new Rect((node.pos.x + nodeTreePos.x)*zoomLevel, (node.pos.y + nodeTreePos.y)*zoomLevel, 100f*zoomLevel, 100f*zoomLevel);
    
        /** If we click on a node ,load the node */
        if (Event.current.type == EventType.MouseDown 
            && Event.current.button == 0 // Left mouse click
            && nodeTreeRect.Contains(mousePos) && rect.Contains(mousePos))
        {
            LoadNode(node);
            Event.current.Use();
        }
    }

    /** Draw the given node (in the node tree) */
    void DrawNode(YarnNode node)
    {
        Rect rect = new Rect((node.pos.x + nodeTreePos.x)*zoomLevel, (node.pos.y + nodeTreePos.y)*zoomLevel, 100f*zoomLevel, 100f*zoomLevel);

        /** Draw the node */
        GUILayout.BeginArea(rect, Styles.Node);
        {
            EditorGUILayout.LabelField(node.title);
            //SockEditor.Utils.HorizontalLine(Color.grey);
        }
        GUILayout.EndArea();
    }

    void DrawMenuBox()
    {
        Rect menuBoxRect = new Rect(10f, 10f, Mathf.Min(currentResizedNodeTreeWidth-20f, 300f), 50f);
        
        GUILayout.BeginArea(menuBoxRect, Styles.Node);
        {
            EditorGUI.BeginChangeCheck();
            string nodeTitle = EditorGUILayout.TextField(currentlyLoadedNode.title);
            if (EditorGUI.EndChangeCheck())
            {
                currentlyLoadedNode.title = nodeTitle;
                hasEditedSinceLastSave = true;
            }

            GUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            string filename = EditorGUILayout.TextField(currentlyLoadedNode.filename);
            if (EditorGUI.EndChangeCheck())
            {
                currentlyLoadedNode.filename = filename;
                hasEditedSinceLastSave = true;
            }
            
            if (GUILayout.Button("✉"))
            {
                string path = EditorUtility.OpenFilePanel("Choose yarn file to save to...", Application.dataPath + "/Dialogue", "yarn");
                if (path != null && path.Length > 0)
                {
                    currentlyLoadedNode.filename = path;
                    hasEditedSinceLastSave = true;
                }
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();
    }
}