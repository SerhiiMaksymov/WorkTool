namespace WorkTool.Core.Modules.Xml.Extension;

public static class ObjectExtension
{
    public static Stream ToXmlStream<TObject>(this TObject obj) where TObject : notnull
    {
        var xmlSerializer = new XmlSerializer(obj.GetType());
        var stream = new MemoryStream();
        xmlSerializer.Serialize(stream, obj);

        return stream;
    }
}
