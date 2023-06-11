using System;

namespace gRpcurlUI.Model
{
    public class ProjectContextFactory
    {
        public static IProjectContext CreateFromJson(Type contextType, object jsonObject)
        {
            var context = (IProjectContext)Activator.CreateInstance(contextType)!;
            context.LoadJsonObject(jsonObject);
            return context;
        }
    }
}
