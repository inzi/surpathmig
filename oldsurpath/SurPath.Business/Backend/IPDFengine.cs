namespace SurpathBackend
{
    public interface IPDEngine
    {
        bool LoadTemplate(string TemplateName);

        object SaveTemplate(object Template, string TemplateName);

        object RenderTemplate(object Template, object Data);
    }
}