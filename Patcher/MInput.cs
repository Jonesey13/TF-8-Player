using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyMInput
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "Monocle.MInput");
            var method = type.Methods.Single(m => m.FullName == "System.Void Monocle.MInput::UpdateJoysticks()");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
        }

        public static void ChangeFoursToEights(Instruction i)
        {
            var ILLines = new List<int> { 223, 455 };
            if (i.OpCode.Code == Code.Ldc_I4_4 && ILLines.Any(num => num ==i.Offset))
            {
                i.OpCode = OpCodes.Ldc_I4_8;
            }
        }
    }
}
