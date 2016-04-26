using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class CleanMyVersusMatchResults
    {
        public static Instruction Stored1;
        public static Instruction Stored2;


        public static void CleanModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusMatchResults");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.VersusMatchResults::.ctor(TowerFall.Session,TowerFall.VersusRoundResults)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => GetMethod(i));
            var firstInstruction = method.Body.Instructions.First();
            var secondInstruction = method.Body.Instructions[1];
            var lastInstruction = method.Body.Instructions.Last();
            method.Body.Instructions.Clear();
            method.Body.Instructions.Add(firstInstruction);
            method.Body.Instructions.Add(secondInstruction);
            method.Body.Instructions.Add(lastInstruction);
        }

        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusMatchResults");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.VersusMatchResults::.ctor(TowerFall.Session,TowerFall.VersusRoundResults)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeMethod(i));
        }

        public static void GetMethod(Instruction i)
        {
            if (i.Operand is MethodReference && ((MethodReference)i.Operand).FullName == "T Monocle.Scene::Add<TowerFall.VersusPlayerMatchResults>(T)")
            {
                Stored1 = Instruction.Create(OpCodes.Callvirt, i.Operand as MethodReference);
            }
        }

        public static void ChangeMethod(Instruction i)
        {
            if (i.OpCode == OpCodes.Pop)
            {
                i.Previous.Operand = Stored1.Operand;
            }
        }
    }
}
