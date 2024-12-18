using IDECore.DTOs;
using System.Text.Json;

namespace IDECore.Service
{
    public class JsonHandler
    {
        private const string filePath = "C:\\Users\\mahan\\source\\repos\\IDEWithDotNet\\IDECore\\Data.json";

        public DataSet LoadData()
        {
            if(!File.Exists(filePath))
            {
                return new DataSet
                {
                    MissingSymbols = new Dictionary<string, Dictionary<string, double>>(),
                    SyntaxErrors = new Dictionary<string, Dictionary<string, double>>()
                };
            }

            string jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<DataSet>(jsonData);
        }
    }
}
