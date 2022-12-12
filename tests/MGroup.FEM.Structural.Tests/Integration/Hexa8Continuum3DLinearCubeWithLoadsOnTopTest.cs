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
                var solverFactory = new SkylineSolver.Factory();
                var algebraicModel = solverFactory.BuildAlgebraicModel(model);
                var solver = solverFactory.BuildSolver(algebraicModel);
                var problem = new ProblemStructural(model, algebraicModel);

                var loadControlAnalyzerBuilder = new LoadControlAnalyzer.Builder(algebraicModel, solver, problem, numIncrements: 2)
                {
                    ResidualTolerance = 0,
                    MaxIterationsPerIncrement = 100,
                    NumIterationsForMatrixRebuild = 1
                };

                var loadControlAnalyzer = loadControlAnalyzerBuilder.Build();
                var staticAnalyzer = new StaticAnalyzer(algebraicModel, problem, loadControlAnalyzer);

                var watchDofs = new List<(INode node, IDofType dof)>()
                {
                    (model.NodesDictionary[10], StructuralDof.TranslationZ),
                };

                var log1 = new TotalDisplacementsPerIterationLog(watchDofs, algebraicModel);
                loadControlAnalyzer.TotalDisplacementsPerIterationLog = log1;

                staticAnalyzer.Initialize();
                staticAnalyzer.Solve();

                var solution = new double[]
                {
                    log1.GetTotalDisplacement(10, watchDofs[0].node, watchDofs[0].dof),
                };

                return solution;
            }
    }
}