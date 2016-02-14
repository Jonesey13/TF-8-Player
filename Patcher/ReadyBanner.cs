using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyReadyBanner
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.ReadyBanner");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.ReadyBanner::.ctor()");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.ReadyBanner::GetButtons()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.ReadyBanner::Update()");
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
