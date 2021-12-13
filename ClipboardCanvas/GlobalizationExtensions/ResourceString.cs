﻿using Windows.ApplicationModel.Resources;
using Microsoft.UI.Xaml.Markup;

namespace ClipboardCanvas.GlobalizationExtensions
{
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    public sealed class ResourceString : MarkupExtension
    {
        private static readonly ResourceLoader ResourceLoader = new ResourceLoader();

        public string Name { get; set; }

        protected override object ProvideValue()
        {
            return ResourceLoader.GetString(Name);
        }
    }
}
