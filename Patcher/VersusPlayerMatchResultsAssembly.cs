using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class VersusPlayerMatchResultsAssembly
    {
        static Instruction StoredSelf;
        static Instruction StoredAward;
        static string selfClassName;


        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.VersusPlayerMatchResults");
            var subType = type.NestedTypes.Single(st => st.FullName.Contains("TowerFall.VersusPlayerMatchResults/<Sequence>"));
            var method = subType.Methods.Single(m => m.FullName.Contains("::MoveNext()"));
            var instructions = method.Body.Instructions.ToList();
            var proc = method.Body.GetILProcessor();
            instructions.ForEach(i => GetMethodPrelim(i, proc, baseModule));
            instructions.ForEach(i => GetMethod(i, proc, baseModule));
        }

        public static void GetMethodPrelim(Instruction i, ILProcessor proc, ModuleDefinition baseModule)
        {
            if (i.Operand is FieldReference && ((FieldReference)i.Operand).FullName.Contains("::selfText"))
            {
                selfClassName = ((FieldReference)i.Operand).FullName.Split('/')[1].Split(':')[0];
            }
        }

        public static void GetMethod(Instruction i, ILProcessor proc, ModuleDefinition baseModule)
        {
            if (selfClassName != null && i.Operand is VariableDefinition && ((VariableDefinition)i.Operand).VariableType.FullName.Contains("TowerFall.VersusPlayerMatchResults/" + selfClassName))
            {
                StoredSelf = Instruction.Create(OpCodes.Ldloc_S, i.Operand as VariableDefinition);
            }

            if (i.Operand is VariableDefinition && ((VariableDefinition)i.Operand).VariableType.FullName.Contains("Monocle.OutlineText"))
            {
                StoredAward = Instruction.Create(OpCodes.Ldloc_S, i.Operand as VariableDefinition);
            }

            if (i.OpCode == OpCodes.Stfld && ((FieldReference)i.Operand).FullName.Contains("::killsText"))
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var textLdfldInstr = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
                var zoomFieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, zoomFieldReference);
                var ldinstr = i.Next;
                proc.InsertAfter(i, instr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertAfter(i, textLdfldInstr);
                proc.InsertAfter(i, ldinstr);
            }

            if (i.OpCode == OpCodes.Stfld && ((FieldReference)i.Operand).FullName.Contains("::deathsText"))
            {
                var textLdfldInstr = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var zoomFieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, zoomFieldReference);
                var ldinstr = i.Next;
                proc.InsertAfter(i, instr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertAfter(i, textLdfldInstr);
                proc.InsertAfter(i, ldinstr);
            }

            if (i.OpCode == OpCodes.Ldfld && ((FieldReference)i.Operand).FullName.Contains("::selfText")
                && i?.Next.OpCode == OpCodes.Call && ((MethodReference)i.Next.Operand).FullName.Contains("::get_Transparent")
                && i?.Next?.Next.OpCode == OpCodes.Stfld && ((FieldReference)i.Next.Next.Operand).FullName.Contains("::OutlineColor"))
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var zoomFieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var ldfldinstr = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
                var stfldinstr = Instruction.Create(OpCodes.Stfld, zoomFieldReference);
                proc.InsertAfter(i, stfldinstr);
                proc.InsertAfter(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertAfter(i, ldfldinstr);
                proc.InsertAfter(i, StoredSelf);
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
