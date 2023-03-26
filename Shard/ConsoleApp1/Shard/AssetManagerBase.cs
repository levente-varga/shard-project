using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
    {
        abstract class AssetManagerBase
        {
            private String assetPath;

            public string AssetPath { get; set; }

            public abstract void RegisterAssets();
            public abstract string GetAssetPath(string asset);
        }

}
