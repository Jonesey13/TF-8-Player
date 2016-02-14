using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyVersusAwards
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusAwards");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.VersusAwards::AssignAward(TowerFall.MatchSettings,TowerFall.AwardInfo)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Collections.Generic.List`1<TowerFall.AwardInfo>[] TowerFall.VersusAwards::GetAwards(TowerFall.MatchSettings,TowerFall.MatchStats[])");
            instructions = method.Body.Instructions.ToList();
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
