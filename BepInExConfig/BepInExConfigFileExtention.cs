using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;

namespace BepInExConfig
{
    public static class BepInExConfigFileExtention
    {
        /// <summary>
        /// ConfigFile の Bind<T>(ConfigDefinition, T, ConfigDescription) リフレクション
        /// </summary>
        private static MethodInfo ConfigFileBindMethod =
            typeof(ConfigFile)
            .GetMethods()
            .Where(m => m.Name == "Bind")
            .Where(m => m.IsGenericMethodDefinition)
            .Where(m => m.GetParameters().First().ParameterType == typeof(ConfigDefinition))
            .Where(m => m.GetParameters().Skip(2).FirstOrDefault()?.ParameterType == typeof(ConfigDescription))
            .Single();

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <returns></returns>
        public static T LoadConfig<T>(this ConfigFile config) where T : new()
        {

            var targetMemberList = GetBepInConfigMembers<T>();
            var resultConfig = new T();

            foreach (var elem in targetMemberList)
            {
                var genericMethod = ConfigFileBindMethod.MakeGenericMethod(elem.Attribute.FieldType);
                var param = new object[] { elem.Attribute.ConfigDefinition, elem.Attribute.DefaultValue, elem.Attribute.ConfigDescription };
                var methodResult = genericMethod.Invoke(config, param);
                var valueProp = methodResult.ValuePropInfo();
                if(valueProp != null)
                    elem.MemberInfo.SetUnderlyingValue(resultConfig, valueProp.GetValue(methodResult, null));
            }

            return resultConfig;
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config"></param>
        /// <param name="saveTarget"></param>
        public static void SaveConfig<T>(this ConfigFile config, T saveTarget)
        {
            var targetMemberList = GetBepInConfigMembers<T>();
            foreach (var elem in targetMemberList)
            {
                var genericMethod = ConfigFileBindMethod.MakeGenericMethod(elem.Attribute.FieldType);
                var param = new object[] { elem.Attribute.ConfigDefinition, elem.Attribute.DefaultValue, elem.Attribute.ConfigDescription };
                var methodResult = genericMethod.Invoke(config, param);
                methodResult.ValuePropInfo()?.SetValue(methodResult, elem.MemberInfo.GetUnderlyingValue(saveTarget), null);
            }

            config.Save();
        }

        /// <summary>
        /// クラスから設定用のフィールド/プロパティを抽出する
        /// クラスに BepInExConfigAttribute が設定されてなかったら例外
        /// メンバーの型と DefaultValue の型が同じ出ない場合例外
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static IEnumerable<MemberAndAttributeInfo> GetBepInConfigMembers<T>()
        {
            if (!Attribute.IsDefined(typeof(T), typeof(BepInExConfigAttribute)))
                throw new ArgumentException("The BepInExConfig attribute has not been assigned to the configuration class / struct.");

            var resultList =
            typeof(T)
            .GetMembers()
            .Where(member => member.MemberType == MemberTypes.Field || member.MemberType == MemberTypes.Property)
            .Where(info => Attribute.IsDefined(info, typeof(BepInExConfigMemberAttribute)))
            .Select(info => new MemberAndAttributeInfo
            {
                MemberInfo = info,
                Attribute = (BepInExConfigMemberAttribute)info.GetCustomAttributes(typeof(BepInExConfigMemberAttribute), false).First()
            })
            .OrderBy(x => x.Attribute.Order);

            var diffTypeMembers = resultList.Where(info => info.MemberInfo.GetUnderlyingType() != info.Attribute.FieldType);
            if (diffTypeMembers.Any())
                throw new ArgumentException($"There is a different type of members than the DefaultValue type of the attribute. : {string.Join(",", diffTypeMembers.Select(m => m.MemberInfo.Name).ToArray())}");

            return resultList;
        }

        private class MemberAndAttributeInfo
        {
            public MemberInfo MemberInfo { get; set; }

            public BepInExConfigMemberAttribute Attribute { get; set; }
        }

        /// <summary>
        /// ValuePropを取得
        /// </summary>
        /// <param name="bindResult"></param>
        /// <returns></returns>
        private static PropertyInfo ValuePropInfo(this object bindResult) =>
            bindResult
            .GetType()
            .GetProperty("Value");

        /// <summary>
        /// Member (Property or Field) の型を取得
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member)
            {
                case FieldInfo fInfo:
                    return fInfo.FieldType;
                case PropertyInfo pInfo:
                    return pInfo.PropertyType;
                default:
                    throw new ArgumentException("Input MemberInfo must be if type FieldInfo or PropertyInfo");
            }
        }

        /// <summary>
        /// MemberInfo(Property or Field) から値を取得
        /// </summary>
        /// <param name="member"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        private static void SetUnderlyingValue(this MemberInfo member, object obj, object value)
        {
            switch (member)
            {
                case FieldInfo fInfo:
                    fInfo.SetValue(obj, value);
                    break;
                case PropertyInfo pInfo:
                    pInfo.SetValue(obj, value, null);
                    break;
                default:
                    throw new ArgumentException("Input MemberInfo must be if type FieldInfo or PropertyInfo");
            }
        }

        /// <summary>
        /// MemberInfo(Property or Field) に値を設定
        /// </summary>
        /// <param name="member"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetUnderlyingValue(this MemberInfo member, object obj)
        {
            switch (member)
            {
                case FieldInfo fInfo:
                    return fInfo.GetValue(obj);
                case PropertyInfo pInfo:
                    return pInfo.GetValue(obj, null);
                default:
                    throw new ArgumentException("Input MemberInfo must be if type FieldInfo or PropertyInfo");
            }
        }

    }
}
