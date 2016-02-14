using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyMenuButtons
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.MenuButtons");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.MenuButtons::Update()");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
        }

        public static void ChangeFoursToEights(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_4)
            {
                i.OpCode = OpCodes.Ldc_I4_8;
            }
        }
    }
}
