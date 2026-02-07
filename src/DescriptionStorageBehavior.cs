using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.SaveSystem;

namespace EditableEncyclopedia
{
    public class DescriptionStorageBehavior : CampaignBehaviorBase
    {
        private Dictionary<string, string> _descriptions = new Dictionary<string, string>();

        public static DescriptionStorageBehavior Instance { get; private set; }

        public DescriptionStorageBehavior()
        {
            Instance = this;
        }

        public override void RegisterEvents() { }

        public override void SyncData(IDataStore dataStore)
        {
            dataStore.SyncData("_editableEncyclopediaDescriptions", ref _descriptions);
            if (_descriptions == null)
                _descriptions = new Dictionary<string, string>();
        }

        public string GetDescription(string objectId)
        {
            return _descriptions.TryGetValue(objectId, out string desc) ? desc : null;
        }

        public bool HasDescription(string objectId)
        {
            return _descriptions.ContainsKey(objectId);
        }

        public void SetDescription(string objectId, string description)
        {
            if (string.IsNullOrEmpty(description))
                _descriptions.Remove(objectId);
            else
                _descriptions[objectId] = description;
        }

        public void RemoveDescription(string objectId)
        {
            _descriptions.Remove(objectId);
        }

        public Dictionary<string, string> GetAllDescriptions()
        {
            return new Dictionary<string, string>(_descriptions);
        }

        public int GetDescriptionCount()
        {
            return _descriptions.Count;
        }

        /// <summary>
        /// Merges imported descriptions into the current set.
        /// Existing keys are overwritten by the imported values.
        /// </summary>
        public int MergeDescriptions(Dictionary<string, string> incoming)
        {
            int count = 0;
            foreach (var kvp in incoming)
            {
                if (!string.IsNullOrEmpty(kvp.Value))
                {
                    _descriptions[kvp.Key] = kvp.Value;
                    count++;
                }
            }
            return count;
        }
    }
}
