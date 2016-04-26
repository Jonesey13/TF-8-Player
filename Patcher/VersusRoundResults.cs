using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyVersusRoundResults
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusRoundResults");
            var method = type.Methods.Single(m => m.FullName == "System.Collections.IEnumerator TowerFall.VersusRoundResults::Sequence(System.Collections.Generic.List`1<TowerFall.EventLog>)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeThreesToSevens(i));
            var subType = type.NestedTypes.Single(st => st.FullName.Contains("TowerFall.VersusRoundResults/<Sequence>"));
            method = subType.Methods.Single(m => m.FullName.Contains("::MoveNext()"));
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeThreeToSeven(i));
        }

        public static void ChangeThreesToSevens(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_3)
            {
                i.OpCode = OpCodes.Ldc_I4_7;
            }
        }

        public static void ChangeThreeToSeven(Instruction i)
        {
            if (i.OpCode.Code == Code.Ldc_I4_3 && i.Previous.OpCode == OpCodes.Stloc_S && i.Next.OpCode == OpCodes.Stloc_S)
            {
                i.OpCode = OpCodes.Ldc_I4_7;
            }
        }
    }
}