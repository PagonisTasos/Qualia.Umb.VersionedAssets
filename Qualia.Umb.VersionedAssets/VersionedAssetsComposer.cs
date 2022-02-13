using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Qualia.Umb.VersionedAssets
{
    public class VersionedAssetsComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddScoped<IAssetVersionCache, AssetVersionCache>();
        }
    }
}