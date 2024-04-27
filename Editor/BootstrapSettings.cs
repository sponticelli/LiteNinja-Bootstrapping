using UnityEngine;

namespace LiteNinja.Bootstrapping
{
    [CreateAssetMenu(menuName = "LiteNinja/Bootstrap/Settings", fileName = "LN_BootstrapSettings", order = 0)]
    public class BootstrapSettings : ScriptableObject
    {
        [SerializeField] private string _bootstrapSceneName = "Bootstrap";
        
        public string BootstrapSceneName => _bootstrapSceneName;
    }
}