#!/bin/bash

if [ -n "$COVERALLS_REPO_TOKEN" ]
then
  dotnet tool install coveralls.net --version 2.0.0 --tool-path tools
  ./tools/csmacnz.Coveralls --opencover -i ./PlanningPoker.Client/PlanningPoker.Client.Tests/coverage.opencover.xml --useRelativePaths
fi