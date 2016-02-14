using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class CleanMyVersusPlayerMatchResults
    {
        public static void CleanModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusPlayerMatchResults");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.VersusPlayerMatchResults::.ctor(TowerFall.Session,TowerFall.VersusMatchResults,System.Int32,Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Vector2,System.Collections.Generic.List`1<TowerFall.AwardInfo>)");
            var firstInstruction = method.Body.Instructions.First();
            var secondInstruction = method.Body.Instructions[1];
            var lastInstruction = method.Body.Instructions.Last();
            method.Body.Instructions.Clear();
            method.Body.Instructions.Add(firstInstruction);
            method.Body.Instructions.Add(secondInstruction);
            method.Body.Instructions.Add(lastInstruction);
        }
    }
}
