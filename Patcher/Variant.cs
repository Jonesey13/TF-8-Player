using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Linq;

namespace Patcher
{
    public static class MyVariant
    {
        public static void PatchModule(ModuleDefinition baseModule)
        {
            var type = baseModule.AllNestedTypes().Single(t => t.FullName == "TowerFall.Variant");
            var method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.Variant::.ctor(Monocle.Subtexture,System.String,System.String,TowerFall.Pickups[],System.Boolean,System.String,System.Nullable`1<TowerFall.UnlockData/Unlocks>,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.Int32)");
            var instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.Variant::Clean(System.Int32)");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Boolean TowerFall.Variant::get_AllTrue()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Int32 TowerFall.Variant::get_Players()");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Void TowerFall.Variant::set_Value(System.Boolean)");
            instructions = method.Body.Instructions.ToList();
            instructions.ForEach(i => ChangeFoursToEights(i));
            method = type.Methods.Single(m => m.FullName == "System.Boolean TowerFall.Variant::get_Value()");
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
