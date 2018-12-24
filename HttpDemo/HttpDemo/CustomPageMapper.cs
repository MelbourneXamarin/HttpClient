using System;
using FreshMvvm;

namespace HttpDemo
{
    public class CustomPageMapper : IFreshPageModelMapper
    {
        public string GetPageTypeName(Type pageModelType)
        {
            return pageModelType.AssemblyQualifiedName?
                .Replace(".ViewModels.", ".Views.")
                .Replace("ViewModel", "View");
        }
    }
}
