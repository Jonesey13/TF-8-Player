﻿using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Xml.Linq;

namespace Patcher
{
	public class PatchAttribute : Attribute { }

    static class CecilExtensions
	{
		public static IEnumerable<TypeDefinition> AllNestedTypes(this TypeDefinition type)
		{
			yield return type;
			foreach (TypeDefinition nested in type.NestedTypes)
				foreach (TypeDefinition moarNested in AllNestedTypes(nested))
					yield return moarNested;
		}

		public static IEnumerable<TypeDefinition> AllNestedTypes(this ModuleDefinition module)
		{
			return module.Types.SelectMany(AllNestedTypes);
		}

		public static string Signature(this MethodReference method)
		{
			return string.Format("{0}({1})", method.Name, string.Join(", ", method.Parameters.Select(p => p.ParameterType)));
		}
	}

	public class Patcher
	{
		/// <summary>
		/// Marker method for calling the base implementation of a patched method
		/// </summary>
		public static void CallRealBase()
		{
		}

		/// <summary>
		/// Unseal, publicize, virtualize.
		/// </summary>
		static void MakeReferenceImage()
		{
			var module = ModuleDefinition.ReadModule("TowerFall.exe");
			foreach (var type in module.AllNestedTypes())
			{
				if (!type.FullName.StartsWith("TowerFall.") && !type.FullName.StartsWith("Monocle"))
				{
					continue;
				}
				if (type.Name.StartsWith("<>"))
				{
					continue;
				}
				if (type.IsNested)
					type.IsNestedPublic = true;
				if (type.IsValueType)
				{
					continue;
				}

				type.IsSealed = false;
				foreach (var field in type.Fields)
					field.IsPublic = true;
				foreach (var method in type.Methods)
				{
					method.IsPublic = true;
					if (!method.IsConstructor && !method.IsStatic)
						method.IsVirtual = true;
				}
			}
			module.Write("TowerFallReference.exe");
		}


		/// <summary>
		/// publicize
		/// </summary>
		static void MakeIntermediateImage()
		{
			var module = ModuleDefinition.ReadModule("TowerFall.exe");
			foreach (var type in module.AllNestedTypes())
			{
				if (!type.FullName.StartsWith("TowerFall.") && !type.FullName.StartsWith("Monocle"))
				{
					continue;
				}
				if (type.Name.StartsWith("<>"))
				{
					continue;
				}
				if (type.IsNested)
					type.IsNestedPublic = true;
				if (type.IsValueType)
				{
					continue;
				}

				foreach (var field in type.Fields)
					field.IsPublic = true;
			}
			module.Write("PatchedTowerFall.exe");
		}

		/// <summary>
		/// Inline classes marked as [Patch], copying fields and replacing method implementations.
		/// As you can probably guess from the code, this is wholly incomplete and will certainly break and have to be
		/// extended in the future.
		/// </summary>
		public static void Patch(string modModulePath)
		{
			var baseModule = ModuleDefinition.ReadModule("PatchedTowerFall.exe");

			var modModule = ModuleDefinition.ReadModule(modModulePath);

			Func<TypeReference, bool> patchType = (type) => {
				if (type.Scope == modModule)
				{
					return type.Resolve().CustomAttributes.Any(attr => attr.AttributeType.FullName == "Patcher.PatchAttribute");
				}
				return false;
			};

            // baseModule won't recognize MemberReferences from modModule without Import(), so recursively translate them.
            // Furthermore, we have to redirect any references to members in [Patch] classes.
            Func<TypeReference, TypeReference> mapType = null;
			mapType = (modType) => {
				if (modType.IsGenericParameter)
				{
					return modType;
				}
                if (modType.IsArray)
				{
					var type = mapType(((ArrayType)modType).ElementType);
					return new ArrayType(type);
				}
				if (patchType(modType))
					modType = modType.Resolve().BaseType;
                return baseModule.Import(modType);
			};
			Action<MethodReference, MethodReference> mapParams = (modMethod, method) => {
				foreach (var param in modMethod.Parameters)
					method.Parameters.Add(new ParameterDefinition(mapType(param.ParameterType)));
			};
			Func<MethodReference, MethodReference> mapMethod = (modMethod) => {
				var method = new MethodReference(modMethod.Name, mapType(modMethod.ReturnType), mapType(modMethod.DeclaringType));
				method.HasThis = modMethod.HasThis;
				mapParams(modMethod, method);
                return method as MethodReference;
			};
			Func<MethodDefinition, string, MethodDefinition> cloneMethod = (modMethod, prefix) => {
				var method = new MethodDefinition(prefix + modMethod.Name, modMethod.Attributes, mapType(modMethod.ReturnType));
				mapParams(modMethod, method);
				foreach (var modParam in modMethod.GenericParameters)
				{
					var param = new GenericParameter(modParam.Owner);
					method.GenericParameters.Add(param);
				}
				return method;
			};

            MyMInput.PatchModule(baseModule);
            MyMainMenu.PatchModule(baseModule);
            MyMenuButtons.PatchModule(baseModule);
            MyMenuInput.PatchModule(baseModule);
            MyPlayerInput.PatchModule(baseModule);
            MyReadyBanner.PatchModule(baseModule);
            MyRoundLogic.PatchModule(baseModule);
            MySession.PatchModule(baseModule);
            MyTFCommands.PatchModule(baseModule);
            MyTFGame.PatchModule(baseModule);
            MyTeamSelectOverlay.PatchModule(baseModule);
            MyVariant.PatchModule(baseModule);
            MyVersusAwards.PatchModule(baseModule);
            MyVersusRoundResults.PatchModule(baseModule);
            CleanMyVersusMatchResults.CleanModule(baseModule);
            CleanMyVersusPlayerMatchResults.CleanModule(baseModule);
            MyAwardInfo.PatchModule(baseModule);
            VersusPlayerMatchResultsAssembly.PatchModule(baseModule);

            foreach (TypeDefinition modType in modModule.Types.SelectMany(CecilExtensions.AllNestedTypes))
				if (patchType(modType))
				{
					var type = baseModule.AllNestedTypes().Single(t => t.FullName == modType.BaseType.FullName);

					// copy over fields including their custom attributes
					foreach (var field in modType.Fields)
						if (field.DeclaringType == modType)
						{
							var newField = new FieldDefinition(field.Name, field.Attributes, mapType(field.FieldType));
							foreach (var attribute in field.CustomAttributes)
								newField.CustomAttributes.Add(new CustomAttribute(mapMethod(attribute.Constructor), attribute.GetBlob()));
							type.Fields.Add(newField);
						}

					// copy over or replace methods
					foreach (var method in modType.Methods)
						if (method.DeclaringType == modType)
						{
							var original = type.Methods.SingleOrDefault(m => m.Signature() == method.Signature());
							MethodDefinition savedMethod = null;
							if (original == null)
								type.Methods.Add(original = cloneMethod(method, ""));
							else {
								savedMethod = cloneMethod(method, "$original_");
								savedMethod.Body = original.Body;
								savedMethod.IsRuntimeSpecialName = false;
								type.Methods.Add(savedMethod);
							}
							original.Body = method.Body;

							// redirect any references in the body
							var proc = method.Body.GetILProcessor();
							var amendments = new List<Action>();
							foreach (var instr in method.Body.Instructions)
							{
								if (instr.Operand is MethodReference)
								{
									var callee = (MethodReference)instr.Operand;
                                    if (callee.Name == "CallRealBase")
                                    {
                                        var baseMethod = type.BaseType.Resolve().Methods.Single(m => m.Name == method.Name) as MethodReference;
                                        amendments.Add(() => proc.InsertBefore(instr, proc.Create(OpCodes.Ldarg_0)));
                                        amendments.Add(() => proc.Replace(instr, proc.Create(OpCodes.Call, baseMethod)));
                                    }
                                    else {
                                        callee = mapMethod((MethodReference)instr.Operand);
                                        if (callee.FullName == original.FullName)
                                            // replace base calls with ones to $original
                                            instr.Operand = savedMethod;
                                        else
                                            instr.Operand = callee;
                                    }
								}
								else if (instr.Operand is FieldReference)
								{
									var field = (FieldReference)instr.Operand;
									instr.Operand = new FieldReference(field.Name, mapType(field.FieldType), mapType(field.DeclaringType));
								}
								else if (instr.Operand is TypeReference)
									instr.Operand = mapType((TypeReference)instr.Operand);
							}
							foreach (var var in method.Body.Variables)
								var.VariableType = mapType(var.VariableType);
							foreach (var amendment in amendments)
							{
								amendment();
							}
							method.Body = proc.Body;
						}
				}
            CleanMyVersusMatchResults.PatchModule(baseModule);
            baseModule.Write("TowerFall8Player.exe");
        }

        public static int Main(string[] args)
		{
			if (args.Length == 0)
			{
                MakeIntermediateImage();
                Patch("Mod.dll");
                File.Delete("PatchedTowerFall.exe");
                return -1;
			}
			else if (args[0] == "MakeReferenceImage")
			{
				MakeReferenceImage();
			}
			return 0;
		}
	}
}
