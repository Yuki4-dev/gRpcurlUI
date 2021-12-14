using System;
using System.Collections.Generic;

namespace gRpcurlUI.Model
{
    public interface IProjectContext<out T> where T : IProject
    {
        string ProjectType { get; }

        string Verion { get; }

        IEnumerable<T> Projects { get; }
    }

    public interface IProject : ICloneable
    {
        string ProjectName { get; set; }

        string SendContent { get; set; }
    }
}
