using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyRoundLogic
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.RoundLogic");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.RoundLogic::.ctor(TowerFall.Session,System.Boolean)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.RoundLogic::FinalKillTeams(TowerFall.PlayerCorpse,TowerFall.Allegiance)");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Int32 TowerFall.RoundLogic::SpawnPlayersFFA()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.RoundLogic::SpawnPlayersTeams()");
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
