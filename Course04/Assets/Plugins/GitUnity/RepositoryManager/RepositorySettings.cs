using UnityEngine;

namespace GitUnity.Repository
{
    public class RepositorySettings : ScriptableObject
    {
        public new string name;
        public string description;
        public string gitignore_template = "Unity";
        public bool @private;
    }
}
