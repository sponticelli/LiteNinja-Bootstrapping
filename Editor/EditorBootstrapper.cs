using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LiteNinja.Bootstrapping
{
    public static class EditorBootstrapper
    {
        /// <summary>
        /// This class is responsible for bootstrapping the game in the editor.
        /// It ensures that the bootstrap scene is loaded first before the current scene.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Get the current scenes name
            var sceneNames = new List<string>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneNames.Add(SceneManager.GetSceneAt(i).name);
            }
            
            var activeSceneName = SceneManager.GetActiveScene().name;

            // Get the bootstrap settings from the resources
            var bootstrapSettings = Resources.Load<BootstrapSettings>("LN_BootstrapSettings");
            
            // If the boostrap scene is already loaded then return
            if (sceneNames.Any(scene => scene == bootstrapSettings.BootstrapSceneName))
            {
                return;
            }
            
            LoadScenes(bootstrapSettings, sceneNames, activeSceneName);
        }
        
        private static void LoadScenes(BootstrapSettings bootstrapSettings, List<string> currentScenes, string activeSceneName)
        {
            var op = SceneManager.LoadSceneAsync(bootstrapSettings.BootstrapSceneName, LoadSceneMode.Single);
            op.completed += _ =>
            {
                // Load all the scenes that were loaded before the bootstrap scene
                foreach (var scene in currentScenes)
                {
                    Debug.Log($"Load {scene}");
                    
                    if (scene == activeSceneName)
                    {
                        var async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                        async.completed += async _ =>
                        {
                            await Task.Delay(300);
                            var activeScene = SceneManager.GetSceneByName(scene);
                            SceneManager.SetActiveScene(activeScene);
                        };

                    }
                    else
                    {
                        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
                    }
                }
                
            };
        }
    }
}