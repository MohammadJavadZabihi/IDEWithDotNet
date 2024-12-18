using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

namespace IDECore.Service
{
    public class CodeFixer
    {
        private readonly JsonHandler _jsonHandler = new JsonHandler();
        private readonly QLearningStorage _qLearningStorage = new QLearningStorage();
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

                //UpdateQWithFeedBack(true);
            }
            inputeCode = string.Join("\n", updateLines);
            return inputeCode;

        }

        #endregion

        #region Qlearning Update

        public void UpdateQWithFeedBack(bool isGood)
        {
            var data = _jsonHandler.LoadData();

            if (isGood)
            {
                foreach (var dataFix in FixCodReturn)
                {
                    foreach (var item in dataFix.Value)
                    {
                        string state = dataFix.Key;
                        string action = item.Key;
                        double reward = item.Value;

                        if(!data.MissingSymbols.ContainsKey(state))
                        {
                            data.MissingSymbols[state] = new Dictionary<string, double>();
                        }

                        if(!data.MissingSymbols[state].ContainsKey(action))
                        {
                            data.MissingSymbols[state][action] = 0;
                        }

                        data.MissingSymbols[state][action] += reward;
                    }
                }
            }
            else
            {
                foreach(var dataFix in FixCodReturn)
                {
                    foreach(var item in dataFix.Value)
                    {
                        string state = dataFix.Key;
                        string action = item.Key;

                        data.MissingSymbols[state][action] = -1;
                    }
                }
            }
        }


        #endregion
    }
}
