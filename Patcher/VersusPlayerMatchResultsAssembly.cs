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
            var subType = type.NestedTypes.Single(st => st.FullName == "TowerFall.VersusPlayerMatchResults/<Sequence>d__1a");
            var method = subType.Methods.Single(m => m.FullName == "System.Boolean TowerFall.VersusPlayerMatchResults/<Sequence>d__1a::MoveNext()");
            var instructions = method.Body.Instructions.ToList();
            var proc = method.Body.GetILProcessor();
            instructions.ForEach(i => GetMethod(i, proc, baseModule));
        }

        public static void GetMethod(Instruction i, ILProcessor proc, ModuleDefinition baseModule)
        {
            if (i.Offset == 283)
            {
                StoredKills = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Offset == 652)
            {
                StoredDeaths = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Offset == 1067)
            {
                StoredSelf1 = Instruction.Create(OpCodes.Ldloc_S, i.Operand as VariableDefinition);
            }

            if (i.Offset == 1081)
            {
                StoredSelf2 = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Offset == 1970)
            {
                StoredAward = Instruction.Create(OpCodes.Ldloc_S, i.Operand as VariableDefinition);
            }

            if (i.Offset == 293)
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldloc_1));
                proc.InsertBefore(i, StoredKills);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertBefore(i, instr);
            }

            if (i.Offset == 667)
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldloc_3));
                proc.InsertBefore(i, StoredDeaths);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertBefore(i, instr);
            }

            if (i.Offset == 1221 || i.Offset == 1086)
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertBefore(i, StoredSelf1);
                proc.InsertBefore(i, StoredSelf2);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldc_R4, (float)0.6));
                proc.InsertBefore(i, instr);
            }

            if (i.Offset == 1982)
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertBefore(i, StoredAward);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldc_R4, (float)0.5));
                proc.InsertBefore(i, instr);
            }
        }
    }
}
