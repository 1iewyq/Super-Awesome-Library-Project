using System.Drawing;
using System.Collections.Generic;

namespace DBLib
{
    public class Database
    {
        public readonly List<DataStruct> _database;

        public static Database Instance { get; } = new Database();
        static Database() { }

        private Database()
        {
            _database = new List<DataStruct>();
            var generator = new DataGen();
            for (var i = 0; i < generator.NumOAccts(); i++)
            {
                var temp = new DataStruct();
                generator.GetNextAccount(out temp.pin, out temp.acctNo, out temp.firstName, out temp.lastName, out temp.balance, out temp.icon);
                _database.Add(temp);
            }
        }

        public uint GetAcctNoByIndex(int index)
        {
            return _database[index].acctNo;
        }

        public uint GetPINByIndex(int index)
        {
            return _database[index].pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            return _database[index].firstName;
        }

        public string GetLastNameByIndex(int index)
        {
            return _database[index].lastName;
        }

        public int GetBalanceByIndex(int index)
        {
            return _database[index].balance;
        }

        public Bitmap GetIconByIndex(int index)
        {
            return _database[index].icon;
        }

        public int GetNumRecords()
        {
            return _database.Count;
        }
    }
}
