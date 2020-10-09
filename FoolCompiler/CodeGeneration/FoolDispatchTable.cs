using System.Collections.Generic;
using System.Text;

namespace FoolCompiler.CodeGeneration
{
    public class FoolDispatchTable
    {
        private static Dictionary<string, List<FoolDispatchTableEntry>> _dispatchTables = new Dictionary<string, List<FoolDispatchTableEntry>>();
        public static void AddDispatchTable(string key, List<FoolDispatchTableEntry> value)
        {
            _dispatchTables.Add(key, value);
        }
        public static List<FoolDispatchTableEntry> GetDispatchTable(string key)
        {
            List<FoolDispatchTableEntry> temporalDispatchTable = new List<FoolDispatchTableEntry>();
            List<FoolDispatchTableEntry> dTable = _dispatchTables[key];
            foreach (FoolDispatchTableEntry dTE in dTable)
            {
                FoolDispatchTableEntry temporalDTE = new FoolDispatchTableEntry(dTE.GetMethodId(), dTE.GetMethodLabel());
                temporalDispatchTable.Add(temporalDTE);
            }
            return temporalDispatchTable;
        }
        public static string CodeGenerationOfDispatchTable()
        {
            StringBuilder sB = new StringBuilder();
            //foreach (KeyValuePair<string, List<FoolDispatchTableEntry>> dt in _dispatchTables)
                foreach (var dt in _dispatchTables)
            {
                sB.Append("class" + dt.Key + ":\n");
                foreach (var entry in _dispatchTables[dt.Key])
                {
                    sB.Append(entry.GetMethodLabel());
                }
            }
            return sB.ToString();
        }
        public static void Reset()
        {
            _dispatchTables = new Dictionary<string, List<FoolDispatchTableEntry>>();
        }
    }
}
