#!/bin/bash

if [ -n "$COVERALLS_REPO_TOKEN" ]
then
  nuget install -OutputDirectory packages -Version 0.7.0 coveralls.net
  packages/coveralls.net.0.7.0/tools/csmacnz.Coveralls.exe --opencover -i ./PlanningPoker.Client/PlanningPoker.Client.Tests/coverage.opencover.xml --useRelativePaths
fi