using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class VersusPlayerMatchResultsAssembly
    {
        static Instruction StoredKills;
        static Instruction StoredDeaths;
        static Instruction StoredSelf1;
        static Instruction StoredSelf2;
        static Instruction StoredAward;


        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusPlayerMatchResults");
            var subType = type.NestedTypes.Single(st => st.FullName.Contains("TowerFall.VersusPlayerMatchResults/<Sequence>"));
            var method = subType.Methods.Single(m => m.FullName.Contains("::MoveNext()"));
            var instructions = method.Body.Instructions.ToList();
            var proc = method.Body.GetILProcessor();
            instructions.ForEach(i => GetMethod(i, proc, baseModule));
        }

        public static void GetMethod(Instruction i, ILProcessor proc, ModuleDefinition baseModule)
        {
            if (i.Operand is FieldReference && ((FieldReference)i.Operand).FullName.Contains("::killsText"))
            {
                StoredKills = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Operand is FieldReference && ((FieldReference)i.Operand).FullName.Contains("::deathsText"))
            {
                StoredDeaths = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Operand is VariableDefinition && ((VariableDefinition)i.Operand).VariableType.FullName.Contains("TowerFall.VersusPlayerMatchResults/<>c"))
            {
                StoredSelf1 = Instruction.Create(OpCodes.Ldloc_S, i.Operand as VariableDefinition);
            }

            if (i.Operand is FieldReference && ((FieldReference)i.Operand).FullName.Contains("::selfText"))
            {
                StoredSelf2 = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Operand is VariableDefinition && ((VariableDefinition)i.Operand).VariableType.FullName.Contains("Monocle.OutlineText"))
            {
                StoredAward = Instruction.Create(OpCodes.Ldloc_S, i.Operand as VariableDefinition);
            }

            if (i.OpCode == OpCodes.Stfld && ((FieldReference)i.Operand).FullName.Contains("::killsText"))
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                var ldinstr = i.Next;
                proc.InsertAfter(i, instr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertAfter(i, StoredKills);
                proc.InsertAfter(i, ldinstr);
            }

            if (i.OpCode == OpCodes.Stfld && ((FieldReference)i.Operand).FullName.Contains("::deathsText"))
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                var ldinstr = i.Next;
                proc.InsertAfter(i, instr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertAfter(i, StoredDeaths);
                proc.InsertAfter(i, ldinstr);
            }

            if (i.OpCode == OpCodes.Stfld && ((FieldReference)i.Operand).FullName.Contains("::selfText"))
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertAfter(i, instr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertAfter(i, StoredSelf2);
                proc.InsertAfter(i, StoredSelf1);
            }

            if (i.OpCode == OpCodes.Stloc_S && i.Previous.OpCode == OpCodes.Newobj 
                && ((MethodReference)i.Previous.Operand).FullName.Contains("Monocle.OutlineText::.ctor"))
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertAfter(i, instr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.5));
                proc.InsertAfter(i, StoredAward);
            }
        }
    }
}
