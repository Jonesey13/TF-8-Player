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
            var subType = type.NestedTypes.Single(st => st.FullName == "TowerFall.VersusRoundResults/<Sequence>d__3");
            method = subType.Methods.Single(m => m.FullName == "System.Boolean TowerFall.VersusRoundResults/<Sequence>d__3::MoveNext()");
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
            if (i.OpCode.Code == Code.Ldc_I4_3 && i.Offset == 1019 )
            {
                i.OpCode = OpCodes.Ldc_I4_7;
            }
        }
    }
}