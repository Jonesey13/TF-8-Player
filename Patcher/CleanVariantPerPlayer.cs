using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class CleanMyVariantPerPlayer
    {
        public static void CleanModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VariantPerPlayer");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.VariantPerPlayer::.ctor(TowerFall.VariantToggle,Microsoft.Xna.Framework.Vector2)");
            var firstInstruction = method.Body.Instructions.First();
            var secondInstruction = method.Body.Instructions[1];
            var thirdInstruction = method.Body.Instructions[2];
            var lastInstruction = method.Body.Instructions.Last();
            method.Body.Instructions.Clear();
            method.Body.Instructions.Add(firstInstruction);
            method.Body.Instructions.Add(secondInstruction);
            method.Body.Instructions.Add(thirdInstruction);
            method.Body.Instructions.Add(lastInstruction);
        }
    }
}