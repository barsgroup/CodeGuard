using Mono.Cecil;
using Xunit;

namespace BarsGroup.CodeGuard.Fody.Tests
{
    public class FodyTest
    {
        [Fact]
        public void Test1()
        {
            var weaver = new ModuleWeaver
            {
                ModuleDefinition = ModuleDefinition.ReadModule("BarsGroup.CodeGuard.Tests.dll")
            };

            weaver.Execute();
        }
    }
}
