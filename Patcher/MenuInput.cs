using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyMenuInput
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.MenuInput");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.MenuInput::UpdateInputs()");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            instructions.ForEach(i => ChangeFivesToNines(i));
        }

        public static void ChangeFoursToEights(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_4)
            {
                i.OpCode = OpCodes.Ldc_I4_8;
            }
        }

        public static void ChangeFivesToNines(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_5)
            {
                i.OpCode = OpCodes.Ldc_I4_S;
                i.Operand = (sbyte)9;
            }
        }
    }
}