using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SWS.Shared.DataAccess
{
    using Models;

    public class DevicesStorage : BaseStorageOfT<DeviceInfo>
    {
        #region Consts

        private static string Db = Path.Combine(Environment.CurrentDirectory, @"TestDb.txt");

        #endregion

        public override async Task<IEnumerable<DeviceInfo>> GetEntitiesAsync()
        {
            try
            {
                var devices = new List<DeviceInfo>();

                var lines = await ReadFromFileAsync();
                if (lines != null && lines.Count() > 0)
                {
                    foreach (var line in lines)
                    {
                        var parameters = line.Split(';');

                        var id = Guid.TryParse(parameters[0], out Guid guid) == true ? guid : Guid.NewGuid();
                        var vendorId = int.TryParse(parameters[1], out int tmpvId) == true ? tmpvId : 0;
                        var productId = int.TryParse(parameters[2], out int tmppId) == true ? tmppId : 0;
                        var name = parameters[3];

                        devices.Add(new DeviceInfo(id, vendorId, productId, name));
                    }
                }

                return devices;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
                throw;
            }
        }

        public override async Task SaveEntityAsync(DeviceInfo entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                await WriteToFileAsync(entity.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: " + e);
                throw;
            }
        }

        #region Private

        private static async Task WriteToFileAsync(string line)
        {
            FileMode mode = FileMode.Create;
            if (File.Exists(Db))
            {
                mode = FileMode.Open;
            } 
            using (var file = File.Open(Db, mode, FileAccess.Write))
            using (var writer = new StreamWriter(file))
            {
                await writer.WriteLineAsync(line);
                await writer.FlushAsync();
            }
        }

        public static async Task<IEnumerable<string>> ReadFromFileAsync()
        {
            if (!File.Exists(Db))
            {
                //Just return empty list
                return new List<string>();
            }

            StreamReader file = new StreamReader(Db);

            var lines = new List<string>();

            while (!file.EndOfStream)
            {
                var line = await file.ReadLineAsync();
                if (String.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            file.Close();
            return lines;
        }

        #endregion
    }
}
