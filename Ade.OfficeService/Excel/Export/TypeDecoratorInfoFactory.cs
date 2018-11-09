﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Ade.OfficeService.Excel
{
    public static class TypeDecoratorInfoFactory
    {
        private static readonly Hashtable Table = Hashtable.Synchronized(new Hashtable(1024));
        public static  TypeDecoratorInfo CreateInstance(Type exportType)
        {
            if (exportType == null)
            {
                throw new ArgumentNullException("importDTOType");
            }

            var key = exportType;
            if (Table[key] != null)
            {
                return (TypeDecoratorInfo)Table[key];
            }

            TypeDecoratorInfo typeDecoratorInfo = new TypeDecoratorInfo() { TypeDecoratorAttrs = new List<BaseDecorateAttribute>() { }, PropertyDecoratorInfos = new List<PropertyDecoratorInfo>() { } };

            //全局装饰特性
            typeDecoratorInfo.TypeDecoratorAttrs.AddRange(exportType.GetCustomAttributes<BaseDecorateAttribute>());

            //列装饰特性
            List<PropertyInfo> props = exportType.GetProperties().ToList().Where(p => p.IsDefined(typeof(ExcelExportAttribute))).ToList();

            for (int i = 0; i < props.Count(); i++)
            {
                typeDecoratorInfo.PropertyDecoratorInfos.Add(
                    new PropertyDecoratorInfo
                    {
                        ColIndex = i,
                        DecoratorAttrs = props[i].GetCustomAttributes<BaseDecorateAttribute>()?.ToList()
                    });
            }

            Table[key] = typeDecoratorInfo;

            return typeDecoratorInfo;
        }
    }
}
