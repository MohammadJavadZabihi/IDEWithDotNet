using System.Text.Json;

namespace IDECore.Service
{
    public class QLearningStorage
    {
        private const string FilePath = "C:\\Users\\mahan\\source\\repos\\IDEWithDotNet\\IDECore\\qtable.json";

        public Dictionary<string, Dictionary<string, double>> QTable { get; set; }

        public QLearningStorage()
        {
            LoadQTable();
        }

        private void LoadQTable()
        {
            if (File.Exists(FilePath))
            {
                string jsonData = File.ReadAllText(FilePath);
                QTable = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, double>>>(jsonData);
            }
            else
            {
                QTable = new Dictionary<string, Dictionary<string, double>>();
            }
        }

        public void SaveQTable()
        {
            string jsonData = JsonSerializer.Serialize(QTable, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }

        public void InitializeState(string state, string action, double defaultValue = 0)
        {
            if (!QTable.ContainsKey(state))
            {
                QTable[state] = new Dictionary<string, double>();
            }

            if (!QTable[state].ContainsKey(action))
            {
                QTable[state][action] = defaultValue;
            }
        }

        public void UpdateQValue(string state, string action, double newValue)
        {
            if (QTable.ContainsKey(state) && QTable[state].ContainsKey(action))
            {
                QTable[state][action] = newValue;
            }
            else
            {
                InitializeState(state, action, newValue);
            }
        }

        public double GetQValue(string state, string action)
        {
            if (QTable.ContainsKey(state) && QTable[state].ContainsKey(action))
            {
                return QTable[state][action];
            }
            return 0;
        }
    }
}
