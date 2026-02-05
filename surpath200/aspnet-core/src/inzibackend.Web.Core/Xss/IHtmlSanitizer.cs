using Abp.Dependency;

namespace inzibackend.Web.Xss;

public interface IHtmlSanitizer : ITransientDependency
{
    string Sanitize(string html);
}

