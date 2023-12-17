using AdventOfCode2023;
using Autofac;

var builder = new ContainerBuilder();

builder.RegisterAssemblyTypes(typeof(Program).Assembly)
							 .Where(t => typeof(IChallenge).IsAssignableFrom(t))
							 .AsImplementedInterfaces();

using var container = builder.Build();

var challenge = container.Resolve<IEnumerable<IChallenge>>().OrderByDescending(x => x.Day).First();
challenge.Init();
ChallengeRunner.RunChallenge(challenge);
