using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace IDECore.Service
{
    public class CodeFixer
    {
        private readonly JsonHandler _jsonHandler = new JsonHandler();
        private readonly QLearningStorage qLearningStorage = new QLearningStorage();
        public Dictionary<string, Dictionary<string, double>> FixCodReturn { get; private set; } = new Dictionary<string, Dictionary<string, double>>();

        #region Fix Code Method

        public string FixCode(string inputeCode)
        {
            var data = _jsonHandler.LoadData();

            var lines = inputeCode.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var updateLines = new List<string>();
            var misingSymbolsHashSet = new HashSet<string>(data.MissingSymbols.Keys);
            var syntaxtErrorHashSet = new HashSet<string>(data.SyntaxErrors.Keys);

            foreach (var line in lines)
            {
                string updateLine = line;

                foreach (var key in misingSymbolsHashSet)
                {
                    if (updateLine.Contains(key))
                    {
                        var misiSymbolValue = data.MissingSymbols[key];

                        if (key == "(" || key == "[" || key == "{")
                        {

                            updateLine = updateLine.Replace(key, key + misiSymbolValue.First().Key);
                        }
                        else
                        {
                            updateLine = updateLine.Replace(key, misiSymbolValue.First().Key + key);
                        }

                        if (!FixCodReturn.ContainsKey(key))
                        {
                            FixCodReturn[key] = new Dictionary<string, double>();
                        }

                        var valuKey = misiSymbolValue.First().Key;
                        var valueValue = misiSymbolValue.First().Value;

                        if (!FixCodReturn[key].ContainsKey(valuKey))
                        {
                            FixCodReturn[key].Add(valuKey, valueValue);
                        }
                    }
                }

                foreach (var error in syntaxtErrorHashSet)
                {
                    if (updateLine.Contains(error))
                    {
                        var errorSyntaxt = data.SyntaxErrors[error];

                        updateLine = updateLine.Replace(error, errorSyntaxt.First().Key);

                        if (!FixCodReturn.ContainsKey(error))
                        {
                            FixCodReturn[error] = new Dictionary<string, double>();

                            var valueKey = errorSyntaxt.First().Key;
                            var valueValu = errorSyntaxt.First().Value;

                            if (!FixCodReturn[error].ContainsKey(valueKey))
                            {
                                FixCodReturn[error].Add(valueKey, valueValu);
                            }
                        }
                    }

                }

                updateLines.Add(updateLine);

            }
            inputeCode = string.Join("\n", updateLines);
            return inputeCode;

        }

        #endregion

        #region Qlearning Update

        public void UpdateQWithFeedBack(bool isGood)
        {
            double alpha = 0.1;
            double gamma = 0.9;

            if (isGood)
            {
                foreach (var dataFix in FixCodReturn)
                {
                    foreach (var item in dataFix.Value)
                    {
                        string state = dataFix.Key;
                        string action = item.Key;
                        double reward = item.Value;

                        double currentQ = qLearningStorage.GetQValue(state, action);

                        double maxNextQ = 0;

                        double newQ = currentQ + alpha * (reward + gamma * maxNextQ - currentQ);

                        qLearningStorage.UpdateQValue(state, action, newQ);
                    }
                }
            }
            else
            {
                foreach (var dataFix in FixCodReturn)
                {
                    foreach (var item in dataFix.Value)
                    {
                        string state = dataFix.Key;
                        string action = item.Key;

                        double currentQ = qLearningStorage.GetQValue(state, action);
                        double penalty = -1;
                        double newQ = currentQ + alpha * (penalty - currentQ);

                        qLearningStorage.UpdateQValue(state, action, newQ);
                    }
                }
            }
            qLearningStorage.SaveQTable();
        }
        #endregion
    }
}
