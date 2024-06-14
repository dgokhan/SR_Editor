using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BansheeGz.BGDatabase;

namespace SR_Editor.Utility
{
    public class CustomLocalizationLoader : BGLoaderForRepoCustom
    {
        public override byte[] Load(BGLoaderForRepo.LoadRequest request)
        {
            var asset = Path.Combine(Environment.CurrentDirectory, $"bansheegz_database_{request.paths[0]}_{request.paths[1]}.bytes");
            var file = File.ReadAllBytes(asset);

            return file;
        }
    }
}
