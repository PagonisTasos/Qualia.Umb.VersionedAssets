namespace Qualia.Umb.VersionedAssets
{
    public interface IAssetVersionCache
    {
        /// <summary>
        /// Appends a version to the end of the filepath (as a query param). 
        /// The version is a hash of the contents of the file. 
        /// The version is cached so that it is recalculated upon system restart.
        /// </summary>
        /// <param name="filepath">relative path to the file</param>
        /// <returns></returns>
        public string AppendVersion(string filepath);
    }
}