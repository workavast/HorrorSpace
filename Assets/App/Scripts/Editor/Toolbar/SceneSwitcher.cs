using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace App.Editor.Toolbar
{
    public static class SceneSwitcher
    {
        private const string TOOLBAR_ELEMENT_NAME = "Custom/Scene Switcher";

        static SceneSwitcher()
        {
            EditorApplication.projectChanged += RefreshToolbar;
            EditorSceneManager.activeSceneChangedInEditMode += (_, __) => RefreshToolbar();
        }
        
        [MainToolbarElement(TOOLBAR_ELEMENT_NAME, defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement SceneSwitcherDropdown()
        {
            var icon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
            var activeSceneName = EditorSceneManager.GetActiveScene().name;
            if (string.IsNullOrEmpty(activeSceneName))
                activeSceneName = "Scene Switcher";

            var content = new MainToolbarContent(activeSceneName, icon, "Switch active scene");
            var dropdown = new MainToolbarDropdown(content, ShowSceneMenu);

            return dropdown;
        }

        private static void ShowSceneMenu(Rect dropDownRect)
        {
            var menu = new GenericMenu();
            var buildScenes = GetBuildScenes();
            var otherScenes = GetOtherScenes(buildScenes.Select(item => item.ScenePath).ToHashSet()).ToList();

            if (buildScenes.Count > 0)
            {
                foreach (var scene in buildScenes)
                    menu.AddItem(new GUIContent(GetSceneMenuLabel(scene.ScenePath)), false, () => OpenScene(scene.ScenePath));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("No scenes in build settings"));
            }

            if (otherScenes.Count > 0)
            {
                menu.AddSeparator(string.Empty);
                foreach (var scenePath in otherScenes)
                    menu.AddItem(new GUIContent(GetSceneMenuLabel(scenePath)), false, () => OpenScene(scenePath));
            }

            menu.DropDown(dropDownRect);
        }

        private static IReadOnlyList<BuildSceneInfo> GetBuildScenes()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene != null && !string.IsNullOrEmpty(scene.path) && !IsSceneInReadOnlyPackage(scene.path))
                .Select((scene, index) => new BuildSceneInfo(index, scene.path))
                .OrderBy(scene => scene.BuildIndex)
                .ToArray();
        }

        private static IEnumerable<string> GetOtherScenes(HashSet<string> buildScenePaths)
        {
            var sceneGuids = AssetDatabase.FindAssets("t:Scene");
            var scenePaths = sceneGuids
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(path => !string.IsNullOrEmpty(path)
                    && !buildScenePaths.Contains(path)
                    && !IsSceneInReadOnlyPackage(path))
                .OrderBy(path => path)
                .ToList();

            return scenePaths;
        }

        private static bool IsSceneInReadOnlyPackage(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
                return false;

            if (!scenePath.StartsWith("Packages/") && !scenePath.StartsWith("Library/PackageCache/"))
                return false;

            var packageInfo = PackageInfo.FindForAssetPath(scenePath);
            if (packageInfo == null)
                return true;

            switch (packageInfo.source)
            {
                case UnityEditor.PackageManager.PackageSource.Local:
                case UnityEditor.PackageManager.PackageSource.Embedded:
                    return false;
                case UnityEditor.PackageManager.PackageSource.BuiltIn:
                case UnityEditor.PackageManager.PackageSource.Registry:
                case UnityEditor.PackageManager.PackageSource.Git:
                case UnityEditor.PackageManager.PackageSource.Unknown:
                default:
                    return true;
            }
        }

        private static string GetSceneMenuLabel(string scenePath)
        {
            var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            return string.IsNullOrEmpty(sceneName) ? scenePath : sceneName;
        }

        private static void OpenScene(string scenePath)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
        }

        private static void RefreshToolbar()
        {
            MainToolbar.Refresh(TOOLBAR_ELEMENT_NAME);
        }

        private readonly struct BuildSceneInfo
        {
            public int BuildIndex { get; }
            public string ScenePath { get; }

            public BuildSceneInfo(int buildIndex, string scenePath)
            {
                BuildIndex = buildIndex;
                ScenePath = scenePath;
            }
        }
    }
}
