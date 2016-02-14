using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyPlayerInput
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.PlayerInput");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.PlayerInput::AssignInputs()");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            instructions.ForEach(i => ChangeThreesToSevens(i));
        }

        public static void ChangeFoursToEights(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_4)
            {
                i.OpCode = OpCodes.Ldc_I4_8;
            }
        }

        public static void ChangeThreesToSevens(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_3)
            {
                i.OpCode = OpCodes.Ldc_I4_7;
            }
        }
    }
}
