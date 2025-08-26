using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DBLib
{
    internal class DataGen
    {
        private readonly Random _rand = new Random();

        private readonly string[] _fNameList =
        {
             "Natalie","Alfred", "Robert", "Jack", "John", "Jane", "Michael","William", "David","Stefan","Nelson","Richard","Charlie","Mary","Linda","Susan","Jessica","Kathleen","Ann"
        };

        private readonly string[] _lNameList =
        {
            "Wong","Liew", "Smith","Johnson","Williams","Jones","Davis","Miller","Wilson","Moore","Taylor","Anderson","Thomas","Jackson","Citizen","Doe"
        };

        private readonly List<Bitmap> _icons;

        public DataGen()
        {
            _icons = new List<Bitmap>();
            try
            {
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var dirInfo = new System.IO.DirectoryInfo(baseDir);
                string resourcesDir = null;
                while (dirInfo != null)
                {
                    var candidate = System.IO.Path.Combine(dirInfo.FullName, "Resources");
                    if (System.IO.Directory.Exists(candidate))
                    {
                        resourcesDir = candidate;
                        break;
                    }
                    dirInfo = dirInfo.Parent;
                }

                if (resourcesDir == null)
                    throw new System.IO.DirectoryNotFoundException("Resources folder not found.");

                var supported = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".png", ".jpg", ".jpeg", ".bmp" };

                foreach (var file in System.IO.Directory.EnumerateFiles(resourcesDir))
                {
                    var ext = System.IO.Path.GetExtension(file);
                    if (!supported.Contains(ext)) continue;

                    using (var src = new Bitmap(file))
                    {
                        var bmp = new Bitmap(64, 64);
                        using (var g = Graphics.FromImage(bmp))
                        {
                            g.DrawImage(src, 0, 0, 64, 64);
                        }
                        _icons.Add(bmp);
                    }
                }

                if (_icons.Count == 0)
                    throw new InvalidOperationException("No images found in Resources.");
            }
            catch (Exception ex)
            {
                var bmp = new Bitmap(64, 64);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(System.Drawing.Color.LightGray);
                }
                _icons.Add(bmp);

                System.Diagnostics.Debug.WriteLine("Icon load error: " + ex);
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
