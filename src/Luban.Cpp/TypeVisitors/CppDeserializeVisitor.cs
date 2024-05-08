using Luban.Types;
using Luban.TypeVisitors;

namespace Luban.Cpp.TypeVisitors;

public class CppDeserializeVisitor : DecoratorFuncVisitor<string, string, string>
{
    public static CppDeserializeVisitor Ins { get; } = new CppDeserializeVisitor();

    public override string DoAccept(TType type, string bufName, string fieldName)
    {
        if (type.IsNullable)
        {
            return $"{{\n" +
                   $"\tbool _has_value_; if (!{bufName}.readBool(_has_value_)){{return false;}}\n" +
                   $"\tif (_has_value_) {{ {fieldName}.reset(new {type.Apply(CppUnderlyingDeclaringTypeNameVisitor.Ins)}); {type.Apply(CppUnderlyingDeserializeVisitor.Ins, bufName, $"*{fieldName}")}}}\n" +
                   $"}}";
        }
        
        return type.Apply(CppUnderlyingDeserializeVisitor.Ins, bufName, fieldName);
    }
    
    public override string Accept(TBean type, string bufName, string fieldName)
    {
        return type.Apply(CppUnderlyingDeserializeVisitor.Ins, bufName, fieldName);
    }
}
