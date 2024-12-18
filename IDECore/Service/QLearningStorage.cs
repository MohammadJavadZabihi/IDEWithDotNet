using IDECore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IDECore.Service
{
    public class QLearningStorage
    {
        private readonly JsonHandler _jsonHandler = new JsonHandler();

        public async void UpdateQWihtFeedBack(string state, string action, double reward)
        {
            var data = _jsonHandler.LoadData();

            //var misingDataHashSet = new HashSet<string>(data.MissingSymbols.Keys);
            //var syntaxtError = new HashSet<string>(data.SyntaxErrors.Keys);

            foreach(var symbol in data.MissingSymbols)
            {

            }
        }
    }
}
