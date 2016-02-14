using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyTFGame
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.TFGame");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.TFGame::.cctor()");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Boolean TowerFall.TFGame::CharacterTaken(System.Int32)");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.TFGame::Initialize()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.TFGame::Load()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Int32 TowerFall.TFGame::get_FirstPlayer()");
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
