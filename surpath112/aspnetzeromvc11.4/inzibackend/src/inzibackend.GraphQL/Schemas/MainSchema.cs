using Abp.Dependency;
using GraphQL.Types;
using GraphQL.Utilities;
using inzibackend.Queries.Container;
using System;

namespace inzibackend.Schemas
{
    public class MainSchema : Schema, ITransientDependency
    {
        public MainSchema(IServiceProvider provider) :
            base(provider)
        {
            Query = provider.GetRequiredService<QueryContainer>();
        }
    }
}