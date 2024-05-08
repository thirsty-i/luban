using Luban.Types;
using Luban.TypeVisitors;
using Luban.Cpp.TemplateExtensions;

namespace Luban.Cpp.TypeVisitors;

public class DeclaringTypeNameVisitor : DecoratorFuncVisitor<string>
{
    public static DeclaringTypeNameVisitor Ins { get; } = new DeclaringTypeNameVisitor();

    public override string DoAccept(TType type)
    {
        if (type.IsNullable)
            return $"::luban::UniquePtr<{type.Apply(CppUnderlyingDeclaringTypeNameVisitor.Ins)}>";
        
        return type.Apply(CppUnderlyingDeclaringTypeNameVisitor.Ins);
    }

    public override string Accept(TBean type)
    {
        if (type.IsDynamic)
            return $"::luban::UniquePtr<{type.Apply(CppUnderlyingDeclaringTypeNameVisitor.Ins)}>";
        
        return type.Apply(CppUnderlyingDeclaringTypeNameVisitor.Ins);
    }
}
