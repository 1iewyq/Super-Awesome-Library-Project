using System;
using System.Collections.Generic;
using System.Drawing;


namespace Super_Awesome_Library_Project
{
    internal class DataGen
    {
        private readonly Random _rand = new Random();

        private readonly string[] _fNameList =
        {
            "Robert", "Jack", "John", "Jane", "Michael","William", "David","Stefan","Nelson","Richard","Charlie","Mary","Linda","Susan","Jessica","Kathleen","Ann", "Natalie","Qing"
        };

        private readonly string[] _lNameList =
        {
            "Smith","JOhnson","Williams","JOnes","Davis","Miller","Wilson","Moore","Taylor","Anderson","Thomas","Jackson","Citizen","Doe","Wong","Liew"
        };

        private readonly List<Bitmap> _icons;

        public DataGen()
        {
            _icons = new List<Bitmap>();
            for (var i = 0; i < 10; i++)
            {
                var image = new Bitmap(64, 64);
                for(var x  = 0; x < 64;x++)
                {
                    for (var y = 0; y < 64;y++)
                    {
                        image.SetPixel(x, y, Color.FromArgb(_rand.Next(256), _rand.Next(256), _rand.Next(256)));
                    }
                }
                _icons.Add(image);
            }

        }

        private string GetFirstName() => _fNameList[_rand.Next(_fNameList.Length)];

        private string GetLastName() => _lNameList[_rand.Next(_lNameList.Length)];

        private uint GetPIN() => (uint)_rand.Next(9999);

        private uint GetAcctNo() => (uint)_rand.Next(100000000, 999999999);

        private int Getbalance() => _rand.Next(-10000, 10000);

        private Bitmap GetIcon() => _icons[_rand.Next(_icons.Count)];

        public void GetNextAccount(out uint pin, out uint acctNo, out string firstName, out string lastName, out int balance, out Bitmap icon)
        {
            pin = GetPIN();
            acctNo = GetAcctNo();
            firstName = GetFirstName();
            lastName = GetLastName();
            balance = Getbalance();
            icon = GetIcon();
        }

        public int NumOAccts() => _rand.Next(100000, 999999);

    }
}
