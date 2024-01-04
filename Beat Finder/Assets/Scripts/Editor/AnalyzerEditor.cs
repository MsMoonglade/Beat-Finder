using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AnalyzerEditor : EditorWindow
{
    [MenuItem("Tools/My Custom Editor")]
    public static void ShowMyEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<AnalyzerEditor>();
        wnd.titleContent = new GUIContent("Song Analyzer");
    }

    public void CreateGUI()
    {
        // Get a list of all songs in the project     

        var allObjectGuids = AssetDatabase.FindAssets("t:AudioClip", new[] { "Assets/Songs" });
        var allObjects = new List<AudioClip>();
        foreach (var guid in allObjectGuids)
        {
            allObjects.Add(AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guid)));
        }
        // Create a two-pane view with the left pane being fixed with
        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

        // Add the panel to the visual tree by adding it as a child to the root element
        rootVisualElement.Add(splitView);
      
        // A TwoPaneSplitView always needs exactly two child elements
        ListView leftPane = new ListView();
        splitView.Add(leftPane);

        var rightPane = new VisualElement();
        splitView.Add(rightPane);

        //Left Panel content
        LeftPanel(leftPane , allObjects);
    }

    private void LeftPanel(ListView leftPanel , List<AudioClip> content)
    {
        // Initialize the list view with all Clip's names
        leftPanel.makeItem = () => new Label();
        leftPanel.bindItem = (item, index) => { (item as Label).text = content[index].name; };
        leftPanel.itemsSource = content;
    }
}
