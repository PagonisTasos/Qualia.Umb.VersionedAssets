using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace Qualia.Umb.VersionedAssets
{
    internal class AssetVersionCache : IAssetVersionCache
    {
        private readonly IMemoryCache _cache;
        private readonly IWebHostEnvironment _environment;

        public AssetVersionCache(IMemoryCache cache, IWebHostEnvironment environment)
        {
            this._cache = cache;
            this._environment = environment;
        }

        public string AppendVersion(string filepath)
        {
            var fullPath = Path.Combine(_environment.ContentRootPath, filepath.TrimStart('~').TrimStart('/'));
            var version = GetOrAddCachedVersion(fullPath);
            return filepath.Contains("?") ? filepath + "&" + version : filepath + "?" + version;
        }


        private const string CACHE_KEY = "__AssetVersions__";

        private string GetOrAddCachedVersion(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            var assetCache = GetOrCreateAssetCache();
            string version;
            if (assetCache.ContainsKey(filePath))
            {
                version = assetCache[filePath];
            }
            else
            {
                version = ComputeFileHash(filePath);
                version = assetCache.GetOrAdd(filePath, version);
            }

            return version;
        }

        private ConcurrentDictionary<string, string> GetOrCreateAssetCache()
        {
            if (_cache.TryGetValue(CACHE_KEY, out ConcurrentDictionary<string, string> assetCache))
            {
                return assetCache;
            }

            assetCache = new ConcurrentDictionary<string, string>();
            _cache.Set(CACHE_KEY, assetCache);

            return assetCache;
        }

        private string ComputeFileHash(string fullFilePath)
        {
            using (var stream = File.OpenRead(fullFilePath))
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    return System.Convert.ToBase64String(sha256.ComputeHash(stream));
                }
            }
        }

    }
}
