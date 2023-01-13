namespace WorkTool.Core.Modules.MaterialDesign.Services
{
    public class MaterialIconsTheme : IStyle
    {
        private readonly Uri baseUri;

        private IStyle? loaded;
        private bool isLoading;

        public MaterialIconsTheme() : this(AvaloniaUriBase.AppStyleUri) { }

        public MaterialIconsTheme(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public IReadOnlyList<IStyle> Children =>
            loaded?.Children ?? (IReadOnlyList<IStyle>)Array.Empty<IStyle>();

        public bool HasResources => Loaded is IResourceProvider { HasResources: true };

        public IStyle Loaded
        {
            get
            {
                if (loaded != null)
                {
                    return loaded;
                }

                isLoading = true;

                var styles = new Styles
                {
                    new StyleInclude(baseUri)
                    {
                        Source = MaterialDesignUriBase.MaterialIconsStyleUri
                    }
                };

                loaded = styles;
                isLoading = false;

                return loaded;
            }
        }

        public bool TryGetResource(object key, out object? value)
        {
            if (!isLoading && Loaded is IResourceProvider resourceProvider)
            {
                return resourceProvider.TryGetResource(key, out value);
            }

            value = null;

            return false;
        }

        public SelectorMatchResult TryAttach(IStyleable target, object? host)
        {
            return Loaded.TryAttach(target, host);
        }
    }
}
