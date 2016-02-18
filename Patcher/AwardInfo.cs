using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyAwardInfo
    {
        static Instruction Stored;

        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.AwardInfo");
            var method = type.Methods.Single(m => m.FullName == "Monocle.Sprite`1<System.Int32> TowerFall.AwardInfo::GetSprite(System.Boolean)");
            var instructions = method.Body.Instructions.ToList();
            var proc = method.Body.GetILProcessor();
            instructions.ForEach(i => GetMethod(i, proc, baseModule));
        }

        public static void GetMethod(Instruction i, ILProcessor proc, ModuleDefinition baseModule)
        {
            if (i.Offset == 165)
            {
                Stored = Instruction.Create(OpCodes.Ldfld, i.Operand as FieldReference);
            }

            if (i.Offset == 175)
            {
                TypeDefinition graphicsComponent = baseModule.GetType("Monocle.GraphicsComponent");
                 var fieldReference = graphicsComponent.Fields.Single(f => f.Name == "Zoom");
                var instr = Instruction.Create(OpCodes.Stfld, fieldReference);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldloc_3));
                proc.InsertBefore(i, Stored);
                proc.InsertBefore(i, proc.Create(OpCodes.Ldc_R4, (float)0.7));
                proc.InsertBefore(i, instr);
            }
        }
    }
}
