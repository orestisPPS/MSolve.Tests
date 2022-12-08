using System.Collections.Generic;
using MGroup.Constitutive.Structural;
using MGroup.FEM.Structural.Tests.ExampleModels;
using MGroup.MSolve.Discretization.Entities;
using MGroup.NumericalAnalyzers;
using MGroup.LinearAlgebra.Vectors;
using MGroup.MSolve.Discretization.Dofs;
using MGroup.NumericalAnalyzers.Dynamic;
using MGroup.Solvers.Direct;
using MGroup.NumericalAnalyzers.Logging;
using MGroup.Solvers.Iterative;
using Xunit;

namespace MGroup.FEM.Structural.Tests.Integration
{
    
    public class Hexa8Continuum3DLinearCubeWithLoadsOnTopTest
    {
            private static List<(INode node, IDofType dof)> watchDofs = new List<(INode node, IDofType dof)>();

            [Fact]
            public void RunTest()
            {
                var model = Hexa8Continuum3DLinearCubeWithLoadsOnTopExample.CreateModel();
                var solution = SolveModel(model);
                Assert.Equal(expected: Hexa8Continuum3DLinearCubeWithLoadsOnTopExample.expectedSolution(), precision: 12);
                
                Assert.Equal(MockStructuralModel.expected_solution_node0_TranslationX, log.DOFValues[watchDofs[0].node, watchDofs[0].dof], precision: 8);
                Assert.Equal(MockStructuralModel.expected_solution_node0_TranslationY, log.DOFValues[watchDofs[1].node, watchDofs[1].dof], precision: 8);
            }

            private static DOFSLog SolveModel(Model model)
            {
                var solverFactory = new SkylineSolver.Factory();
                //var solverFactory = new PcgSolver.Factory();
             
                var algebraicModel = solverFactory.BuildAlgebraicModel(model);
                var solver = solverFactory.BuildSolver(algebraicModel);
                var problem = new ProblemStructural(model, algebraicModel);
                
                var linearAnalyzer = new LinearAnalyzer(algebraicModel, solver, problem);
                var staticAnalyzer = new StaticAnalyzer(problem, linearAnalyzer, solver);
                
                watchDofs.Add((model.NodesDictionary[0], StructuralDof.TranslationX));
                watchDofs.Add((model.NodesDictionary[0], StructuralDof.TranslationY));
                linearAnalyzer.LogFactory = new LinearAnalyzerLogFactory(watchDofs, algebraicModel);
                
                staticAnalyzer.Initialize();
                staticAnalyzer.Solve();
                
                return (DOFSLog)linearAnalyzer.Logs[0];
            }
    }
}