using Luban.Cpp.TemplateExtensions;
using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.Cpp.TypeVisitors;

public class CppUnderlyingDeserializeVisitor : ITypeFuncVisitor<string, string, string>
{
    public static CppUnderlyingDeserializeVisitor Ins { get; } = new CppUnderlyingDeserializeVisitor();

    public string Accept(TBool type, string bufName, string fieldName)
    {
        return $"if (!{bufName}.readBool({fieldName})) return false;";
    }

    public string Accept(TByte type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readByte({fieldName})) return false;";
    }

    public string Accept(TShort type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readShort({fieldName})) return false;";
    }

    public string Accept(TInt type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readInt({fieldName})) return false;";
    }

    public string Accept(TLong type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readLong({fieldName})) return false;";
    }

    public string Accept(TFloat type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readFloat({fieldName})) return false;";
    }

    public string Accept(TDouble type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readDouble({fieldName})) return false;";
    }

    public string Accept(TEnum type, string bufName, string fieldName)
    {
        return $"\n{{\n" +
               $"\tint __enum_temp__;\n" +
               $"\tif(!{bufName}.readInt(__enum_temp__)) return false;\n" +
               $"\t{fieldName} = {CppTemplateExtension.MakeTypeCppName(type.DefEnum)}(__enum_temp__);\n" +
               $"}}";
    }

    public string Accept(TString type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readString({fieldName})) return false;";
    }

    public string Accept(TBean type, string bufName, string fieldName)
    {
        return $"if(!{CppTemplateExtension.MakeTypeCppName(type.DefBean)}::deserialize{type.DefBean.Name}({bufName}, {fieldName})) return false;";
    }

    public string Accept(TDateTime type, string bufName, string fieldName)
    {
        return $"if(!{bufName}.readLong({fieldName})) return false;";
    }

    public string Accept(TArray type, string bufName, string fieldName)
    {
        return $"\n{{\n" +
               $"\t::luban::int32 n;\n" +
               $"\tif(!{bufName}.readSize(n)) return false;\n" +
               $"\tn = std::min(n, ::luban::int32({bufName}.size()));\n" +
               $"\t{fieldName}.reserve(n);\n" +
               $"\tfor(int i = 0 ; i < n ; i++)\n" +
               $"\t{{\n" +
               $"\t\t{type.ElementType.Apply(DeclaringTypeNameVisitor.Ins)} _e;\n" +
               $"\t\t{type.ElementType.Apply(this, bufName, "_e")} {fieldName}.push_back(_e);\n" +
               $"\t}}\n" +
               $"}}";
    }

    public string Accept(TList type, string bufName, string fieldName)
    {
        return $"\n{{\n" +
               $"\t::luban::int32 n;\n" +
               $"\tif(!{bufName}.readSize(n)) return false;\n" +
               $"\tn = std::min(n, ::luban::int32({bufName}.size()));\n" +
               $"\t{fieldName}.reserve(n);\n" +
               $"\tfor(int i = 0 ; i < n ; i++)\n" +
               $"\t{{\n" +
               $"\t\t{type.ElementType.Apply(DeclaringTypeNameVisitor.Ins)} _e;\n" +
               $"\t\t{type.ElementType.Apply(this, bufName, "_e")} {fieldName}.push_back(_e);\n" +
               $"\t}}\n" +
               $"}}";
    }

    public string Accept(TSet type, string bufName, string fieldName)
    {
        return $"\n{{\n" +
               $"\t::luban::int32 n;\n" +
               $"\tif(!{bufName}.readSize(n)) return false;\n" +
               $"\tn = std::min(n, ::luban::int32({bufName}.size()));\n" +
               $"\t{fieldName}.reserve(n * 3 / 2);\n" +
               $"\tfor(int i = 0 ; i < n ; i++)\n" +
               $"\t{{\n" +
               $"\t\t{type.ElementType.Apply(DeclaringTypeNameVisitor.Ins)} _e;\n" +
               $"\t\t{type.ElementType.Apply(this, bufName, "_e")}\n" +
               $"\t\t{fieldName}.insert(_e);\n" +
               $"\t}}\n" +
               $"}}";
    }

    public string Accept(TMap type, string bufName, string fieldName)
    {
        return $"\n{{\n" +
               $"\t::luban::int32 n;\n" +
               $"\tif(!{bufName}.readSize(n)) return false;\n" +
               $"\tn = std::min(n, (::luban::int32){bufName}.size());\n" +
               $"\t{fieldName}.reserve(n * 3 / 2);\n" +
               $"\tfor(int i = 0 ; i < n ; i++)\n" +
               $"\t{{\n" +
               $"\t\t{type.KeyType.Apply(DeclaringTypeNameVisitor.Ins)} _k;\n" +
               $"\t\t{type.ValueType.Apply(DeclaringTypeNameVisitor.Ins)} _v;\n" +
               $"\t\t{type.KeyType.Apply(this, bufName, "_k")}\n"  +
               $"\t\t{type.ValueType.Apply(this, bufName, "_v")}\n" +
               $"\t\t{fieldName}[_k] = _v;\n" +
               $"\t}}\n" +
               $"}}\n";

    }
}
