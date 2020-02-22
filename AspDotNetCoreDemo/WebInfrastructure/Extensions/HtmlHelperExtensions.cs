using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace AspDotNetCoreDemo.WebInfrastructure.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent TextBoxWithRequiredAttributeFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {

            var textbox = helper.TextBoxFor(expression, htmlAttributes);

            TagBuilder textboxBuilder = (TagBuilder)textbox;

            var expressionString = expression.ToString();

            if (IsPropertyRequired(expression.Parameters[0].Type, expressionString.Substring(expressionString.IndexOf('.') + 1)))
            {
                textboxBuilder.Attributes.Add("required", "true");
            }

            return textboxBuilder;
        }

        public static IHtmlContent PassWithRequiredAttributeFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {

            var textbox = helper.PasswordFor(expression, htmlAttributes);

            TagBuilder textboxBuilder = (TagBuilder)textbox;

            var expressionString = expression.ToString();

            if (IsPropertyRequired(expression.Parameters[0].Type, expressionString.Substring(expressionString.IndexOf('.') + 1)))
            {
                textboxBuilder.Attributes.Add("required", "true");
            }

            return textboxBuilder;
        }

        private static bool IsPropertyRequired(Type type, string propertyName)
        {
            // Get the FieldInfo object
            var propertyInfo = type.GetProperty(propertyName);

            // See if the Required attribute is defined for the field 
            bool isRequired = Attribute.IsDefined(propertyInfo, typeof(RequiredAttribute));

            return isRequired;
        }
    }
}
