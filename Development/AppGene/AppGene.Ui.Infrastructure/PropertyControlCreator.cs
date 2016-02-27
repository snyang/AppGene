﻿using AppGene.Common.Entities.Infrastructure.Inferences;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AppGene.Ui.Infrastructure
{
    public static class PropertyControlCreator
    {
        // TODO: Enrich the class PropertyControlCreator
        public static void Create(DisplayPropertyInfo property,
            Grid parent,
            int row,
            int column,
            string bindingPathPrefix,
            Style style)
        {
            if (property.PropertyInfo.PropertyType.IsEnum)
            {
                CreateEnumControl(property, parent, row, column, bindingPathPrefix, style);
                return;
            }
            if (property.PropertyInfo.PropertyType == typeof(DateTime))
            {
                CreateDateControl(property, parent, row, column, bindingPathPrefix, style);
                return;
            }

            CreateTextControl(property, parent, row, column, bindingPathPrefix, style);
            return;
        }

        private static void CreateTextControl(DisplayPropertyInfo property, Grid parent, int row, int column, string bindingPathPrefix, Style style)
        {
            UiTool.CreateField(parent,
                property.PropertyName,
                property.Name,
                bindingPathPrefix + "/" + property.PropertyName,
                style,
                row,
                column);
        }

        private static void CreateDateControl(DisplayPropertyInfo property, Grid parent, int row, int column, string bindingPathPrefix, Style style)
        {
            UiTool.CreateDateField(parent,
                property.PropertyName,
                property.Name,
                bindingPathPrefix + "/" + property.PropertyName,
                null,
                row,
                column);
        }

        private static void CreateEnumControl(DisplayPropertyInfo property, Grid parent, int row, int column, string bindingPathPrefix, Style style)
        {
            UiTool.CreateEnumField(parent,
                property.PropertyInfo.PropertyType,
                property.PropertyName,
                property.Name,
                bindingPathPrefix + "/" + property.PropertyName,
                style,
                row,
                column);
        }
    }
}