using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YchetTovarov
{
    class Config
    {
        public fullList list = new fullList();
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TovarList");

        public Config()
        {
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (File.Exists(Path.Combine(folder, "test.txt")))
            {
               list = JsonConvert.DeserializeObject<fullList>(File.ReadAllText(Path.Combine(folder, "test.txt")));

            }
        }
        public void Save()
        {
            File.WriteAllText(Path.Combine(folder, "test.txt"), JsonConvert.SerializeObject(list));
            list = JsonConvert.DeserializeObject<fullList>(File.ReadAllText(Path.Combine(folder, "test.txt")));
        }
    }
}
