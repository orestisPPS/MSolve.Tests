using System.Collections.Generic;
using System.Linq;
using MGroup.Constitutive.Structural;
using MGroup.Constitutive.Structural.BoundaryConditions;
using MGroup.Constitutive.Structural.Continuum;
using MGroup.Constitutive.Structural.Transient;
using MGroup.FEM.Structural.Continuum;
using MGroup.MSolve.Discretization;
using MGroup.MSolve.Discretization.Entities;
using MGroup.FEM.Helpers;


namespace MGroup.FEM.Structural.Tests.ExampleModels
{
	public class Hexa8Continuum3DLinearCubeWithLoadsOnTopExample
	{
		public static Model CreateModel()
		{
			var model = new Model();
			
			//Create nodes and assign them to the nodes dictionary(Key: Node global id, Value: Node)
			int nodeIndex = -1;
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 2.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 1.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 0.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 2.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 1.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 0.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 2.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 1.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 2.0, y: 0.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 2.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 1.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 0.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 2.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 1.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 0.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 2.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 1.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 1.0, y: 0.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 2.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 1.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 0.0, z: 2.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 2.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 1.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 0.0, z: 1.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 2.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 1.0, z: 0.0);
			model.NodesDictionary[++nodeIndex] = new Node(id: nodeIndex, x: 0.0, y: 0.0, z: 0.0);
			
			
			//Assign nodes to elements. Correct ordering is not guaranteed. Reordering needs to be applied
			var elementNodes = new INode[][]
			{
				new[] { model.NodesDictionary[13], model.NodesDictionary[4],  model.NodesDictionary[3],  model.NodesDictionary[12], model.NodesDictionary[10], model.NodesDictionary[1], model.NodesDictionary[0], model.NodesDictionary[9] },
				new[] { model.NodesDictionary[14], model.NodesDictionary[5],  model.NodesDictionary[4],  model.NodesDictionary[13], model.NodesDictionary[11], model.NodesDictionary[2], model.NodesDictionary[1], model.NodesDictionary[10] },
				new[] { model.NodesDictionary[16], model.NodesDictionary[7],  model.NodesDictionary[6],  model.NodesDictionary[15], model.NodesDictionary[13], model.NodesDictionary[4], model.NodesDictionary[3], model.NodesDictionary[12] },
				new[] { model.NodesDictionary[17], model.NodesDictionary[8],  model.NodesDictionary[7],  model.NodesDictionary[16], model.NodesDictionary[14], model.NodesDictionary[5], model.NodesDictionary[4], model.NodesDictionary[13] },
				new[] { model.NodesDictionary[22], model.NodesDictionary[13], model.NodesDictionary[12], model.NodesDictionary[21], model.NodesDictionary[19], model.NodesDictionary[10], model.NodesDictionary[9], model.NodesDictionary[18] },
				new[] { model.NodesDictionary[23], model.NodesDictionary[14], model.NodesDictionary[13], model.NodesDictionary[22], model.NodesDictionary[20], model.NodesDictionary[11], model.NodesDictionary[10], model.NodesDictionary[19] },
				new[] { model.NodesDictionary[25], model.NodesDictionary[16], model.NodesDictionary[15], model.NodesDictionary[24], model.NodesDictionary[22], model.NodesDictionary[13], model.NodesDictionary[12], model.NodesDictionary[21] },
				new[] { model.NodesDictionary[26], model.NodesDictionary[17], model.NodesDictionary[16], model.NodesDictionary[25], model.NodesDictionary[23], model.NodesDictionary[14], model.NodesDictionary[13], model.NodesDictionary[22] },
			};
			
			//Apply node reordering
			var nodeReordering = new GMeshElementLocalNodeOrdering();
			var rearrangeNodes = elementNodes.Select(x => nodeReordering.ReorderNodes(x, CellType.Hexa8)).ToArray();
			
			// Create linear elastic isotropic material
			var E = 1E6; // Young's modulus [Pa]
			var v = 0.3; // Poisson's ratio
			var elasticMaterial = new ElasticMaterial3D(youngModulus: E, poissonRatio: v);
			var dynamicMaterial = new TransientAnalysisProperties(density: 1, rayleighCoeffMass: 0, rayleighCoeffStiffness: 0);
			var elementFactory = new ContinuumElement3DFactory(elasticMaterial, dynamicMaterial);
			
			// create elements
			for (int i = 0; i < elementNodes.Length; i++)
			{
				model.ElementsDictionary[i] = elementFactory.CreateElement(CellType.Hexa8, rearrangeNodes[i]);
				model.ElementsDictionary[i].ID = i;
				model.SubdomainsDictionary[0].Elements.Add(model.ElementsDictionary[i]);
			}
			
			// Create computational subdomains (1 subdomain in most cases)
			model.SubdomainsDictionary.Add(key: 0, new Subdomain(id: 0));
			
			//Apply Dirichlet BC u(x,y,0) = 0
			var dirichlet = new List<INodalDisplacementBoundaryCondition>();
			foreach (var node in model.NodesDictionary.Values.Where(node => node.Z <= 1E-6))
			{
				dirichlet.Add(new NodalDisplacement(node, StructuralDof.TranslationX, amount: 0d));
				dirichlet.Add(new NodalDisplacement(node, StructuralDof.TranslationY, amount: 0d));
				dirichlet.Add(new NodalDisplacement(node, StructuralDof.TranslationZ, amount: 0d));
			}
			
			//Apply Neumann BC @ z = 2 (Point Loads)
			var neumann = new List<INodalLoadBoundaryCondition>();
			foreach (var node in model.NodesDictionary.Values.Where(node => node.Z >= 1.999999))
			{
				switch (node.X - 1)
				{
					case <= 1 when node.Y - 1 <= 1:
						neumann.Add(new NodalLoad(node, StructuralDof.TranslationZ, -1));
						break;
					case <= 2 when node.Y - 1 <= 2:
						neumann.Add(new NodalLoad(node, StructuralDof.TranslationZ, -2));
						break;
				}
			}
			model.BoundaryConditions.Add(new StructuralBoundaryConditionSet(dirichlet, neumann));
			return model;
		}

		public static IReadOnlyList<double[]> expectedSolution()
		{
			return new double[][]
			{

			};
		}

	}
}
