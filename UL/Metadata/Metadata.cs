﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metadata
{
    //public class DB_TypeRef
    //{
    //    //数据库全名
    //    public string identifer;
    //    //类型参数
    //    public List<DB_TypeRef> parameters;
    //    //模板类型名
    //    public string template_parameter_name;

    //    public override string ToString()
    //    {
    //        return DB.WriteObject(this);
    //    }

    //    public static DB_TypeRef Void
    //    {
    //        get
    //        {
    //            return new DB_TypeRef() { identifer = "void" };
    //        }
    //    }

    //    public bool IsVoid
    //    {
    //        get
    //        {
    //            return identifer == "void";
    //        }
    //    }

    //    public DB_TypeRef()
    //    {
    //        identifer = "void";
    //        parameters = new List<DB_TypeRef>();
    //        template_parameter_name = "";
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj is DB_TypeRef)
    //        {
    //            DB_TypeRef v = obj as DB_TypeRef;
    //            if (v.identifer != identifer)
    //            {
    //                return false;
    //            }
    //            if (v.parameters.Count != parameters.Count)
    //                return false;
    //            for (int i = 0; i < v.parameters.Count; i++)
    //            {
    //                if (parameters[i] != v.parameters[i])
    //                    return false;
    //            }
    //            if (template_parameter_name != v.template_parameter_name)
    //                return false;

    //            return true;
    //        }

    //        return base.Equals(obj);
    //    }

    //    public static bool operator ==(DB_TypeRef a, DB_TypeRef b)
    //    {
    //        if (!System.Object.ReferenceEquals(a, null))
    //            return a.Equals(b);
    //        if (!System.Object.ReferenceEquals(b, null))
    //            return b.Equals(a);
    //        return System.Object.ReferenceEquals(a, b);
    //    }

    //    public static bool operator !=(DB_TypeRef a, DB_TypeRef b)
    //    {
    //        return !(a == b);
    //    }

    //    public override int GetHashCode()
    //    {
    //        int code = identifer.GetHashCode();
    //        foreach (var p in parameters)
    //        {
    //            code ^= p.GetHashCode();
    //        }
    //        code ^= template_parameter_name.GetHashCode();

    //        return code;
    //    }
    //}
    public class GenericParameterDefinition
    {
        public string type_name;    //类型占位符名称
        public List<Expression.TypeSyntax> constraint = new List<Expression.TypeSyntax>();    //类型约束
    }

    public class DB_Type
    {
        //依据全名查找类的委托
        public delegate DB_Type delFindType(string full_name);
        public static delFindType Find;


        //存储的数据库的全局唯一名
        public string static_full_name
        {
            get
            {
                return GetRefType().GetStaticFullName();
            }
        }


        //命名空间下的唯一名
        public string unique_name
        {
            get
            {
                return GetRefType().GetUniqueName();
            }
        }
        //返回此类的引用
        public Expression.TypeSyntax GetRefType()
        {
            //string[] ns_list = _namespace.Split(',');
            //Expression.QualifiedNameSyntax root_ns = null;
            //Expression.QualifiedNameSyntax last = null;
            //for (int i=0;i<ns_list.Length;i++)
            //{
            //    Expression.QualifiedNameSyntax qas = new Expression.QualifiedNameSyntax();
            //    qas.Left = last;
            //    qas.Right = new Expression.IdentifierNameSyntax() { Identifier = ns_list[i] };

            //    if(last == null)
            //    {
            //        root_ns = qas;
            //    }
            //    last = qas;
            //}
            Expression.TypeSyntax ts = null;
            if (is_generic_paramter)
            {
                ts = new Expression.GenericParameterSyntax() { Name = name, declare_type = declare_type };
            }
            else if(is_generic_type)
            {
                Expression.GenericNameSyntax syntax = new Expression.GenericNameSyntax();
                syntax.Name = name;
                syntax.name_space = _namespace;
                foreach (var a in generic_parameters)
                    syntax.Arguments.Add(a);
                ts = syntax;
            }

            else if (is_generic_type_definition)
            {
                Expression.GenericNameSyntax syntax = new Expression.GenericNameSyntax();
                syntax.Name = name;
                syntax.name_space = _namespace;
                foreach (var a in generic_parameter_definitions)
                    syntax.Arguments.Add(new Expression.GenericParameterSyntax() { Name = name, declare_type = this.declare_type });
                ts = syntax;
            }
            else
            {
                ts = new Expression.IdentifierNameSyntax() { Name = name,name_space = _namespace };
            }

            return ts;
        }
        public string _namespace;
        public string name;
        public string comments = "";
        public int modifier;
        public bool is_abstract;
        public Expression.TypeSyntax base_type = Expression.TypeSyntax.Void;
        public List<string> usingNamespace = new List<string>();
        public string ext = "";
        public bool is_value_type;
        public bool is_interface;
        public bool is_class;
        public bool is_enum;
        public List<Expression.TypeSyntax> interfaces = new List<Expression.TypeSyntax>();
        public bool is_generic_type_definition;
        public List<GenericParameterDefinition> generic_parameter_definitions = new List<GenericParameterDefinition>();

        //动态类型
        public string declare_type; //动态类型引用的类型

        public bool is_generic_type;
        public List<Expression.TypeSyntax> generic_parameters = new List<Expression.TypeSyntax>();

        public bool is_generic_paramter;
        public int generic_parameter_position;
        

        public Dictionary<string, DB_Member> members = new Dictionary<string, DB_Member>();

        //public static string GetNamespace(string full_name)
        //{
        //    int generic_def_mark = full_name.IndexOf("[");
        //    int generic_mark = full_name.IndexOf("<");
            
        //    if (generic_def_mark >= 0)
        //        full_name = full_name.Substring(0, generic_def_mark);
        //    else if(generic_mark>=0)
        //        full_name = full_name.Substring(0, generic_mark);

        //    int name_space_mark = full_name.LastIndexOf(".");
        //    if (name_space_mark>=0)
        //        return full_name.Substring(0, name_space_mark);
        //    return "";
        //}

        //public static string GetName(string full_name)
        //{
        //    int generic_def_mark = full_name.IndexOf("[");
        //    int generic_mark = full_name.IndexOf("<");

        //    if (generic_def_mark >= 0)
        //        full_name = full_name.Substring(0, generic_def_mark);
        //    else if (generic_mark >= 0)
        //        full_name = full_name.Substring(0, generic_mark);

        //    int name_space_mark = full_name.LastIndexOf(".");
        //    if (name_space_mark >= 0)
        //        return full_name.Substring(name_space_mark + 1);
        //    return full_name;
        //}
        //public static int ParseGenericParameterCount(string full_name)
        //{
        //    int generic_def_mark = full_name.IndexOf("[");
        //    int generic_mark = full_name.IndexOf("<");

        //    if(generic_def_mark>=0)
        //    {
        //        return int.Parse(full_name.Substring(generic_def_mark + 1, full_name.IndexOf(']', generic_def_mark)));
        //    }

        //    if(generic_mark>0)
        //    {
        //        return ParseGenericParameters(full_name).Count;
        //    }

        //    return 0;
        //}
        //public static string GetGenericDefinitionName(string generic_type_name)
        //{
        //    int generic_def_mark = generic_type_name.IndexOf("[");
        //    int generic_mark = generic_type_name.IndexOf("<");
        //    if (generic_def_mark >= 0)
        //        return generic_type_name;
        //    if (generic_mark < 0)
        //        return generic_type_name;

        //    List<string> paramters = ParseGenericParameters(generic_type_name);
        //    if(paramters.Count>0)
        //    {
        //        return generic_type_name.Substring(0, generic_mark) + "[" + paramters.Count + "]";
        //    }
        //    return generic_type_name;
        //}

        //public static bool GetDeclareTypeName(string full_name,out string declare_type,out string name)
        //{
        //    int declare_type_ref_mark = full_name.IndexOf(":");
        //    if (declare_type_ref_mark < 0)
        //    {
        //        declare_type = "";
        //        name = "";
        //        return false;
        //    }
                
        //    declare_type = full_name.Substring(0, declare_type_ref_mark);
        //    name = full_name.Substring(declare_type_ref_mark + 1);
        //    return true;
        //}

        //public static List<string> ParseGenericParameters(string full_name)
        //{
        //    List<string> results = new List<string>();
        //    int generic_mark = full_name.IndexOf("<");
        //    if (generic_mark < 0)
        //        return results;

        //    string generic_string = full_name.Substring(generic_mark+1,full_name.Length- generic_mark-2);

        //    int depth = 0;
        //    StringBuilder sb = new StringBuilder();
        //    for(int i=0;i<generic_string.Length;i++)
        //    {
        //        if(generic_string[i] == ',')
        //        {
        //            if(depth==0)
        //            {
        //                results.Add(sb.ToString());
        //                sb.Clear();
        //            }
        //            else
        //            {
        //                sb.Append(generic_string[i]);
        //            }
        //        }
        //        else if(generic_string[i] == '<')
        //        {
        //            depth++;
        //            sb.Append(generic_string[i]);
        //        }
        //        else if(generic_string[i] == '>')
        //        {
        //            depth--;
        //            sb.Append(generic_string[i]);
        //        }
        //        else
        //        {
        //            sb.Append(generic_string[i]);
        //        }
        //    }
        //    if(sb.Length>0)
        //        results.Add(sb.ToString());
        //    return results;
        //}

        string genericDefinitionString
        {
            get
            {
                if(is_generic_type_definition)
                {
                    
                    return "[" + generic_parameter_definitions.Count+"]";
                }

                return "";
            }
        }
        string genericString
        {
            get
            {
                if(is_generic_type)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<");
                    for (int i = 0; i < generic_parameters.Count; i++)
                    {
                        sb.Append(generic_parameters[i]);
                        if (i < generic_parameters.Count - 1)
                            sb.Append(",");
                    }
                    sb.Append(">");
                    return sb.ToString();
                }
                return "";
            }
        }

        public static DB_Type MakeGenericType(DB_Type genericTypeDef, List<Expression.TypeSyntax> genericParameters)
        {
            DB_Type dB_Type = DB.ReadObject<DB_Type>(DB.WriteObject(genericTypeDef));
            dB_Type.is_generic_type_definition = false;
            dB_Type.is_generic_type = true;
            dB_Type.declare_type = genericTypeDef.static_full_name;
            //dB_Type.generic_parameter_definitions.Clear();
            dB_Type.generic_parameters.AddRange(genericParameters);
            return dB_Type;
        }

        public static DB_Type MakeGenericParameterType(DB_Type declare_type, GenericParameterDefinition def)
        {
            DB_Type dB_Type = DB.ReadObject<DB_Type>(DB.WriteObject(declare_type));
            dB_Type.is_generic_paramter = true;
            dB_Type.is_generic_type_definition = false;
            dB_Type.generic_parameter_position = declare_type.generic_parameter_definitions.FindIndex((a) => { return a.type_name == def.type_name; });
            //dB_Type._namespace = dB_Type._namespace + "." + dB_Type.name;
            dB_Type.name = def.type_name;
            dB_Type.declare_type = declare_type.static_full_name;

            return dB_Type;
        }

        public DB_Member FindMethod(string name,List<DB_Type> typeParameters)
        {
            foreach(var m in members.Values)
            {
                if (m.name != name)
                    continue;
                if(m.member_type == (int)MemberTypes.Constructor || m.member_type == (int)MemberTypes.Method)
                {
                    if(m.method_args.Length == typeParameters.Count)
                    {
                        for(int i=0;i< m.method_args.Length;i++)
                        {
                            if (m.method_args[i].type == typeParameters[i].GetRefType())
                                return m;
                        }
                    }
                }
            }

            return null;
        }
    }

    public class DB_Member
    {
        public string declaring_type;
        public string name;
        public int order;   //字段序号
        public bool is_static;
        public int modifier;
        public string comments = "";
        //public int id;
        public int member_type;
        public string ext = "";
        //*****************变量***********************/
        public Expression.TypeSyntax field_type  = Expression.TypeSyntax.Void;
        public Expression.Exp field_initializer;
        //********************************************/

        //*****************方法***********************/
        public class Argument
        {
            public Metadata.Expression.TypeSyntax type;
            public string name;
            public bool is_ref;
            public bool is_out;
            public string default_value = "";
        }
        public Argument[] method_args;

        public Expression.TypeSyntax method_ret_type = Expression.TypeSyntax.Void;
        //泛型参数
        public List<GenericParameterDefinition> method_generic_parameter_definitions = new List<GenericParameterDefinition>();

        public DB_BlockSyntax method_body;
        //********************************************/

        //签名，一个类唯一
        public string identifier
        {
            get
            {
                if(member_type == (int)MemberTypes.Field)
                {
                    return name;
                }
                else if(member_type == (int)MemberTypes.Method || member_type == (int)MemberTypes.Constructor)
                {
                    StringBuilder sb = new StringBuilder(name);

                    if (method_generic_parameter_definitions.Count > 0)
                    {
                        sb.Append("<");
                        for (int i = 0; i < method_generic_parameter_definitions.Count; i++)
                        {
                            sb.Append(method_generic_parameter_definitions[i].type_name);
                            if (i < method_generic_parameter_definitions.Count - 1)
                                sb.Append(",");
                        }
                        sb.Append(">");
                    }

                    sb.Append("(");
                    for (int i = 0; i < method_args.Length; i++)
                    {
                        sb.Append(method_args[i].type.ToString());
                        if (method_args[i].is_ref)
                            sb.Append(" ref");
                        if(method_args[i].is_out)
                            sb.Append(" out");
                        if (i < method_args.Length - 1)
                            sb.Append(",");
                    }
                    sb.Append(")");
                    
                    return sb.ToString();
                }
                else
                {
                    return name;
                }
            }
        }
    
        public Expression.TypeSyntax typeName
        {
            get
            {
                if (member_type == (int)MemberTypes.Field)
                    return field_type;
                else if (member_type == (int)MemberTypes.Method)
                    return method_ret_type;

                return Expression.TypeSyntax.Void;
            }
        }
    }


    public class JsonConverterType<TBase>:Newtonsoft.Json.JsonConverter where TBase:class
    {
        //
        // 摘要:
        //     Determines whether this instance can convert the specified object type.
        //
        // 参数:
        //   objectType:
        //     Type of the object.
        //
        // 返回结果:
        //     true if this instance can convert the specified object type; otherwise, false.
        public override bool CanConvert(Type objectType)
        {
            if (objectType.IsArray)
                return false;
            return typeof(TBase).IsAssignableFrom(objectType);
        }
        public override bool CanWrite
        {
            get
            {
                return false;   
            }
        }
        //
        // 摘要:
        //     Reads the JSON representation of the object.
        //
        // 参数:
        //   reader:
        //     The Newtonsoft.Json.JsonReader to read from.
        //
        //   objectType:
        //     Type of the object.
        //
        //   existingValue:
        //     The existing value of object being read.
        //
        //   serializer:
        //     The calling serializer.
        //
        // 返回结果:
        //     The object value.
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var target = Create(objectType, jsonObject);
            if (target == null)
                return null;
            serializer.Populate(jsonObject.CreateReader(), target);
            return target;
        }

        object Create(Type objectType, JObject jsonObject)
        {
            JToken jToken = null;
            if(jsonObject.TryGetValue("$Type",out jToken))
            {
                string typeName = jToken.ToString();
                Type trueType = Type.GetType(typeName);
                object trueObject = System.Activator.CreateInstance(trueType);
                //if(!(trueObject is TBase))
                //{
                //    Console.WriteLine(string.Format("无法转换 {0} => {1}", trueObject.GetType().FullName,typeof(TBase).FullName));
                //}
                return trueObject;
            }
            return System.Activator.CreateInstance(objectType);
        }

        //
        // 摘要:
        //     Writes the JSON representation of the object.
        //
        // 参数:
        //   writer:
        //     The Newtonsoft.Json.JsonWriter to write to.
        //
        //   value:
        //     The value.
        //
        //   serializer:
        //     The calling serializer.
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
            //serializer.Serialize(writer, value);
            //JObject jo = JObject.FromObject(value);
            //jo.Add("$type", value.GetType().FullName);
            //jo.WriteTo(writer, serializer.Converters.ToArray());
        }
    }

    [JsonConverter(typeof(JsonConverterType<DB_Syntax>))]
    public class DB_Syntax
    {
        [JsonProperty("$Type")]
        public string JsonType
        {
            get
            {
                return GetType().FullName;
            }
        }
    }

    //语句
    [JsonConverter(typeof(JsonConverterType<DB_StatementSyntax>))]
    public class DB_StatementSyntax:DB_Syntax
    {
        
    }
    [JsonConverter(typeof(JsonConverterType<DB_BlockSyntax>))]
    public class DB_BlockSyntax: DB_StatementSyntax
    {
        public List<DB_StatementSyntax> List = new List<DB_StatementSyntax>();
    }
    [JsonConverter(typeof(JsonConverterType<DB_IfStatementSyntax>))]
    public class DB_IfStatementSyntax:DB_StatementSyntax
    {
        public Expression.Exp Condition;
        public DB_StatementSyntax Statement;
        public DB_StatementSyntax Else;
    }
    [JsonConverter(typeof(JsonConverterType<DB_ExpressionStatementSyntax>))]
    public class DB_ExpressionStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Exp;
    }
    [JsonConverter(typeof(JsonConverterType<DB_LocalDeclarationStatementSyntax>))]
    public class DB_LocalDeclarationStatementSyntax : DB_StatementSyntax
    {
        public VariableDeclarationSyntax Declaration = new VariableDeclarationSyntax();
    }
    [JsonConverter(typeof(JsonConverterType<VariableDeclarationSyntax>))]
    public sealed class VariableDeclarationSyntax : DB_Syntax
    {
        public Expression.TypeSyntax Type;
        public List<VariableDeclaratorSyntax> Variables = new List<VariableDeclaratorSyntax>();
    }
    [JsonConverter(typeof(JsonConverterType<VariableDeclaratorSyntax>))]
    public class VariableDeclaratorSyntax : DB_Syntax
    {
        public string Identifier;
        public Expression.Exp Initializer;
    }

    [JsonConverter(typeof(JsonConverterType<DB_ForStatementSyntax>))]
    public class DB_ForStatementSyntax : DB_StatementSyntax
    {
        public VariableDeclarationSyntax Declaration;
        public Expression.Exp Condition;
        public List<Expression.Exp> Incrementors = new List<Expression.Exp>();
        public DB_StatementSyntax Statement;
    }

    [JsonConverter(typeof(JsonConverterType<DB_DoStatementSyntax>))]
    public class DB_DoStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Condition;
        public DB_StatementSyntax Statement;
    }
    [JsonConverter(typeof(JsonConverterType<DB_WhileStatementSyntax>))]
    public class DB_WhileStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Condition;
        public DB_StatementSyntax Statement;
    }
    [JsonConverter(typeof(JsonConverterType<DB_SwitchStatementSyntax>))]
    public class DB_SwitchStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Expression;
        public List<SwitchSectionSyntax> Sections = new List<SwitchSectionSyntax>();

        public class SwitchSectionSyntax
        {
            public List<Expression.Exp> Labels = new List<Expression.Exp>();
            public List<DB_StatementSyntax> Statements = new List<DB_StatementSyntax>();
        }
    }



    [JsonConverter(typeof(JsonConverterType<DB_BreakStatementSyntax>))]
    public class DB_BreakStatementSyntax : DB_StatementSyntax
    {

    }

    [JsonConverter(typeof(JsonConverterType<DB_ReturnStatementSyntax>))]
    public class DB_ReturnStatementSyntax : DB_StatementSyntax
    {
        public Expression.Exp Expression;
    }

    namespace Expression
    {
        [JsonConverter(typeof(JsonConverterType<Exp>))]
        public class Exp : DB_Syntax
        {
        }
        [JsonConverter(typeof(JsonConverterType<MethodExp>))]
        public class MethodExp : Exp
        {
            //调用函数的对象，或者类，如果为null，表示创建Name类型的对象
            public Exp Caller;
            //调用的函数名
            public string Name;

            //调用的参数
            public List<Exp> Args = new List<Exp>();
        }
        [JsonConverter(typeof(JsonConverterType<FieldExp>))]
        public class FieldExp : Exp
        {
            //调用函数的对象，或者类，如果为null，表示访问本地变量，成员变量，全局类
            public Exp Caller;
            //调用的函数名
            public string Name;
        }

        [JsonConverter(typeof(JsonConverterType<IndifierExp>))]
        public class IndifierExp:Exp
        {
            public string Name;
        }

        //常量表达式(分为字符常量或者数值常量)
        [JsonConverter(typeof(JsonConverterType<ConstExp>))]
        public class ConstExp : Exp
        {
            public string value;
        }


        //变量访问表达式（可能是本地变量，成员变量，类）
        //public class VariableExp : Exp
        //{
        //    public string Name;
        //}

        //对象创建表达式
        [JsonConverter(typeof(JsonConverterType<ObjectCreateExp>))]
        public class ObjectCreateExp : Exp
        {
            //类型名称
            public Expression.TypeSyntax Type;
            //调用的参数
            public List<Exp> Args = new List<Exp>();
        }
        [JsonConverter(typeof(JsonConverterType<TypeSyntax>))]
        public abstract class TypeSyntax : Exp
        {
            public string name_space = "";
            public string Name = "";

            [JsonIgnore]
            public bool IsVoid
            {
                get { return this == Void; }
            }
            [JsonIgnore]
            public static TypeSyntax Void
            {
                get
                {
                    return new IdentifierNameSyntax() { Name = "void",name_space = "System" };
                }
            }

            public static bool operator ==(TypeSyntax a, TypeSyntax b)
            {
                if (!System.Object.ReferenceEquals(a, null))
                    return a.Equals(b);
                if (!System.Object.ReferenceEquals(b, null))
                    return b.Equals(a);
                return System.Object.ReferenceEquals(a, b);
            }

            public static bool operator !=(TypeSyntax a, TypeSyntax b)
            {
                return !(a == b);
            }

            public override bool Equals(object obj)
            {
                throw new NotImplementedException();
            }

            public override int GetHashCode()
            {
                throw new NotImplementedException();
            }

            //数据库的全名
            public abstract string GetStaticFullName();
            //命名空间下唯一名
            public abstract string GetUniqueName();
            //全名
            protected abstract string ToFullName();
            //全名
            public override string ToString()
            {
                return ToFullName();
            }
        }
        //[JsonConverter(typeof(JsonConverterType<NameSyntax>))]
        //public abstract class NameSyntax :TypeSyntax
        //{
        //    //public int Arity;
        //}
        //[JsonConverter(typeof(JsonConverterType<SimpleNameSyntax>))]
        //public abstract class SimpleNameSyntax:NameSyntax
        //{
        //    public string Name = "";

            
        //}

        [JsonConverter(typeof(JsonConverterType<IdentifierNameSyntax>))]
        public class IdentifierNameSyntax: TypeSyntax
        {
            
            public override bool Equals(object obj)
            {
                if (obj is IdentifierNameSyntax)
                {
                    IdentifierNameSyntax Other = obj as IdentifierNameSyntax;
                    return Other.Name == Name && Other.name_space == name_space;
                }

                return false;
            }

            public override int GetHashCode()
            {
                return name_space.GetHashCode() ^ Name.GetHashCode();
            }

            public override string GetStaticFullName()
            {
                return name_space + "." + Name;
            }
            public override string GetUniqueName()
            {
                return Name;
            }
            protected override string ToFullName()
            {
                return GetStaticFullName();
            }
        }

        [JsonConverter(typeof(JsonConverterType<GenericParameterSyntax>))]
        public class GenericParameterSyntax:TypeSyntax
        {
            public string declare_type;
            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is GenericParameterSyntax)
                {
                    GenericParameterSyntax Other = obj as GenericParameterSyntax;
                    return Other.Name == Name && declare_type == Other.declare_type;
                }

                return false;
            }

            public override string GetStaticFullName()
            {
                return declare_type;
            }
            public override string GetUniqueName()
            {
                return Name;
            }
            protected override string ToFullName()
            {
                return GetStaticFullName();
            }
        }

        [JsonConverter(typeof(JsonConverterType<GenericNameSyntax>))]
        public class GenericNameSyntax: TypeSyntax
        {
            public List<TypeSyntax> Arguments = new List<TypeSyntax>();

            public override bool Equals(object obj)
            {
                if (obj is GenericNameSyntax)
                {
                    GenericNameSyntax other = obj as GenericNameSyntax;

                    if (other.Arguments.Count != Arguments.Count)
                        return false;
                    for (int i = 0; i < Arguments.Count; i++)
                        if (Arguments[i] != other.Arguments[i])
                            return false;

                    if (other.Name != Name)
                        return false;
                    return true;
                }

                return false;
            }

            public override int GetHashCode()
            {
                int id_hash = Name.GetHashCode();
                foreach (var a in Arguments)
                    id_hash ^= a.GetHashCode();

                return id_hash;
            }

            public override string GetStaticFullName()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name_space);
                sb.Append(".");
                sb.Append(GetUniqueName());
                return sb.ToString();
            }
            public override string GetUniqueName()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Name);
                sb.Append("[");
                sb.Append(Arguments.Count);
                sb.Append("]");
                return sb.ToString();
            }

            protected override string ToFullName()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(name_space);
                sb.Append(".");
                sb.Append(Name);
                sb.Append("<");
                for (int i = 0; i < Arguments.Count; i++)
                {
                    sb.Append(Arguments[i].ToString());
                    if (i < Arguments.Count - 1)
                        sb.Append(",");
                }
                sb.Append(">");
                return sb.ToString();
            }
        }

        //[JsonConverter(typeof(JsonConverterType<QualifiedNameSyntax>))]
        //public class QualifiedNameSyntax:NameSyntax
        //{
        //    public NameSyntax Left;

        //    public SimpleNameSyntax Right;


        //    public override bool Equals(object obj)
        //    {
        //        if (obj is QualifiedNameSyntax)
        //        {
        //            QualifiedNameSyntax other = obj as QualifiedNameSyntax;

        //            if (other.Left!=Left)
        //                return false;

        //            if (other.Right != Right)
        //                return false;
        //            return true;
        //        }

        //        return false;
        //    }

        //    public override int GetHashCode()
        //    {
        //        int id_hash = Left!=null? Left.GetHashCode():1;
        //        int id_right = Right != null ? Right.GetHashCode() : 1;

        //        return id_hash ^ id_right;
        //    }
        //}
    }




    ////表达式
    //[JsonConverter(typeof(JsonConverterType<DB_ExpressionSyntax>))]
    //public class DB_ExpressionSyntax : DB_Syntax
    //{
    //}


    //[JsonConverter(typeof(JsonConverterType<VariableDeclaratorSyntax>))]
    //public class VariableDeclaratorSyntax : DB_Syntax
    //{
    //    public string Identifier;
    //    //public List<DB_ArgumentSyntax> ArgumentList = new List<DB_ArgumentSyntax>();
    //    public DB_ExpressionSyntax Initializer;
    //}

    //[JsonConverter(typeof(JsonConverterType<DB_LiteralExpressionSyntax>))]
    //public class DB_LiteralExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public string token;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_MemberAccessExpressionSyntax>))]
    //public class DB_MemberAccessExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public DB_ExpressionSyntax Exp;
    //    public string name;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_ArgumentSyntax>))]
    //public class DB_ArgumentSyntax:DB_ExpressionSyntax
    //{
    //    public DB_ExpressionSyntax Expression;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_InvocationExpressionSyntax>))]
    //public class DB_InvocationExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public DB_ExpressionSyntax Exp;
    //    public List<DB_ArgumentSyntax> Arguments = new List<DB_ArgumentSyntax>();

    //}
    //[JsonConverter(typeof(JsonConverterType<DB_IdentifierNameSyntax>))]
    //public class DB_IdentifierNameSyntax : DB_ExpressionSyntax
    //{
    //    public string Name;
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_InitializerExpressionSyntax>))]
    //public class DB_InitializerExpressionSyntax : DB_ExpressionSyntax
    //{
    //    public List<DB_ExpressionSyntax> Expressions = new List<DB_ExpressionSyntax>();
    //}
    //[JsonConverter(typeof(JsonConverterType<DB_ObjectCreationExpressionSyntax>))]
    //public class DB_ObjectCreationExpressionSyntax:DB_ExpressionSyntax
    //{
    //    public string Type;
    //    public List<DB_ArgumentSyntax> Arguments = new List<DB_ArgumentSyntax>();
    //    public DB_InitializerExpressionSyntax Initializer;
    //}

    //
    // 摘要:
    //     
    public enum MemberTypes
    {
        //
        // 摘要:
        //     指定该成员是一个构造函数
        Constructor = 1,
        //
        // 摘要:
        //     指定该成员是一个事件。
        Event = 2,
        //
        // 摘要:
        //     指定该成员是一个字段。
        Field = 4,
        //
        // 摘要:
        //     指定该成员是一种方法。
        Method = 8,
        //
        // 摘要:
        //     指定成员是属性。
        Property = 16,
        //
        // 摘要:
        //     指定该成员是一种类型。
        TypeInfo = 32,
        //
        // 摘要:
        //     指定该成员是自定义成员的指针类型。
        EnumMember = 64,
        
    }

    public enum Modifier
    {
        Public,
        Private,
        Protected
    }

    public class DB
    {

        public static T ReadObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default(T);
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            //jsetting.Converters.Add(new JsonConverterType<T>());
            return JsonConvert.DeserializeObject<T>(json, jsetting);
        }
        public static string WriteObject<T>(T v)
        {
            if (v == null)
                return "";
            JsonSerializerSettings jsetting = new JsonSerializerSettings();
            jsetting.NullValueHandling = NullValueHandling.Ignore;
            //jsetting.Converters.Add(new JsonConverterType<T>());
            return JsonConvert.SerializeObject(v, jsetting);
        }


        public static int MakeModifier(bool isPublic, bool isPrivate, bool isProtected)
        {
            if (isPublic)
                return (int)Modifier.Public;
            else if (isPrivate)
                return (int)Modifier.Private;
            else if (isProtected)
                return (int)Modifier.Protected;
            return (int)Modifier.Public;
        }

        public static void SaveDBType(DB_Type type, OdbcConnection _con, OdbcTransaction _trans)
        {
            {
                string cmdText = string.Format("delete from type where full_name='{0}'", type.static_full_name);
                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.ExecuteNonQuery();
            }

            {
                string cmdText = string.Format("delete from member where declaring_type='{0}'", type.static_full_name);
                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.ExecuteNonQuery();
            }

            {
                string cmdText = "insert into type(full_name,comments,modifier,is_abstract,base_type,ext,is_value_type,is_interface,is_class,interfaces,is_generic_type_definition,generic_parameter_definitions,name,namespace,usingNamespace,is_enum) values(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?);";


                OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
                cmd.Parameters.AddWithValue("1", type.static_full_name);
                cmd.Parameters.AddWithValue("2", type.comments);
                cmd.Parameters.AddWithValue("3", type.modifier);
                cmd.Parameters.AddWithValue("4", type.is_abstract);
                cmd.Parameters.AddWithValue("5", WriteObject(type.base_type));
                cmd.Parameters.AddWithValue("6", type.ext);
                cmd.Parameters.AddWithValue("7", type.is_value_type);
                cmd.Parameters.AddWithValue("8", type.is_interface);
                cmd.Parameters.AddWithValue("9", type.is_class);
                cmd.Parameters.AddWithValue("10", WriteObject( type.interfaces));
                cmd.Parameters.AddWithValue("11", type.is_generic_type_definition);
                cmd.Parameters.AddWithValue("12", WriteObject(type.generic_parameter_definitions));
                cmd.Parameters.AddWithValue("13", type.name);
                cmd.Parameters.AddWithValue("14", type._namespace);
                cmd.Parameters.AddWithValue("15", WriteObject(type.usingNamespace));
                cmd.Parameters.AddWithValue("16", type.is_enum);
                cmd.ExecuteNonQuery();
            }
        }

        public static void SaveDBMember(DB_Member member, OdbcConnection _con, OdbcTransaction _trans)
        {
            //{
            //    string cmdText = string.Format("delete from member where declaring_type='{0}' and name='{1}' and child=?", member.declaring_type, member.name);
            //    OdbcCommand cmd = new OdbcCommand(cmdText, _con, _trans);
            //    cmd.Parameters.AddWithValue("1", member.child);
            //    cmd.ExecuteNonQuery();
            //}

            {
                string CommandText = string.Format("insert into member(declaring_type,identifier,name,comments,modifier,is_static,member_type,ext,field_type,method_args,method_ret_type,method_body,`order`,field_initializer) values(\"{0}\",\"{1}\",\"{2}\",\"{3}\",{4},{5},\"{6}\",\"{7}\",?,?,?,?,?,?);",
                member.declaring_type, member.identifier, member.name, member.comments, member.modifier, member.is_static, member.member_type, member.ext);

                OdbcCommand cmd = new OdbcCommand(CommandText, _con, _trans);

                cmd.Parameters.AddWithValue("1", WriteObject(member.field_type));
                cmd.Parameters.AddWithValue("2", WriteObject( member.method_args));
                cmd.Parameters.AddWithValue("3", WriteObject(member.method_ret_type));
                cmd.Parameters.AddWithValue("4", WriteObject(member.method_body));
                cmd.Parameters.AddWithValue("5", member.order);
                cmd.Parameters.AddWithValue("6", WriteObject(member.field_initializer));
                cmd.ExecuteNonQuery();
            }

        }

        public static Dictionary<string,DB_Type> LoadNamespace(string ns, OdbcConnection _con)
        {
            Dictionary<string, DB_Type> results = new Dictionary<string, DB_Type>();
            string cmdText = string.Format("select * from type where full_name like '{0}%'", ns);
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            //cmd.Parameters.AddWithValue("1", ns);
            var reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                DB_Type type = ReadType(reader);
                type.members = LoadMembers(type.static_full_name, _con);
                results.Add(type.unique_name, type);
            }

            return results;
        }

        public static DB_Type LoadType(string full_name, OdbcConnection _con)
        {
            string cmdText = string.Format("select * from type where full_name = '{0}'", full_name);
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            //cmd.Parameters.AddWithValue("1", ns);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                DB_Type type = ReadType(reader);
                type.members = LoadMembers(type.static_full_name, _con);
                return type;
            }

            return null;
        }

        static DB_Type ReadType(OdbcDataReader reader)
        {
            DB_Type type = new DB_Type();
            //type.full_name = (string)reader["full_name"];
            type.name = (string)reader["name"];
            type._namespace = (string)reader["namespace"];
            type.modifier = (int)reader["modifier"];
            type.comments = (string)reader["comments"];
            type.ext = (string)reader["ext"];
            //type.imports = (string)reader["imports"];
            type.is_abstract = (bool)reader["is_abstract"];
            type.is_interface = (bool)reader["is_interface"];
            type.is_value_type = (bool)reader["is_value_type"];
            type.base_type = DB.ReadObject<Expression.TypeSyntax>((string)reader["base_type"]);
            type.is_class = (bool)reader["is_class"];
            type.interfaces = ReadObject<List<Expression.TypeSyntax>>((string)reader["interfaces"]);
            type.is_generic_type_definition = (bool)reader["is_generic_type_definition"];
            type.generic_parameter_definitions = ReadObject<List<GenericParameterDefinition>>((string)reader["generic_parameter_definitions"]);
            type.usingNamespace = ReadObject<List<string>>((string)reader["usingNamespace"]);
            type.is_enum = (bool)reader["is_enum"];
            return type;
        }

        public static Dictionary<string,DB_Member> LoadMembers(string type, OdbcConnection _con)
        {
            Dictionary<string, DB_Member> results = new Dictionary<string, DB_Member>();
            string cmdText = string.Format("select * from member where declaring_type = ?");
            OdbcCommand cmd = new OdbcCommand(cmdText, _con);
            cmd.Parameters.AddWithValue("1", type);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                DB_Member member = new DB_Member();
                member.declaring_type = type;
                member.comments = (string)reader["comments"];
                member.ext = (string)reader["ext"];
                member.field_type = DB.ReadObject<Expression.TypeSyntax>((string)reader["field_type"]);
                member.is_static = (bool)reader["is_static"];
                member.modifier = (int)reader["modifier"];
                member.name = (string)reader["name"];
                member.member_type = (int)reader["member_type"];
                member.method_args = ReadObject<DB_Member.Argument[]>((string)reader["method_args"]);
                member.method_body = ReadObject<DB_BlockSyntax>((string)reader["method_body"]);
                member.method_ret_type = DB.ReadObject<Expression.TypeSyntax>((string)reader["method_ret_type"]);
                member.order = (int)reader["order"];
                member.field_initializer = ReadObject<Expression.Exp>((string)reader["field_initializer"]);
                results.Add(member.identifier, member);
            }

            return results;
        }
    }
}
