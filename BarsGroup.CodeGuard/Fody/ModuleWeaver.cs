using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Mono.Cecil;
using Mono.Cecil.Rocks;

public class ModuleWeaver
{
    private readonly MethodDefinition _guardThatMethod;

    public ModuleDefinition ModuleDefinition { get; set; }
    public IAssemblyResolver AssemblyResolver { get; set; }

    public ModuleWeaver()
    {
        var guardType = ModuleDefinition.ReadModule("BarsGroup.CodeGuard.dll").GetType("BarsGroup.CodeGuard", "Guard");
        _guardThatMethod = guardType.Methods.Single(m => m.Name == "That");
    }

    public void Execute()
    {
        //ReadConfig();

        //var nullGuardAttribute = ModuleDefinition.GetNullGuardAttribute();

        //if (nullGuardAttribute == null)
        //    nullGuardAttribute = ModuleDefinition.Assembly.GetNullGuardAttribute();

        //if (nullGuardAttribute != null)
        //    ValidationFlags = (ValidationFlags)nullGuardAttribute.ConstructorArguments[0].Value;

        //ReferenceFinder.FindReferences(AssemblyResolver, ModuleDefinition);
        var types = GetTypesToProcess();

        //CheckForBadAttributes(types);
        ProcessAssembly(types);
        //RemoveAttributes(types);
        //RemoveReference();
    }

    private List<TypeDefinition> GetTypesToProcess()
    {
        var allTypes = new List<TypeDefinition>(ModuleDefinition.GetTypes());

        return allTypes.Where(t => !t.IsInterface && t.Methods.Any()).ToList();
    }

    //void ReadConfig()
    //{
    //    if (Config == null)
    //    {
    //        return;
    //    }

    //    ReadIncludeDebugAssert();
    //    ReadExcludeRegex();
    //}

    //void ReadIncludeDebugAssert()
    //{
    //    var includeDebugAssertAttribute = Config.Attribute("IncludeDebugAssert");
    //    if (includeDebugAssertAttribute != null)
    //    {
    //        if (!bool.TryParse(includeDebugAssertAttribute.Value, out IncludeDebugAssert))
    //        {
    //            throw new WeavingException($"Could not parse 'IncludeDebugAssert' from '{includeDebugAssertAttribute.Value}'.");
    //        }
    //    }
    //}

    //void ReadExcludeRegex()
    //{
    //    var attribute = Config.Attribute("ExcludeRegex");
    //    var regex = attribute?.Value;
    //    if(!string.IsNullOrWhiteSpace(regex))
    //    { 
    //        ExcludeRegex = new Regex(regex, RegexOptions.Compiled | RegexOptions.CultureInvariant); 
    //    }
    //}

    //void CheckForBadAttributes(List<TypeDefinition> types)
    //{
    //    foreach (var typeDefinition in types)
    //    {
    //        foreach (var method in typeDefinition.AbstractMethods())
    //        {
    //            if (method.ContainsAllowNullAttribute())
    //            {
    //                LogError($"Method '{method.FullName}' is abstract but has a [AllowNullAttribute]. Remove this attribute.");
    //            }
    //            foreach (var parameter in method.Parameters)
    //            {
    //                if (parameter.ContainsAllowNullAttribute())
    //                {
    //                    LogError($"Method '{method.FullName}' is abstract but has a [AllowNullAttribute]. Remove this attribute.");
    //                }
    //            }
    //        }
    //    }
    //}

    void ProcessAssembly(List<TypeDefinition> types)
    {
        //var isDebug = IncludeDebugAssert && DefineConstants.Any(c => c == "DEBUG") && ReferenceFinder.DebugAssertMethod != null;

        var methodProcessor = new MethodProcessor();
        //var propertyProcessor = new PropertyProcessor(ValidationFlags, isDebug);

        foreach (var method in types.SelectMany(t => t.MethodsWithBody()))
        {
            methodProcessor.Process(_guardThatMethod, method);

            //foreach (var property in type.ConcreteProperties())
            //    propertyProcessor.Process(property);
        }
    }

    void RemoveAttributes(List<TypeDefinition> types)
    {
        ModuleDefinition.Assembly.RemoveAllNullGuardAttributes();
        ModuleDefinition.RemoveAllNullGuardAttributes();
        foreach (var typeDefinition in types)
        {
            typeDefinition.RemoveAllNullGuardAttributes();

            foreach (var method in typeDefinition.Methods)
            {
                method.MethodReturnType.RemoveAllNullGuardAttributes();

                foreach (var parameter in method.Parameters)
                {
                    parameter.RemoveAllNullGuardAttributes();
                }
            }

            foreach (var property in typeDefinition.Properties)
            {
                property.RemoveAllNullGuardAttributes();
            }
        }
    }

    void RemoveReference()
    {
        //var referenceToRemove = ModuleDefinition.AssemblyReferences.FirstOrDefault(x => x.Name == "NullGuard");
        //if (referenceToRemove == null)
        //{
        //    LogInfo("\tNo reference to 'NullGuard.dll' found. References not modified.");
        //    return;
        //}

        //ModuleDefinition.AssemblyReferences.Remove(referenceToRemove);
        //LogInfo("\tRemoving reference to 'NullGuard.dll'.");
    }
}