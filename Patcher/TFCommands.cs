using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyTFCommands
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.TFCommands");
            Mono.Cecil.MethodDefinition method;
            try {
                method = type.Methods.Single(m => m.FullName.Contains("System.Void TowerFall.TFCommands::<Init>b__4(System.String[])"));
            }
            catch (System.InvalidOperationException)
            {
                var subType = type.NestedTypes.Single(st => st.FullName == "TowerFall.TFCommands/<>c");
                method = subType.Methods.Single(m => m.FullName.Contains("System.Void TowerFall.TFCommands/<>c::<Init>b__0_4(System.String[])"));
            }
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
