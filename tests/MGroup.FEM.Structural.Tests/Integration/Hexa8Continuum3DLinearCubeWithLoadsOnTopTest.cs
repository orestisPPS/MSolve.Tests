using System.Collections.Generic;
using MGroup.Constitutive.Structural;
using MGroup.FEM.Structural.Tests.Commons;
using MGroup.FEM.Structural.Tests.ExampleModels;
using MGroup.MSolve.Discretization.Entities;
using MGroup.NumericalAnalyzers;
using MGroup.LinearAlgebra.Vectors;
using MGroup.MSolve.Discretization.Dofs;
using MGroup.NumericalAnalyzers.Discretization.NonLinear;
using MGroup.NumericalAnalyzers.Logging;
using MGroup.NumericalAnalyzers.Dynamic;
using MGroup.Solvers.Direct;
using MGroup.NumericalAnalyzers.Logging;
using MGroup.Solvers.Iterative;
using Xunit;

namespace MGroup.FEM.Structural.Tests.Integration
{
    
    public class Hexa8Continuum3DLinearCubeWithLoadsOnTopTest
    {
        [Fact]
        private static void RunTest()
        {
            var model = Hexa8Continuum3DLinearCubeWithLoadsOnTopExample.CreateModel();
            var computedDisplacements = SolveModel(model);
            Assert.True(Utilities.AreDisplacementsSame(Hexa8Continuum3DLinearCubeWithLoadsOnTopExample.GetExpectedDisplacements(), computedDisplacements, tolerance: 1E-3));
        }

            private static double[] SolveModel(Model model)
            {
                var watchDofs = new List<(INode node, IDofType dof)>();
                var solverFactory = new SkylineSolver.Factory();
                //var solverFactory = new PcgSolver.Factory();
             
                var algebraicModel = solverFactory.BuildAlgebraicModel(model);
                var solver = solverFactory.BuildSolver(algebraicModel);
                var problem = new ProblemStructural(model, algebraicModel);
                
                var linearAnalyzer = new LinearAnalyzer(algebraicModel, solver, problem);
                var staticAnalyzer = new StaticAnalyzer(algebraicModel, problem, linearAnalyzer);
                
                watchDofs.Add((model.NodesDictionary[10], StructuralDof.TranslationZ));


                linearAnalyzer.LogFactory = new LinearAnalyzerLogFactory(watchDofs, algebraicModel);

                staticAnalyzer.Initialize();
                staticAnalyzer.Solve();

                
                DOFSLog log = (DOFSLog)linearAnalyzer.Logs[0];
                
                var solution = new double[] { log.DOFValues[watchDofs[0].node, watchDofs[0].dof] };

                return solution;
            }
    }
}