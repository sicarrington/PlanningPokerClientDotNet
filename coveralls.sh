#!/bin/bash

if [ -n "$COVERALLS_REPO_TOKEN" ]
then
  dotnet tool install coveralls.net --version 1.0.0 --tools-path tools
  ./tools/csmacnz.Coveralls --opencover -i ./PlanningPoker.Client/PlanningPoker.Client.Tests/coverage.opencover.xml --useRelativePaths

  # nuget install -OutputDirectory packages -Version 0.7.0 coveralls.net
  # packages/coveralls.net.0.7.0/tools/csmacnz.Coveralls.exe --opencover -i ./PlanningPoker.Client/PlanningPoker.Client.Tests/coverage.opencover.xml --useRelativePaths
fi